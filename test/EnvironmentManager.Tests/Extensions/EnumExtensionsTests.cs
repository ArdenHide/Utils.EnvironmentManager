using Xunit;
using FluentAssertions;
using EnvironmentManager.Extensions;

namespace EnvironmentManager.Tests.Extensions;

public class EnumExtensionsTests
{
    internal enum Animal { DOG, CAT, COW }

    internal static IDictionary<Animal, string> Sounds => new Dictionary<Animal, string>
    {
        { Animal.DOG, "woof woof" },
        { Animal.CAT, "meow meow" },
        { Animal.COW, "moo" }
    };

    public class GetEnvironmentValue
    {
        public GetEnvironmentValue()
        {
            Sounds.ToList().ForEach(x => Environment.SetEnvironmentVariable(x.Key.ToString(), x.Value));
        }

        [Theory]
        [InlineData(Animal.DOG)]
        [InlineData(Animal.CAT)]
        [InlineData(Animal.COW)]
        internal void WhenValueExist_WhenRaiseExceptionSetFalse(Animal animal)
        {
            var value = animal.GetEnvironmentValue();

            value.Should().Be(Sounds[animal]);
        }
    }
}