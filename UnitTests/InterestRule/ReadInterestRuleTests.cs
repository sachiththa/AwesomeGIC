using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Infrastructure.DBContext;
using AwesomeGIC.Infrastructure.InterestRule.Queries;
using AwesomeGIC.Infrastructure.Utils;
using Moq;

namespace InfrastructureUnitTests.InterestRule
{
    public class ReadInterestRuleTests
    {

        //[Fact]
        //public void ReplaceInterestRule_GetInterestRuleForMonth_ReturnsReplaceInterestRuleInfo()
        //{
        //    // Arrange
        //    var mockIDateHandler = new Mock<IDateHandler>();
        //    var date = DateTime.Parse("2023-03-23");
        //    var rules = new List<Rule>() { new Rule() { Id = "RULE999", Date = "20230312" } };
        //    Database.Rules.AddRange(rules);
        //    mockIDateHandler.Setup(p => p.ConvertDateTime(It.IsAny<string>(), It.IsAny<string>())).Returns(date);
        //    ReadInterestRule readInterestRule = new ReadInterestRule(mockIDateHandler.Object);

        //    // Act
        //    var result = readInterestRule.GetInterestRuleForMonth(3);

        //    // Assert
        //    Assert.Equal("RULE999", result?.Id);
        //}

        [Fact]
        public void ReplaceInterestRule_GetInterestRuleForMonth_NotExistsReturnsNull()
        {
            // Arrange
            var mockIDateHandler = new Mock<IDateHandler>();
            var date = DateTime.Parse("2023-10-23");
            mockIDateHandler.Setup(p => p.ConvertDateTime(It.IsAny<string>(), It.IsAny<string>())).Returns(date);
            ReadInterestRule readInterestRule = new ReadInterestRule(mockIDateHandler.Object);

            // Act
            var result = readInterestRule.GetInterestRuleForMonth(2);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ReplaceInterestRule_GetSortedInterestRulesByDate_ReturnsSortedRules()
        {
            // Arrange
            var mockIDateHandler = new Mock<IDateHandler>();
            // var date = DateTime.Parse("2023-03-23");
            var rules = new List<Rule>() { new Rule() { Id = "091", Date = "20230612" }, new Rule() { Id = "10", Date = "20230612" } };
            var rulesExisting = Database.Rules.ToList();
            Database.Rules.Clear();
            Database.Rules.AddRange(rules);
            //  mockIDateHandler.Setup(p => p.ConvertDateTime(It.IsAny<string>(), It.IsAny<string>())).Returns(date);
            ReadInterestRule readInterestRule = new ReadInterestRule(mockIDateHandler.Object);

            // Act
            var result = readInterestRule.GetAllInterestRules();

            // Assert
            Assert.Equal(2, result.Count());

            //revert
            Database.Rules.Clear();
            Database.Rules.AddRange(rulesExisting);
        }
    }
}
