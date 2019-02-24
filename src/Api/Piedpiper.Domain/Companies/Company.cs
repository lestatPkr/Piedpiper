using System.Collections.Generic;
using Piedpiper.Domain.Screening;

namespace Piedpiper.Domain.Companies
{
    public class Company
    {
        public CompanyId CompanyId { get;  set; }
        public Name Name { get;  set; }
        public ScreeningScore ScreeningScore { get; set; }
        public List<ScreeningData> ScreeningData { get; set; }

        


    }
}
