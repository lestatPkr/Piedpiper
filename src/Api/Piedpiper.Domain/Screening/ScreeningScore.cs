using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Piedpiper.Domain.Screening
{
    public class ScreeningScore
    {
        public double Value { get; set; }
        public bool MustHavesMissing { get; set; }
        public int NiceToHavePercentage { get; set; }
        public int SuperNiceToHavePercentage { get; set; }
        public int MissingKpis { get; set; }
        public int NoMetKpis { get; set; }

        public MatchStatus Match
        {
            get
            {
                if (MustHavesMissing) return MatchStatus.NoMatch;
                if (Value < 0.3) return MatchStatus.Low;
                if (Value < 0.5) return MatchStatus.MediumLow;
                if (Value < 0.6) return MatchStatus.Medium;
                if (Value < 0.8) return MatchStatus.MediumHigh;
                if (Value < 0.95) return MatchStatus.High;
                return MatchStatus.PerfectMatch;
            }
        }

        public ScreeningScore(double value, bool mustHavesMissing, int niceToHavePercentage, int superNiceToHavePercentage, int missingKpis, int noMetKpis)
        {
            Value = value;
            MustHavesMissing = mustHavesMissing;
            NiceToHavePercentage = niceToHavePercentage;
            SuperNiceToHavePercentage = superNiceToHavePercentage;
            MissingKpis = missingKpis;
            NoMetKpis = noMetKpis;
        }
    }
}
