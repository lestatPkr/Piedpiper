using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using static Piedpiper.Contracts.Investors;

namespace Piedpiper.Host.Modules.Investors
{
    public class InvestorsQueryService
    {
        Func<IAsyncDocumentSession> GetSession { get; }

        public InvestorsQueryService(Func<IAsyncDocumentSession> getSession)
            => GetSession = getSession;

        public async Task<Contracts.Investors.V1.GetDashboard.Result> GetInvestor(
            Contracts.Investors.V1.GetDashboard query, CancellationToken cancellationToken)
        {
            using (var session = GetSession())
            {
                var result = await RavenQueryableExtensions.Select(session.Query<Projections.InvestorDashboardProjection.InvestorDashboard>()
                        .Where(x=> x.InvestorId == query.InvestorId), x => new V1.GetDashboard.Result
                    {
                        InvestorId = x.InvestorId,
                        Name = x.Name,
                        Companies = Enumerable.ToList(x.Companies).Select(c=> new V1.GetDashboard.Company
                        {
                            CompanyId = c.CompanyId,
                            Score = c.Score,
                            NiceToHavePercentage = c.NiceToHavePercentage,
                            Name = c.Name,
                            NoMetKpis = c.NoMetKpis,
                            SuperNiceToHavePercentage = c.SuperNiceToHavePercentage,
                            MustHavesMissing = c.MustHavesMissing,
                            MissingKpis = c.MissingKpis,
                            MatchStatus = c.MatchStatus
                        }).ToList()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return result;
            }
        }
    }
}
