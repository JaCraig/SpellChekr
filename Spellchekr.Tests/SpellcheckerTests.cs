using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Spellchekr.Interfaces;
using Spellchekr.Tests.BaseClasses;
using Xunit;

namespace Spellchekr.Tests
{
    public class SpellcheckerTests : TestBaseClass<SpellChecker>
    {
        [Theory]
        [InlineData(new object[] { "Merica", "america" })]
        [InlineData(new object[] { "Cananda", "canada" })]
        [InlineData(new object[] { "Amabia", "arabia" })]
        [InlineData(new object[] { "BoopBoop", "boopboop" })]
        public void CheckSpelling(string input, string expected)
        {
            ObjectPool<System.Text.StringBuilder> ObjectPool = new DefaultObjectPoolProvider().CreateStringBuilderPool();
            var TestObject = new SpellChecker(new ISpellingDictionary[] { new CountrySpellChecker(ObjectPool) });
            Assert.Equal(expected, TestObject.GetDictionary("Country")?.Correct(input));
        }

        [Fact]
        public void FromServices()
        {
            _ = new ServiceCollection().AddCanisterModules(x => x.RegisterSpellChecker()?.AddAssembly(typeof(SpellcheckerTests).Assembly));
            ISpellingDictionary? TestObject = new ServiceCollection().AddCanisterModules()?.BuildServiceProvider()?.GetService<SpellChecker>()?.GetDictionary("Country");
            Assert.NotNull(TestObject);
            Assert.Equal("Country", TestObject.Name);
            Assert.Equal("arabia", TestObject.Correct("Amabia"));
        }

        [Fact]
        public void GetDictionary()
        {
            ObjectPool<System.Text.StringBuilder> ObjectPool = new DefaultObjectPoolProvider().CreateStringBuilderPool();
            var TestObject = new SpellChecker(new ISpellingDictionary[] { new CountrySpellChecker(ObjectPool) });
            Assert.Equal("Country", TestObject.GetDictionary("Country")?.Name);
        }
    }
}