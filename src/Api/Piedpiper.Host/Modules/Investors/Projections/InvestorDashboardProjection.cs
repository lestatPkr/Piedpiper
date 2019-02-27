using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Piedpiper.Domain.Inverstors;
using Piedpiper.Domain.Screening;
using Piedpiper.Framework;
using Piedpiper.Infrastructure.RavenDB;
using Raven.Client.Documents.Linq.Indexing;
using Raven.Client.Documents.Session;

namespace Piedpiper.Host.Modules.Investors.Projections
{
    public class InvestorDashboardProjection : Projection
    {
        Func<IAsyncDocumentSession> GetSession { get; }

        public InvestorDashboardProjection(Func<IAsyncDocumentSession> getSession) => GetSession = getSession;

        public override async Task Handle(object e)
        {
            switch (e)
            {
                case Events.V1.InvestorRegistered x:
                    
                    await GetSession.ThenSave<InvestorDashboard>(
                        InvestorDashboard.Id(x.InvestorId), doc =>
                        {
                            doc.Name = x.Name;
                            doc.InvestorId = x.InvestorId;
                            doc.ScreeningCriteria = new ScreeningCriteria
                            {
                                MustHave = x.ScreeningCriteria.MustHave,
                                NiceToHave = x.ScreeningCriteria.NiceToHave,
                                SuperNiceToHave = x.ScreeningCriteria.SuperNiceToHave
                            };
                        });
                    break;
                case Events.V1.InvestorNameChanged x:
                    using (var session = GetSession())
                    {
                        var investor = await session.LoadAsync<InvestorDashboard>(InvestorDashboard.Id(x.InvestorId));
                        investor.Name = x.Name;
                        await session.SaveChangesAsync();
                    }
                    break;
                case Events.V1.CompanyRegistered x:
                    using (var session = GetSession())
                    {
                        var investor = await session.LoadAsync<InvestorDashboard>(InvestorDashboard.Id(x.InvestorId));
                        investor.Companies.Add(new InvestorCompanyProjection
                        {
                            CompanyId = x.CompanyId,
                            Name = x.CompanyName,
                            
                            
                        });
                        await session.SaveChangesAsync();

                    }
                    break;
                case Events.V1.CompanyScoreChanged x:
                    using (var session = GetSession())
                    {
                        var company = (await session.LoadAsync<InvestorDashboard>(InvestorDashboard.Id(x.InvestorId)))
                            .Companies.FirstOrDefault(c=> c.CompanyId == x.CompanyId);
                        company.MatchStatus = x.MatchStatus;
                        company.Score = x.Score;
                        company.MissingKpis = x.MissingKpis;
                        company.MustHavesMissing = x.MustHavesMissing;
                        company.NoMetKpis = x.NoMetKpis;
                        company.NiceToHavePercentage = x.NiceToHavePercentage;
                        company.SuperNiceToHavePercentage = x.SuperNiceToHavePercentage;
                        await session.SaveChangesAsync();

                    }
                    break;
                case Events.V1.InvestorScreeningCriteriaChanged x:
                    using (var session = GetSession())
                    {
                        var investor = await session.LoadAsync<InvestorDashboard>(InvestorDashboard.Id(x.InvestorId));
                        investor.ScreeningCriteria = new ScreeningCriteria
                        {
                            MustHave = x.ScreeningCriteria.MustHave,
                            NiceToHave = x.ScreeningCriteria.NiceToHave,
                            SuperNiceToHave = x.ScreeningCriteria.SuperNiceToHave
                        };
                        await session.SaveChangesAsync();

                    }
                    break;
                default:
                        break;

            }
        }

        public override bool CanHandle(object e)
            => e is Events.V1.InvestorRegistered
               || e is Events.V1.InvestorNameChanged
               || e is Events.V1.CompanyRegistered
               || e is Events.V1.InvestorScreeningCriteriaChanged
               || e is Events.V1.CompanyScoreChanged;

      
        public class InvestorCompanyProjection
        {
            public Guid CompanyId { get; set; }
            public string Name { get; set; }
            public double Score { get; set; }
            public bool MustHavesMissing { get; set; }
            public int NiceToHavePercentage { get; set; }
            public int SuperNiceToHavePercentage { get; set; }
            public int MissingKpis { get; set; }
            public int NoMetKpis { get; set; }
            public int MatchStatus { get; set; }
        }
        public class ScreeningCriteriaProjection
        {
            public List<int> MustHave { get; set; } = new List<int>();
            public List<int> SuperNiceToHave { get; set; } = new List<int>();
            public List<int> NiceToHave { get; set; } = new List<int>();
            
        }
        public class InvestorDashboard
        {
            public static string Id(Guid id) => $"Investor/{id}";
            public Guid InvestorId { get; set; }
            public string Name { get; set; }
            public List<InvestorCompanyProjection> Companies { get; set; } = new List<InvestorCompanyProjection>();
            public ScreeningCriteria ScreeningCriteria { get; set; }

        }
    }

   
}
