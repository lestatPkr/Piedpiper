using System.Collections.Generic;
using Piedpiper.Domain.Inverstors;
using Piedpiper.Framework;

namespace Piedpiper.Domain.Screening
{
    public class ScreeningCriteria : Value<ScreeningCriteria>
    {
        public List<KPI> MustHave { get; set; } 
        public List<KPI> SuperNiceToHave { get; set; }
        public List<KPI> NiceToHave { get; set; }
        public double MustHaveWeigth { get; set; } = 0.55;
        public double NiceToHaveWeigth { get; set; } = 0.3;
        public double SuperNiceToHaveWeigth { get; set; } = 0.15;

        public static ScreeningCriteria Default()
        {
            var screeningCriteria = new ScreeningCriteria {MustHave = new List<KPI>()};
            screeningCriteria.MustHave.AddRange(new List<KPI>
            {
                KPI.CEOFullTime,
                KPI.CTOFullTime,
                KPI.RoundBetween1MAnd10M
            });
            screeningCriteria.NiceToHave = new List<KPI>();
            screeningCriteria.NiceToHave.AddRange(new List<KPI>
            {
                KPI.SourceOfLeadsIsPersonalContact,
                KPI.CEORepeatsEntrepeneur,
                KPI.MoreThan50KMonthlyRecurrentRevenue
            });
            screeningCriteria.SuperNiceToHave = new List<KPI>();
            screeningCriteria.SuperNiceToHave.AddRange(new List<KPI>
            {
                KPI.CEOWorkedAtHighGrowthStartup,
                KPI.CTOPlus2YearsLeadingTechTeams,
                KPI.PreviousInvestorsFromTopFunds
            });
            
            return screeningCriteria;
        }

    }
}
