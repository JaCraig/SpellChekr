using BigBook.Registration;
using Canister.Interfaces;

/* Unmerged change from project 'Spellchekr (net9.0)'
Before:
using Spellchekr;
After:
using Microsoft.Extensions.DependencyInjection;
using Spellchekr;
*/

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
        public static IServiceCollection? AddSpellChecker(this IServiceCollection? serviceDescriptors) => serviceDescriptors?.AddSingleton<SpellChecker>();

        /// <summary>
        /// Registers the spell checker.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>The bootstrapper.</returns>
        public static ICanisterConfiguration? RegisterSpellChecker(this ICanisterConfiguration? bootstrapper) => bootstrapper?.AddAssembly(typeof(ServiceCollectionExtensions).Assembly).RegisterBigBookOfDataTypes();
    }
}