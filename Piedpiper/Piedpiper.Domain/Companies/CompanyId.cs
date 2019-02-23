using System;
using Piedpiper.Domain.Inverstors;
using Piedpiper.Framework;

namespace Piedpiper.Domain.Companies
{
    public class CompanyId : Value<CompanyId>
    {
        public static readonly CompanyId Default = new CompanyId(Guid.Empty);

        public readonly Guid Value;

        public CompanyId(Guid value) => Value = value;

        public static CompanyId Parse(Guid companyId)
        {
            if (companyId == Guid.Empty)
                throw new InvalidInvestorId();

            return new CompanyId(companyId);
        }

        public static implicit operator Guid(CompanyId self) => self.Value;
        public static implicit operator CompanyId(Guid value) => Parse(value);
        public static implicit operator string(CompanyId self) => self.Value.ToString();
    }
}
