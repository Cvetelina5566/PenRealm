using NuGet.ContentModel;
using PenRealm.Models;
using Xunit;

namespace PenRealm.Tests
{
    public class PoemTests
    {
        [Fact]
        public void Poem_Without_Title_Should_Be_Invalid()
        {
            var poem = new Poem
            {
                Title = null,
                Content = "Some text"
            };

            Assert.Null(poem.Title);
        }

        [Fact]
        public void Poem_With_Title_Should_Be_Valid()
        {
            var poem = new Poem
            {
                Title = "My Title",
                Content = "Some text"
            };

            Assert.NotNull(poem.Title);
        }
    }
}

