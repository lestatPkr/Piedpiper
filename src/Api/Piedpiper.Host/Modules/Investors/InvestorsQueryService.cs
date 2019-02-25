using System;
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

        public async Task<Contracts.Investors.V1.GetInvestor.Result> GetInvestor(
            Contracts.Investors.V1.GetInvestor query, CancellationToken cancellationToken)
        {
            using (var session = GetSession())
            {
                var result = await session.Query<Projections.InvestorDashboardProjection.InvestorDashboard>()
                    .Where(x=> x.InvestorId == query.InvestorId)
                    .Select(x => new V1.GetInvestor.Result
                    {
                        InvestorId = x.InvestorId,
                        Name = x.Name
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                return result;
            }
        }
    }
}
