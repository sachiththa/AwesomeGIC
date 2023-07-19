using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Infrastructure.DBContext;
using AwesomeGIC.Infrastructure.Transaction.Commands;
using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Infrastructure.InterestRule.Commands;

namespace InfrastructureUnitTests.InterestRule
{
    public class ReplaceInterestRuleTests
    {
        [Fact]
        public void ReplaceInterestRule_Succeess_ReturnsReplaceInterestRuleInfo()
        {
            // Arrange
            var rules = new List<Rule>() { new Rule() { Id = "01", Date = "20230612" }, new Rule() { Id = "01", Date = "20230612" } };
            Database.Rules.AddRange(rules);
            ReplaceInterestRule replaceInterestRule = new ReplaceInterestRule();

            // Act
            var result = replaceInterestRule.Replace(rules.First());

            // Assert
            Assert.True(result.IsSaved);
        }

        [Fact]
        public void ReplaceInterestRule_ExceptionOccured_ReturnsReplaceInterestWithError()
        {
            // Arrange
            var rules = new List<Rule>() { new Rule() { Id = "01", Date = "20230612" }, new Rule() { Id = "01", Date = "20230612" } };
            Database.Rules = null;
            ReplaceInterestRule replaceInterestRule = new ReplaceInterestRule();

            // Act
            var result = replaceInterestRule.Replace(rules.First());

            // Assert
            Assert.False(result.IsSaved);

            // revert
            Database.Rules = new List<Rule>();
        }
    }
}
