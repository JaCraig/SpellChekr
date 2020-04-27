using BigBook;
using Microsoft.Extensions.ObjectPool;
using Spellchekr.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Spellchekr.BaseClasses
{
    /// <summary>
    /// Spelling dictionary base
    /// </summary>
    /// <seealso cref="ISpellingDictionary"/>
    public abstract class SpellingDictionaryBase : ISpellingDictionary
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpellingDictionaryBase"/> class.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <param name="objectPool">The object pool.</param>
        protected SpellingDictionaryBase(string words, ObjectPool<StringBuilder> objectPool)
            : this(words.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries), objectPool)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpellingDictionaryBase"/> class.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <param name="objectPool">The object pool.</param>
        protected SpellingDictionaryBase(string[] words, ObjectPool<StringBuilder> objectPool)
        {
            for (int i = 0; i < words.Length; i++)
            {
                var TempWords = words[i].Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < TempWords.Length; ++x)
                {
                    string trimmedWord = TempWords[x].Trim().ToLower();
                    if (WordRegex.IsMatch(trimmedWord))
                    {
                        Dictionary.Add(trimmedWord);
                    }
                }
            }
            ObjectPool = objectPool;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the word regex.
        /// </summary>
        /// <value>The word regex.</value>
        protected static Regex WordRegex { get; } = new Regex("[a-z]+", RegexOptions.Compiled);

        /// <summary>
        /// The dictionary
        /// </summary>
        /// <value>The dictionary.</value>
        protected Bag<string> Dictionary { get; } = new Bag<string>();

        /// <summary>
        /// Gets the object pool.
        /// </summary>
        /// <value>The object pool.</value>
        protected ObjectPool<StringBuilder> ObjectPool { get; }

        /// <summary>
        /// Corrects the specified word.
        /// </summary>
        /// <param name="words">The word.</param>
        /// <returns>The corrected word.</returns>
        public string? Correct(string? words)
        {
            if (string.IsNullOrEmpty(words))
                return words;

            words = words.ToLower();

            if (Dictionary.Contains(words))
                return words;

            StringBuilder Result = ObjectPool?.Get() ?? new StringBuilder();
            var WordList = words.Split(new char[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            for (int y = 0; y < WordList.Length; ++y)
            {
                var SplitWord = WordList[y];
                if (Dictionary.Contains(SplitWord))
                {
                    Result.Append(SplitWord).Append(" ");
                    continue;
                }
                var List = Edits(SplitWord);
                var Candidates = new Dictionary<string, int>();

                for (var i = 0; i < List.Count; i++)
                {
                    var Item = List[i];
                    if (Dictionary.Contains(Item) && !Candidates.ContainsKey(Item))
                        Candidates.Add(Item, Dictionary[Item]);
                }

                if (Candidates.Count > 0)
                    return Candidates.OrderByDescending(x => x.Value).First().Key;

                for (int i = 0; i < List.Count; ++i)
                {
                    var Item = List[i];
                    var EditList = Edits(Item);
                    for (int x = 0; x < EditList.Count; ++x)
                    {
                        var wordVariation = EditList[x];
                        if (Dictionary.Contains(wordVariation) && !Candidates.ContainsKey(wordVariation))
                            Candidates.Add(wordVariation, Dictionary[wordVariation]);
                    }
                }

                Result.Append((Candidates.Count > 0) ? Candidates.OrderByDescending(x => x.Value).First().Key : SplitWord).Append(" ");
            }
            var ReturnValue = Result.ToString().Trim();
            ObjectPool?.Return(Result);
            return ReturnValue;
        }

        /// <summary>
        /// Gets the possible edits for the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>The list of possible words.</returns>
        private static List<string> Edits(string word)
        {
            var splits = new Tuple<string, string>[word.Length];
            var transposes = new List<string>();
            var deletes = new List<string>();
            var replaces = new List<string>();
            var inserts = new List<string>();

            // Splits
            for (int i = 0; i < word.Length; i++)
            {
                splits[i] = new Tuple<string, string>(word.Substring(0, i), word.Substring(i));
            }

            // Deletes
            for (int i = 0; i < splits.Length; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (!string.IsNullOrEmpty(b))
                {
                    deletes.Add(a + b.Substring(1));
                }
            }

            // Transposes
            for (int i = 0; i < splits.Length; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (b.Length > 1)
                {
                    transposes.Add(a + b[1] + b[0] + b.Substring(2));
                }
            }

            // Replaces
            for (int i = 0; i < splits.Length; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                if (!string.IsNullOrEmpty(b))
                {
                    for (char c = 'a'; c <= 'z'; c++)
                    {
                        replaces.Add(a + c + b.Substring(1));
                    }
                }
            }

            // Inserts
            for (int i = 0; i < splits.Length; i++)
            {
                string a = splits[i].Item1;
                string b = splits[i].Item2;
                for (char c = 'a'; c <= 'z'; c++)
                {
                    inserts.Add(a + c + b);
                }
            }

            return deletes.Union(transposes).Union(replaces).Union(inserts).ToList();
        }
    }
}