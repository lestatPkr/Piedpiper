using System;

namespace Piedpiper.Framework
{
    public abstract class EntityNotFoundException : DomainException
    {
        protected EntityNotFoundException()
            : base("Entity not found.") { }
    }

    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message) { }

        public Guid Id { get; } = Guid.NewGuid();
    }
}
