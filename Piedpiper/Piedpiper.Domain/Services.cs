using System.Threading.Tasks;

// ReSharper disable CheckNamespace

namespace Piedpiper.Domain.Shared.Services
{
    public delegate Task<bool> CheckTextForProfanity(string text);   
}
