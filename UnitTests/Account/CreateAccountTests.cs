using AwesomeGIC.Infrastructure.Account.Commands;

namespace InfrastructureUnitTests.Account
{
    public class CreateAccountTests
    {
        [Fact]
        public void CreateAccount_Succeess_ReturnsAccountInfo()
        {
            // Arrange
            var createAccount = new CreateAccount();
            var account = new AwesomeGIC.Domain.Entities.Account { Id = "01", Balance = 123 };

            // Act
            var result = createAccount.Create(account);

            // Assert 
            Assert.Equal("01", result.Id);
            Assert.Equal(123, result.Balance);
        }
    }
}
