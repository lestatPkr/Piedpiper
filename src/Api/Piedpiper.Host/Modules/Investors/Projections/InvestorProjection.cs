using System;
using System.Threading.Tasks;
using Piedpiper.Domain.Inverstors;
using Piedpiper.Framework;
using Piedpiper.Infrastructure.RavenDB;
using Raven.Client.Documents.Session;

namespace Piedpiper.Host.Modules.Investors.Projections
{
    public class InvestorsProjection : Projection
    {
        Func<IAsyncDocumentSession> GetSession { get; }

        public InvestorsProjection(Func<IAsyncDocumentSession> getSession) => GetSession = getSession;

        public override async Task Handle(object e)
        {
            switch (e)
            {
                case Events.V1.InvestorRegistered x:
                    await GetSession.ThenSave<InvestorProjection>(
                        InvestorProjection.Id(x.InvestorId), doc =>
                        {
                            doc.Name = x.Name;
                            doc.InvestorId = x.InvestorId;
                        });
                    break;
                case Events.V1.InvestorNameChanged x:
                    var investor = await GetSession.Get<InvestorProjection>(x.InvestorId.ToString());
                    investor.Name = x.Name;
                    await GetSession().SaveChangesAsync();
                    break;


            }
        }

        public override bool CanHandle(object e)
            => e is Events.V1.InvestorRegistered
            || e is Events.V1.InvestorNameChanged;

        public class InvestorProjection
        {
            public static string Id(Guid id) => $"Investor/{id}";
            public Guid InvestorId { get; set; }
            public string Name { get; set; }
            
        }
    }
}
