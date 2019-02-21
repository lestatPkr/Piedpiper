﻿using Piedpiper.Framework;

namespace Piedpiper.Domain.ClassifiedAds
{
    public class Price : Value<Price>
    {
        public static readonly Price Default = new Price(0);

        public readonly double Value;

        internal Price(double value) => Value = value;

        public static Price Parse(double value)
        {
            if (value < 0)
                throw new PriceNotAllowed();

            return new Price(value);
        }

        public static implicit operator double(Price self) => self.Value;
        public static implicit operator Price(double value) => Parse(value);
    }
}
