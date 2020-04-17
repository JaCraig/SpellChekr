using Canister.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Spellchekr.Interfaces;

namespace Spellchekr.Modules
{
    /// <summary>
    /// Canister module
    /// </summary>
    /// <seealso cref="IModule"/>
    public class CanisterModule : IModule
    {
        /// <summary>
        /// Order to run this in
        /// </summary>
        public int Order => 1;

        /// <summary>
        /// Loads the module using the bootstrapper
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        public void Load(IBootstrapper bootstrapper)
        {
            bootstrapper?.RegisterAll<ISpellingDictionary>(ServiceLifetime.Singleton)
                .Register<SpellChecker>(ServiceLifetime.Singleton);
        }
    }
}