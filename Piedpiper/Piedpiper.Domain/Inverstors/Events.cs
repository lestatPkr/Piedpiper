using System;

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


                public override string ToString()
                    => $"Investor '{InvestorId}' registered.";
            }

            public class InvestorAdChanged
            {
                public Guid InvestorId { get; set; }
                public string Name { get; set; }
                public DateTimeOffset ChangedAt { get; set; }

                public override string ToString()
                    => $"Investor '{InvestorId}' title changed.";
            }

            
        }
    }
}
