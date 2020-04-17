# SpellChekr
Basic spell checker. The system is based on [Peter Norvig's](http://norvig.com/spell-correct.html) spelling suggestion system.

[![Build status](https://ci.appveyor.com/api/projects/status/ourfxpft7ckqgbho?svg=true)](https://ci.appveyor.com/project/JaCraig/spellchekr)

## Usage
In order to use it you need to declare a dictionary:

    public class ExampleSpellChecker : SpellingDictionaryBase
    {
        public ExampleSpellChecker(ObjectPool<StringBuilder> objectPool)
            : base(Words, objectPool)
        {
        }
        
        public override string Name => "Example";

        private static readonly string[] Words = new string[]
        {
          "Word1",
          "Word2"
          ...
          "Word9999999"
        }
      }
      
And then set it up on the service collection along with the SpellChecker class:

    ServiceCollection.AddSingleton<ExampleSpellChecker>().AddSpellChecker();
    
From there you just use the SpellChecker class:

    string Output=SpellChecker.GetDictionary("Example").Correct(input);
    
The GetDictionary method takes in the name of the dictionary that you wish to use and Correct takes the words that you wish to check.
