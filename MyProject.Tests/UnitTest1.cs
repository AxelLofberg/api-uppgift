using Xunit;


namespace api_uppgift
{
    public class ProgramTests
    {
        [Fact]
        public void ToRovarsprak_TranslatesTextToRovarsprak()
        {
            // Arrange
            string inputText = "Hello";

            // Act
            string rovarsprakText = Program.ToRovarsprak(inputText);

            // Assert
            Assert.Equal("Hohelollolo", rovarsprakText);
        }

        [Fact]
        public void FromRovarsprak_TranslatesRovarsprakToText()
        {
            // Arrange
            string rovarsprakText = "HoHelollolo";

            // Act
            string originalText = Program.FromRovarsprak(rovarsprakText);

            // Assert
            Assert.Equal("Hello", originalText);
        }
    }
}
