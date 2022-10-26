using Xunit;

namespace MyLibrary.Tests
{
    public class CalculatorTests
    {
        private readonly Calculator _calculator;
        public CalculatorTests()
        {
            _calculator = new Calculator(); 
        }

        [Theory]
        [InlineData(5, 4, 9)]
        [InlineData(2, 8, 10)]
        [InlineData(11, 2, 13)]
        [InlineData(1, 2, 5)]
        public void Add_ShouldWork(double x, double y, double expected)
        {
            double actual = _calculator.Add(x, y);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(5, 4, 1)]
        [InlineData(2, 8, -6)]
        [InlineData(11, 2, 9)]
        [InlineData(1, 2, -1)]
        public void Substract_ShouldWork(double x, double y, double expected)
        {
            double actual = _calculator.Subtract(x, y);

            Assert.Equal(expected, actual);
        }
    }
}