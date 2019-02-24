//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Raven.Client.Documents;
//using Raven.Client.Documents.Linq;
//using Raven.Client.Documents.Session;

//namespace Piedpiper.Host.Modules.ClassifiedAds
//{
//    public class InvestorsQueryService
//    {
//        Func<IAsyncDocumentSession> GetSession { get; }

//        public InvestorsQueryService(Func<IAsyncDocumentSession> getSession)
//            => GetSession = getSession;

//        public async Task<Piedpiper.Contracts.Investors.V1.GetInvestor.Result> GetInvestor(
//            Piedpiper.Contracts.Investors.V1.GetInvestor query, CancellationToken cancellationToken)
//        {
//            using (var session = GetSession())
//            {
//                if (query.Page <= 0) query.Page = 1;
//                if (query.PageSize <= 0) query.PageSize = 10;

//                var result = await session.Query<Projections.AvailableClassifiedAdsProjection.AvailableClassifiedAd>()
//                    .Where(x => x.PublishedAt != null)
//                    .OrderByDescending(x => x.PublishedAt)
//                    .Statistics(out var statistics)
//                    .Skip((query.Page - 1) * query.PageSize)
//                    .Take(query.PageSize)
//                    .Select(x => new Piedpiper.Contracts.Investors.V1.GetInvestor.Result.Item
//                    {
//                        ClassifiedAdId = x.ClassifiedAdId,
//                        Title = x.Title,
//                        Text = x.Text,
//                        Price = x.Price,
//                        Owner = x.Owner,
//                        PublishedAt = x.PublishedAt.Value
//                    })
//                    .ToListAsync(cancellationToken);

//                return new Piedpiper.Contracts.Investors.V1.GetInvestor.Result
//                {
//                    Page = query.Page,
//                    PageSize = query.PageSize,
//                    TotalItems = statistics.TotalResults - statistics.SkippedResults,
//                    TotalPages = (int) Math.Ceiling((double) statistics.TotalResults / query.PageSize),
//                    Items = result.ToArray()
//                };
//            }
//        }
//    }
//}
