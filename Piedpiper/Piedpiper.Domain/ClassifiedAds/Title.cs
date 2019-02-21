﻿using System;
using Piedpiper.Framework;

namespace Piedpiper.Domain.ClassifiedAds
{
    public class Title : Value<Title>
    {
        public static readonly Title Default = new Title(String.Empty);

        public readonly string Value;

        internal Title(string value) => Value = value;

        public static Title Parse(string value)
        {
            if (value.Length > 100)
                throw new TitleTooLong();

            return new Title(value);
        }

        public static implicit operator string(Title self) => self.Value;
        public static implicit operator Title(string value) => Parse(value);
    }
}
