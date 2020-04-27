namespace Spellchekr.Interfaces
{
    /// <summary>
    /// Spelling dictionary interface
    /// </summary>
    public interface ISpellingDictionary
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// Corrects the specified words.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns>The corrected words.</returns>
        string? Correct(string? words);
    }
}