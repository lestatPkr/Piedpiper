using System;
using Piedpiper.Framework;

namespace Piedpiper.Domain.Inverstors
{
    public class Name : Value<Name>
    {
        public static readonly Name Default = new Name(String.Empty);

        public readonly string Value;

        internal Name(string value) => Value = value;

        public static Name Parse(string value)
        {
            if (value.Length > 100)
                throw new NameTooLong();

            return new Name(value);
        }

        public static implicit operator string(Name self) => self.Value;
        public static implicit operator Name(string value) => Parse(value);
    }
}
