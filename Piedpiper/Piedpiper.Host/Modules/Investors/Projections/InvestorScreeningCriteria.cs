using System;
using System.Threading.Tasks;
using Piedpiper.Domain.Inverstors;
using Piedpiper.Domain.Screening;
using Piedpiper.Framework;
using Piedpiper.Infrastructure.RavenDB;
using Raven.Client.Documents.Session;

namespace Piedpiper.Host.Modules.Investors.Projections
{
    public class InvestorScreeningCriteriaProjection : Projection
    {
        Func<IAsyncDocumentSession> GetSession { get; }

        public InvestorScreeningCriteriaProjection(Func<IAsyncDocumentSession> getSession) => GetSession = getSession;

        public override async Task Handle(object e)
        {
            switch (e)
            {
                case Events.V1.InvestorRegistered x:
                    await GetSession.ThenSave<InvestorScreeningCriteria>(
                        InvestorScreeningCriteria.Id(x.InvestorId), doc =>
                        {
                            doc.ScreeningCriteria = x.ScreeningCriteria;
                        });
                    break;
                case Events.V1.InvestorScreeningCriteriaChanged x:
                    await GetSession.ThenSave<InvestorScreeningCriteria>(
                        InvestorScreeningCriteria.Id(x.InvestorId), doc =>
                        {
                            doc.ScreeningCriteria = x.ScreeningCriteria;
                        });
                    break;


            }
        }

        public override bool CanHandle(object e)
            => e is Events.V1.InvestorRegistered
            || e is Events.V1.InvestorScreeningCriteriaChanged;

        public class InvestorScreeningCriteria
        {
            public static string Id(Guid id) => $"InvestorScreeningCriteria/{id}";
            public Guid  InvestorId { get; set; }
            public ScreeningCriteria ScreeningCriteria { get; set; }
            
            
        }
    }
}
