using System;
using System.Collections.Generic;
using System.Text;

namespace Piedpiper.Domain.Inverstors
{
    public class Investor
    {
        Name Name { get; set; }

        private ScreeningCriteria ScreeningCriteria { get; set; }
    }
}
