using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Piedpiper.Domain.Inverstors;
using Piedpiper.Framework;
using Piedpiper.Host.Modules.Investors;
using Piedpiper.Host.Modules.Investors.Projections;
using Piedpiper.Infrastructure.EventStore;
using Piedpiper.Infrastructure.JsonNet;
using Piedpiper.Infrastructure.RavenDB;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Piedpiper.Host
{
    public class Startup
    {
        static readonly Serilog.ILogger Log = Serilog.Log.ForContext<Startup>();

       

        public Startup(IHostingEnvironment environment)
        {
            Environment = environment;
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            Serilog.Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        IConfiguration Configuration { get; }
        IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
            => ConfigureServicesAsync(services).GetAwaiter().GetResult();

        async Task ConfigureServicesAsync(IServiceCollection services)
        {
            var gesConnection = EventStoreConnection.Create(
                Configuration["EventStore:ConnectionString"],
                ConnectionSettings.Create().KeepReconnecting(),
                Environment.ApplicationName);

            gesConnection.Connected += (sender, args)
                => Log.Information("Connection to {endpoint} event store established.", args.RemoteEndPoint);

            await gesConnection.ConnectAsync();

            var serializer = new JsonNetSerializer();

            var typeMapper = new TypeMapper()
                .Map<Events.V1.InvestorNameChanged>(nameof(Events.V1.InvestorNameChanged))
                .Map<Events.V1.InvestorRegistered>(nameof(Events.V1.InvestorRegistered))
                .Map<Events.V1.CompanyRegistered>(nameof(Events.V1.CompanyRegistered))
                .Map<Events.V1.CompanyScoreChanged>(nameof(Events.V1.CompanyScoreChanged));


            var aggregateStore = new GesAggregateStore(
                (type, id) => $"{type.Name}-{id}",
                gesConnection,
                serializer,
                typeMapper);

           

            services.AddSingleton(new InvestorsApplicationService(
                aggregateStore, () => DateTimeOffset.UtcNow));

            var documentStore = ConfigureRaven();

            IAsyncDocumentSession GetSession() => documentStore.OpenAsyncSession();

            services.AddSingleton<Func<IAsyncDocumentSession>>(GetSession);

            services.AddSingleton(new InvestorsQueryService(GetSession));

            await ProjectionManager.With
                .Connection(gesConnection)
                .Serializer(serializer)
                .TypeMapper(typeMapper)
                .CheckpointStore(new RavenCheckpointStore(GetSession))
                .Projections(
                    new InvestorsProjection(GetSession),
                    new InvestorDashboardProjection(GetSession))
                .Activate();

            services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.DescribeStringEnumsInCamelCase();
                //c.AddSecurityDefinition("Piedpiper", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = "apiKey" });
                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                //    { "Bearer", Enumerable.Empty<string>() },
                //});
                c.SwaggerDoc("v1", new Info { Title = "Piedpiper api", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment environment)
        {
            app.UseExceptionMiddleware();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint(
                Configuration["Swagger:Endpoint:Url"],
                Configuration["Swagger:Endpoint:Name"]));
        }

        IDocumentStore ConfigureRaven()
        {
            var store = new DocumentStore
            {
                Urls = new[] { Configuration["RavenDb:Url"] },
                Database = Configuration["RavenDb:Database"]
            };

            if (Environment.IsDevelopment()) store.OnBeforeQuery += (_, args)
                => args.QueryCustomization
                    .WaitForNonStaleResults()
                    .AfterQueryExecuted(result =>
                    {
                        Log.ForContext("SourceContext", "Raven").Debug("{index} took {duration}", result.IndexName, result.DurationInMs);
                    });

            try
            {
                store.Initialize();
                Log.Information("Connection to {url} document store established.", store.Urls[0]);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Failed to establish connection to \"{store.Urls[0]}\" document store!" +
                    $"Please check if https is properly configured in order to use the certificate.", ex);
            }

            try
            {
                var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
                if (record == null)
                {
                    store.Maintenance.Server
                        .Send(new CreateDatabaseOperation(new DatabaseRecord(store.Database)));

                    Log.Debug("{database} document store database created.", store.Database);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    $"Failed to ensure that \"{store.Database}\" document store database exists!", ex);
            }

            try
            {
                IndexCreation.CreateIndexes(Assembly.GetExecutingAssembly(), store);
                Log.Information("{database} document store database indexes created or updated.", store.Database);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to create or update \"{store.Database}\" document store database indexes!", ex);
            }

            return store;
        }
    }
}
