using System;
using System.Threading.Tasks;
using Piedpiper.Domain.Shared.Services;
using Piedpiper.Framework;

namespace Piedpiper.Domain.ClassifiedAds
{
    public class AdText : Value<AdText>
    {
        public static readonly AdText Default = new AdText(String.Empty);
        
        internal AdText(string value) => Value = value;
        
        public readonly string Value;
        
        public static async Task<AdText> Parse(string text, CheckTextForProfanity checkTextForProfanity)
        {
            var containsProfanity = await checkTextForProfanity(text);
            if (containsProfanity)
                throw new ProfanityFound();   
            
            return new AdText(text);
        }
        
        public static implicit operator string(AdText self) => self.Value;
    }
}
