using System;
using System.Collections.Generic;
using Piedpiper.Domain.Companies;
using Piedpiper.Domain.Screening;

namespace Piedpiper.Domain.Inverstors
{
    public static class Events
    {
        public static class V1
        {
            public class InvestorRegistered
            {
                public Guid InvestorId { get; set; }
                public DateTimeOffset RegisteredAt { get; set; }
                public string Name { get; set; }
                public ScreeningCriteria    ScreeningCriteria { get; set; }

                public override string ToString()
                    => $"Investor '{InvestorId}' registered.";
            }

            public class InvestorNameChanged
            {
                public Guid InvestorId { get; set; }
                public string Name { get; set; }
                public DateTimeOffset ChangedAt { get; set; }

                public override string ToString()
                    => $"Investor '{InvestorId}' name changed.";
            }

            public class InvestorScreeningCriteriaChanged
            {
                public Guid InvestorId { get; set; }
                public ScreeningCriteria ScreeningCriteria { get; set; }
                public DateTimeOffset ChangedAt { get; set; }

                public override string ToString()
                    => $"Investor '{InvestorId}' screening criteria changed.";
            }
            
            public class CompanyRegistered
            {
                public InvestorId InvestorId { get; set; }
                public CompanyId CompanyId { get; set; }
                public string CompanyName { get; set; }
                public List<ScreeningData> ScreeningData { get; set; }
               
                public override string ToString()
                    => $"Company '{CompanyId}' registered in investor {InvestorId}";

            }
            public class CompanyScoreChanged
            {
                public InvestorId InvestorId { get; set; }
                public CompanyId CompanyId { get; set; }
                
                public bool MustHavesMissing { get; set; }
                public int NiceToHavePercentage { get; set; }
                public int SuperNiceToHavePercentage { get; set; }
                public int MissingKpis { get; set; }
                public int NoMetKpis { get; set; }
                public int MatchStatus { get; set; }
                public double Score { get; set; }
                public override string ToString()
                    => $"Company '{CompanyId}' score changed";

            }
        }
    }
}
