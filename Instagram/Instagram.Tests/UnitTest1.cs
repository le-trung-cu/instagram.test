using Xunit;

namespace Instagram.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(0, "_")]
        [InlineData(1, "a")]
        [InlineData(2, "b")]
        [InlineData(25, "y")]
        [InlineData(26, "z")]
        [InlineData(27, "A")]
        [InlineData(28, "B")]
        [InlineData(51, "Y")]
        [InlineData(52, "Z")]
        [InlineData(53, "a_")]
        [InlineData(54, "aa")]
        [InlineData(55, "ab")]
        [InlineData(104, "aY")]
        [InlineData(105, "aZ")]
        public void Correct_Convert_Int_To_Slug(int num, string slug)
        {
            Assert.Equal(slug, new Shared.Base53(num).Slug);
        }

        [Theory]
        [InlineData(0, "_")]
        [InlineData(1, "a")]
        [InlineData(2, "b")]
        [InlineData(25, "y")]
        [InlineData(26, "z")]
        [InlineData(27, "A")]
        [InlineData(28, "B")]
        [InlineData(51, "Y")]
        [InlineData(52, "Z")]
        [InlineData(53, "a_")]
        [InlineData(54, "aa")]
        [InlineData(55, "ab")]
        [InlineData(104, "aY")]
        [InlineData(105, "aZ")]
        public void Correct_Conver_Slug_To_Int(int num, string slug)
        {
            Assert.Equal(num, new Shared.Base53(slug).Value);
        }
    }
}