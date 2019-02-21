using System;
using System.Collections.Generic;
using System.Text;
using Piedpiper.Framework;

namespace Piedpiper.Domain.Inverstors
{
    public class ScreeningCriteria : Value<ScreeningCriteria>
    {
        protected List<Guid> MustHave { get; set; } = new List<Guid>();
        protected List<Guid> SuperNiceToHave { get; set; } = new List<Guid>();
        protected List<Guid> NiceToHave { get; set; } = new List<Guid>();



    }
}
