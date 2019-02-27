using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Piedpiper.Domain.Companies;
using Piedpiper.Domain.Screening;
using Piedpiper.Domain.Services;
using Piedpiper.Framework;

namespace Piedpiper.Domain.Inverstors
{
    public class Investor : Aggregate
    {
       
        Name Name { get; set; }
        ScreeningCriteria ScreeningCriteria { get; set; }
        List<Company> MonitoredCompanies { get; set; } = new List<Company>();

        protected override void When(object e)
        {
            switch (e)
            {
                case Events.V1.InvestorRegistered x:
                    Id = new InvestorId(x.InvestorId);
                    Name = x.Name;
                    ScreeningCriteria = x.ScreeningCriteria;
                    break;
                case Events.V1.InvestorNameChanged x:
                    Name = x.Name;
                    break;
                case Events.V1.InvestorScreeningCriteriaChanged x:
                    //TODO: Improve to handle single changes
                    ScreeningCriteria = x.ScreeningCriteria;
                    break;
                case Events.V1.CompanyRegistered x:
                    var company = new Company
                    {
                        CompanyId = x.CompanyId,
                        Name = x.CompanyName,
                        ScreeningData = x.ScreeningData
                    };
                    MonitoredCompanies.Add(company);
                    
                    break;
                case Events.V1.CompanyScoreChanged x:
                    var companyUpdated = MonitoredCompanies.FirstOrDefault(c => c.CompanyId == x.CompanyId);
                    companyUpdated.ScreeningScore = new ScreeningScore(
                        x.Score, 
                        x.MustHavesMissing,
                        x.NiceToHavePercentage, 
                        x.SuperNiceToHavePercentage, 
                        x.MissingKpis, 
                        x.NoMetKpis);
                    break;


            }
        }

        public void Register(InvestorId id, string name, Func<DateTimeOffset> getUtcNow)
        {
            if (Version >= 0)
                throw new InvestorAlreadyRegistered();
            if (id.Value == Guid.Empty)
            {
                id = new InvestorId(Guid.NewGuid());
            }

            Apply(new Events.V1.InvestorRegistered()
            {
                InvestorId = id,
                Name = name,
                RegisteredAt = getUtcNow(),
                ScreeningCriteria = ScreeningCriteria.Default()
            });
        }
        public void ChangeName(InvestorId id, string name, Func<DateTimeOffset> getUtcNow)
        {

            Apply(new Events.V1.InvestorNameChanged()
            {
                InvestorId = id,
                Name = name,
                ChangedAt = getUtcNow()
            });

           
        }
        public void ChangeSreeningCriteria(InvestorId id, ScreeningCriteria screeningCriteria, Func<DateTimeOffset> getUtcNow)
        {   
            Apply(new Events.V1.InvestorScreeningCriteriaChanged()
            {
                InvestorId = id,
                ScreeningCriteria = screeningCriteria
            });
            MonitoredCompanies.ForEach(c=> UpdateCompanyScore(c.CompanyId, getUtcNow));
        }
        public void RegisterCompany(InvestorId id, CompanyId companyId, string name, List<ScreeningData> screeningData, Func<DateTimeOffset> getUtcNow)
        {
            Apply(new Events.V1.CompanyRegistered()
            {
                InvestorId = id,
                CompanyId = companyId,
                CompanyName = name,
                ScreeningData = screeningData,
                
            });
            UpdateCompanyScore(companyId, getUtcNow);
        }
        private void UpdateCompanyScore(CompanyId companyId, Func<DateTimeOffset> getUtcNow)
        {
            var scoreCalculator = new ScoreCalculator(ScreeningCriteria);
            var company = MonitoredCompanies.FirstOrDefault(c => c.CompanyId == companyId);
            var score = scoreCalculator.CalculateScore(company.ScreeningData);
            
            Apply(new Events.V1.CompanyScoreChanged
            {
                CompanyId = company.CompanyId,
                InvestorId = Id,
                MatchStatus = (int)score.Match,
                MissingKpis = score.MissingKpis,
                MustHavesMissing = score.MustHavesMissing,
                NiceToHavePercentage = score.NiceToHavePercentage,
                NoMetKpis = score.NoMetKpis,
                SuperNiceToHavePercentage = score.SuperNiceToHavePercentage,
                Score = score.Value
            });
        }

    }
}
