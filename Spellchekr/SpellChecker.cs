using Spellchekr.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Spellchekr
{
    /// <summary>
    /// Spell checker main class
    /// </summary>
    public class SpellChecker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellChecker"/> class.
        /// </summary>
        /// <param name="dictionaries">The dictionaries.</param>
        public SpellChecker(IEnumerable<ISpellingDictionary> dictionaries)
        {
            Dictionaries = dictionaries.ToDictionary(x => x.Name);
        }

        /// <summary>
        /// Gets the dictionaries.
        /// </summary>
        /// <value>The dictionaries.</value>
        private Dictionary<string, ISpellingDictionary> Dictionaries { get; }

        /// <summary>
        /// Gets the dictionary specified.
        /// </summary>
        /// <param name="name">The name of the dictionary.</param>
        /// <returns>The dictionary specified.</returns>
        public ISpellingDictionary GetDictionary(string name)
        {
            return Dictionaries.TryGetValue(name, out var ReturnValue) ? ReturnValue : null;
        }
    }
}