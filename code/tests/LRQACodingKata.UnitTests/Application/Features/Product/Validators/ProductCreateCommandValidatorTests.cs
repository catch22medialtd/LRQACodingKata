using FluentValidation.TestHelper;
using LRQACodingKata.Application.Features.Product.Commands;
using LRQACodingKata.Application.Features.Product.Validators;
using LRQACodingKata.Core.Constants;

namespace LRQACodingKata.UnitTests.Application.Features.Product.Validators
{
    public class ProductCreateCommandValidatorTests
    {
        private readonly ProductCreateCommandValidator _validator;

        public ProductCreateCommandValidatorTests()
        {
            _validator = new ProductCreateCommandValidator();
        }

        [Fact]
        public void Validator_ShouldBeValid_WhenAllPropertiesAreValid()
        {
            // Arrange
            var command = new ProductCreateCommand("Valid Product", 10.99m, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Name_ShouldHaveValidationError_WhenEmpty(string? name)
        {
            // Arrange
            var command = new ProductCreateCommand(name!, 10.99m, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("'Name' must not be empty.");
        }

        [Fact]
        public void Name_ShouldHaveValidationError_WhenExceedsMaximumLength()
        {
            // Arrange
            var longName = new string('A', EntityPropertyLengths.Product_Name + 1);
            var command = new ProductCreateCommand(longName, 10.99m, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage($"The length of 'Name' must be {EntityPropertyLengths.Product_Name} characters or fewer. You entered {longName.Length} characters.");
        }

        [Fact]
        public void Name_ShouldBeValid_WhenAtMaximumLength()
        {
            // Arrange
            var maxLengthName = new string('A', EntityPropertyLengths.Product_Name);
            var command = new ProductCreateCommand(maxLengthName, 10.99m, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData(0.00)]
        [InlineData(-1.00)]
        [InlineData(-0.01)]
        public void Price_ShouldHaveValidationError_WhenBelowMinimum(decimal price)
        {
            // Arrange
            var command = new ProductCreateCommand("Valid Product", price, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price)
                .WithErrorMessage($"'Price' must be between {EntityPropertyRanges.Product_Price_Min} and {EntityPropertyRanges.Product_Price_Max}. You entered {price}.");
        }

        [Theory]
        [InlineData(99_000.01)]
        [InlineData(100_000.00)]
        [InlineData(999_999.99)]
        public void Price_ShouldHaveValidationError_WhenAboveMaximum(decimal price)
        {
            // Arrange
            var command = new ProductCreateCommand("Valid Product", price, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Price)
                .WithErrorMessage($"'Price' must be between {EntityPropertyRanges.Product_Price_Min} and {EntityPropertyRanges.Product_Price_Max}. You entered {price}.");
        }

        [Theory]
        [InlineData(0.01)]
        [InlineData(1.00)]
        [InlineData(999.99)]
        [InlineData(99_000.00)]
        public void Price_ShouldBeValid_WhenWithinValidRange(decimal price)
        {
            // Arrange
            var command = new ProductCreateCommand("Valid Product", price, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Price);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-10)]
        [InlineData(-999)]
        public void Stock_ShouldHaveValidationError_WhenBelowMinimum(int stock)
        {
            // Arrange
            var command = new ProductCreateCommand("Valid Product", 10.99m, stock);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Stock)
                .WithErrorMessage($"'Stock' must be between {EntityPropertyRanges.Product_Stock_Min} and {EntityPropertyRanges.Product_Stock_Max}. You entered {stock}.");
        }

        [Theory]
        [InlineData(10_000)]
        [InlineData(15_000)]
        [InlineData(99_999)]
        public void Stock_ShouldHaveValidationError_WhenAboveMaximum(int stock)
        {
            // Arrange
            var command = new ProductCreateCommand("Valid Product", 10.99m, stock);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Stock)
                .WithErrorMessage($"'Stock' must be between {EntityPropertyRanges.Product_Stock_Min} and {EntityPropertyRanges.Product_Stock_Max}. You entered {stock}.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(9_999)]
        public void Stock_ShouldBeValid_WhenWithinValidRange(int stock)
        {
            // Arrange
            var command = new ProductCreateCommand("Valid Product", 10.99m, stock);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Stock);
        }

        [Fact]
        public void Validator_ShouldHaveMultipleValidationErrors_WhenAllPropertiesAreInvalid()
        {
            // Arrange
            var command = new ProductCreateCommand("", -1.00m, -1);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.Price);
            result.ShouldHaveValidationErrorFor(x => x.Stock);
        }

        [Fact]
        public void Validator_ShouldHaveValidationError_WhenNameExceedsLengthAndPriceIsInvalid()
        {
            // Arrange
            var longName = new string('A', EntityPropertyLengths.Product_Name + 1);
            var command = new ProductCreateCommand(longName, 0.00m, 5);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.Price);
            result.ShouldNotHaveValidationErrorFor(x => x.Stock);
        }

        [Theory]
        [InlineData("Single Character Product", 0.01, 0)]
        [InlineData("Z", 99_000.00, 9_999)]
        public void Validator_ShouldBeValid_WhenAllPropertiesAreAtBoundaryValues(string name, decimal price, int stock)
        {
            // Arrange
            var command = new ProductCreateCommand(name, price, stock);

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
