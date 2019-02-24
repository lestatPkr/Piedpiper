﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using EventStore.ClientAPI.SystemData;
using Piedpiper.Framework;

namespace Piedpiper.Infrastructure.EventStore
{
    public class GesAggregateStore : IAggregateStore
    {
        const int MaxReadSize = 4096;

        static readonly Serilog.ILogger Log = Serilog.Log.ForContext<GesAggregateStore>();

        readonly IEventStoreConnection _connection;
        readonly GetStreamName _getStreamName;
        readonly ISerializer _serializer;
        readonly TypeMapper _typeMapper;
        readonly UserCredentials _userCredentials;

        public GesAggregateStore(
            GetStreamName getStreamName,
            IEventStoreConnection connection,
            ISerializer serializer,
            TypeMapper typeMapper,
            UserCredentials userCredentials = null)
        {
            _getStreamName = getStreamName ?? throw new ArgumentNullException(nameof(connection));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _typeMapper = typeMapper ?? throw new ArgumentNullException(nameof(typeMapper));
            _userCredentials = userCredentials;
        }

        /// <summary>
        ///     Loads and returns an aggregate by id, from the store.
        /// </summary>
        public async Task<T> Load<T>(string aggregateId, CancellationToken cancellationToken = default)
            where T : Aggregate, new()
        {
            if (String.IsNullOrWhiteSpace(aggregateId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(aggregateId));

            var stream = _getStreamName(typeof(T), aggregateId);
            var aggregate = new T();

            var nextPageStart = 0L;
            do
            {
                var page = await _connection.ReadStreamEventsForwardAsync(
                    stream, nextPageStart, MaxReadSize, false, _userCredentials);

                if (page.Events.Length > 0)
                {
                    aggregate.Load(
                        page.Events.Last().Event.EventNumber,
                        page.Events.Select(re => _serializer.Deserialize(
                            re.Event.Data, _typeMapper.GetType(re.Event.EventType)
                        )).ToArray());
                }

                nextPageStart = !page.IsEndOfStream ? page.NextEventNumber : -1;
            } while (nextPageStart != -1);

            Log.Debug("Loaded {aggregate} changes from stream {stream}", aggregate, stream);

            return aggregate;
        }

        /// <summary>
        ///     Saves changes to the store.
        /// </summary>
        public async Task<(long NextExpectedVersion, long LogPosition, long CommitPosition )> Save<T>(
            T aggregate,  CancellationToken cancellationToken = default)
            where T : Aggregate
        {
            if (aggregate == null)
                throw new ArgumentNullException(nameof(aggregate));

            var changes = aggregate.GetChanges()
                .Select(e => new EventData(
                    Guid.NewGuid(),
                    _typeMapper.GetTypeName(e.GetType()),
                    _serializer.IsJsonSerializer,
                    _serializer.Serialize(e),
                    null))
                .ToArray();

            if (!changes.Any())
            {
                Log.Information("{Id} v{Version} aggregate has no changes.", aggregate.Id, aggregate.Version);
                return default;
            }

            var stream = _getStreamName(typeof(T), aggregate.Id.ToString());

            WriteResult result;
            try
            {
                result = await _connection.AppendToStreamAsync(stream, aggregate.Version, changes, _userCredentials);
            }
            catch (WrongExpectedVersionException)
            {
                var page = await _connection.ReadStreamEventsBackwardAsync(stream, -1, 1, false, _userCredentials);
                throw new WrongExpectedStreamVersionException(
                    $"Failed to append stream {stream} with expected version {aggregate.Version}. " +
                    $"{(page.Status == SliceReadStatus.StreamNotFound ? "Stream not found!" : $"Current Version: {page.LastEventNumber}")}");
            }

            Log.Debug("Saved {count} {aggregate} change(s) into stream {streamName}", changes.Length, aggregate, stream);

            foreach (var change in aggregate.GetChanges())
                Log.Information(change.ToString());

            return (
                result.NextExpectedVersion,
                result.LogPosition.CommitPosition,
                result.LogPosition.PreparePosition);
        }

        /// <summary>
        ///     Returns the last version of the aggregate, if found.
        /// </summary>
        public async Task<long> GetLastVersionOf<T>(string aggregateId, CancellationToken cancellationToken = default)
            where T : Aggregate
        {
            if (String.IsNullOrWhiteSpace(aggregateId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(aggregateId));

            var page = await _connection.ReadStreamEventsBackwardAsync(
                _getStreamName(typeof(T), aggregateId), long.MaxValue, 1, false, _userCredentials);

            return page.LastEventNumber;
        }
    }
}
