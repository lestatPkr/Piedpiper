using System;
using System.Collections.Generic;
using System.Text;

namespace Piedpiper.Domain.Screening
{
    public enum MatchStatus
    {
        NoMatch = 0,
        Low = 1,
        MediumLow = 2,
        Medium = 3,
        MediumHigh = 4,
        High = 5,
        PerfectMatch = 6
    }
}
