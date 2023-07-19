using AwesomeGIC.Application.Rules;
using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Infrastructure.InterestRule.Queries;
using AwesomeGIC.Infrastructure.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUnitTests.Rules
{
    public class RuleHandlerTests
    {
        [Fact]
        public void IsValidRuleInput_ValidRule_ReturnsTrue()
        {
            // Arrange
            var mockIDateHandler = new Mock<IDateHandler>();
            var mockIReadInterestRule = new Mock<IReadInterestRule>();

            var rule = "20230612|10|091";
            var ruleHandler = new RuleHandler(mockIDateHandler.Object, mockIReadInterestRule.Object);
            var date = DateTime.Parse("2023-10-23");
            mockIDateHandler.Setup(p => p.isDateFormatValid(It.IsAny<string>())).Returns(true);

            // Act
            var result = ruleHandler.IsValidRuleInput(rule.Split("|"));

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsValidRuleInput_InValidRule_ReturnsFalse()
        {
            // Arrange
            var mockIDateHandler = new Mock<IDateHandler>();
            var mockIReadInterestRule = new Mock<IReadInterestRule>();

            var rule = "20230612|10";
            var ruleHandler = new RuleHandler(mockIDateHandler.Object, mockIReadInterestRule.Object);

            // Act
            var result = ruleHandler.IsValidRuleInput(rule.Split("|"));

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsValidRuleInput_InValidDateFormat_ReturnsFalse()
        {
            // Arrange
            var mockIDateHandler = new Mock<IDateHandler>();
            var mockIReadInterestRule = new Mock<IReadInterestRule>();

            var rule = "20230612|10|901";
            var ruleHandler = new RuleHandler(mockIDateHandler.Object, mockIReadInterestRule.Object);
            var date = DateTime.Parse("2023-10-23");
            mockIDateHandler.Setup(p => p.isDateFormatValid(It.IsAny<string>())).Returns(false);

            // Act
            var result = ruleHandler.IsValidRuleInput(rule.Split("|"));

            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public void IsValidRuleInput_ValidDateFormat_ReturnsTrue()
        {
            // Arrange
            var mockIDateHandler = new Mock<IDateHandler>();
            var mockIReadInterestRule = new Mock<IReadInterestRule>();

            var rule = "20230612|10|90";
            var ruleHandler = new RuleHandler(mockIDateHandler.Object, mockIReadInterestRule.Object);
            var date = DateTime.Parse("2023-10-23");
            mockIDateHandler.Setup(p => p.isDateFormatValid(It.IsAny<string>())).Returns(true);

            // Act
            var result = ruleHandler.IsValidRuleInput(rule.Split("|"));

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
