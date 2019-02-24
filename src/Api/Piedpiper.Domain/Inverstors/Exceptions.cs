using Piedpiper.Framework;

namespace Piedpiper.Domain.Inverstors
{
    public class InvestorNotFound : EntityNotFoundException { }

    public class InvestorAlreadyRegistered : DomainException
    {
        public InvestorAlreadyRegistered()
            : base("Investor already registered.") { }
    }

    public class NameTooLong : DomainException
    {
        public NameTooLong()
            : base("Name too long.") { }
    }

    public class NameRequired : DomainException
    {
        public NameRequired()
            : base("Name required.") { }
    }

    public class InvalidInvestorId : DomainException
    {
        public InvalidInvestorId()
            : base("Id cannot be default.") { }
    }

  
    
}
