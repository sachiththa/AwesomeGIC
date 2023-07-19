using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Infrastructure.Account.Commands;
using AwesomeGIC.Infrastructure.DBContext;

namespace InfrastructureUnitTests.Account
{
    public class UpdateAccountTests
    {
        [Fact]
        public void UpdateAccount_SucceessWithdrwaTransaction0_DeductsBalance()
        {
            // Arrange
            var updateAccount = new UpdateAccount();
            var account = new AwesomeGIC.Domain.Entities.Account { Id = "01", Balance = 123 };
            var transaction = new TransactionInfo() { Id = "001", Amount = 120, AccountId = "A001", Type = AwesomeGIC.Domain.Enums.TransactionType.W };
            Database.Accounts.Add(account);

            // Act
            var result = updateAccount.Update("01", transaction);

            // Assert 
            Assert.Equal("01", result.Id);
            Assert.Equal(3, result.Balance);
        }

        [Fact]
        public void UpdateAccount_SucceessDepositTransaction0_IncreasesBalance()
        {
            // Arrange
            var updateAccount = new UpdateAccount();
            var account = new AwesomeGIC.Domain.Entities.Account { Id = "01", Balance = 123 };
            var transaction = new TransactionInfo() { Id = "001", Amount = 120, AccountId = "A001", Type = AwesomeGIC.Domain.Enums.TransactionType.D };
            Database.Accounts.Add(account);

            // Act
            var result = updateAccount.Update("01", transaction);

            // Assert 
            Assert.Equal("01", result.Id);
            Assert.Equal(243, result.Balance);
        }
    }
}
