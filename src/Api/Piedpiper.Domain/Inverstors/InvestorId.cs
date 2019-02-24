using System;
using Piedpiper.Framework;

namespace Piedpiper.Domain.Inverstors
{
    public class InvestorId : Value<InvestorId>
    {
        public static readonly InvestorId Default = new InvestorId(Guid.Empty);

        public readonly Guid Value;

        public InvestorId(Guid value) => Value = value;

        public static InvestorId Parse(Guid investorId)
        {
            if (investorId == Guid.Empty)
                throw new InvalidInvestorId();

            return new InvestorId(investorId);
        }

        public static implicit operator Guid(InvestorId self) => self.Value;
        public static implicit operator InvestorId(Guid value) => Parse(value);
        public static implicit operator string(InvestorId self) => self.Value.ToString();
    }
}
