using AwesomeGIC.Infrastructure.Account.Commands;
using AwesomeGIC.Infrastructure.Account.Queries;
using AwesomeGIC.Infrastructure.DBContext;

namespace InfrastructureUnitTests.Account
{
    public class ReadAccountTests
    {
        [Fact]
        public void ReadAccount_NoAccountFound_ReturnsNull()
        {
            // Arrange
            var readAccount = new ReadAccount();
            var account = new AwesomeGIC.Domain.Entities.Account { Id = "01", Balance = 123 };
            Database.Accounts.Add(account);

            // Act
            var result = readAccount.GetAccountByID("999");

            // Assert 
            Assert.Null(result);
        }

        [Fact]
        public void ReadAccount_Succeess_ReturnsAccountInfo()
        {
            // Arrange
            var readAccount = new ReadAccount();
            var account = new AwesomeGIC.Domain.Entities.Account { Id = "01", Balance = 123 };
            Database.Accounts.Add(account);

            // Act
            var result = readAccount.GetAccountByID("01");

            // Assert 
            Assert.Equal("01", result.Id);
            Assert.Equal(123, result.Balance);
        }

   
    }
}

