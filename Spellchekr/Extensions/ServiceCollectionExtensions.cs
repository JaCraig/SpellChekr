using BigBook.Registration;
using Canister.Interfaces;
using Spellchekr;
using Spellchekr.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
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
            if (serviceDescriptors.Exists<SpellChecker>())
                return serviceDescriptors;
            return serviceDescriptors?.AddAllSingleton<ISpellingDictionary>()
                ?.AddSingleton<SpellChecker>()
                ?.RegisterBigBookOfDataTypes();
        }

        /// <summary>
        /// Registers the spell checker.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        /// <returns>The bootstrapper.</returns>
        public static ICanisterConfiguration? RegisterSpellChecker(this ICanisterConfiguration? bootstrapper) => bootstrapper?.AddAssembly(typeof(ServiceCollectionExtensions).Assembly).RegisterBigBookOfDataTypes();
    }
}