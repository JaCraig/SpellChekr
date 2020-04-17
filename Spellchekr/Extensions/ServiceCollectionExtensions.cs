using BigBook.Registration;
using Canister.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Spellchekr.Extensions
{
    /// <summary>
    /// Service collection extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the spell checker.
        /// </summary>
        /// <param name="serviceDescriptors">The service descriptors.</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection? AddSpellChecker(this IServiceCollection? serviceDescriptors)
        {
            return serviceDescriptors?.AddSingleton<SpellChecker>();
        }

        /// <summary>
        /// Registers the spell checker.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>The bootstrapper.</returns>
        public static IBootstrapper? RegisterSpellChecker(this IBootstrapper? bootstrapper)
        {
            return bootstrapper?.AddAssembly(typeof(ServiceCollectionExtensions).Assembly).RegisterBigBookOfDataTypes();
        }
    }
}