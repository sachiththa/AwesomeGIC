using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Infrastructure.DBContext;
using AwesomeGIC.Infrastructure.Transaction.Commands;

namespace InfrastructureUnitTests.Transaction
{
    public class SaveTransactionTests
    {
        [Fact]
        public void SaveTransaction_Succeess_ReturnsTransactionStatusWithTransactionInfo()
        {
            // Arrange
            SaveTransaction saveTransaction = new SaveTransaction();
            var transaction = new TransactionInfo() { Id = "001", AccountBalance = 123, AccountId = "A001" };

            // Act
            var result = saveTransaction.Save(transaction);

            // Assert
            Assert.True(result.IsSaved);
            Assert.Equal("001", result.TransactionInfo.Id);
            Assert.Equal(123, result.TransactionInfo.AccountBalance);
        }

        [Fact]
        public void SaveTransaction_Succeess_ReturnsTransactionStatusWithNoErrors()
        {
            // Arrange
            SaveTransaction saveTransaction = new SaveTransaction();
            var transaction = new TransactionInfo() { Id = "001", AccountBalance = 123, AccountId = "A001" };

            // Act
            var result = saveTransaction.Save(transaction);

            // Assert
            Assert.Equal("", result.Error);
        }

        [Fact]
        public void SaveTransaction_ExceptionOccured_ReturnsTransactionStatusWithError()
        {
            // Arrange
            SaveTransaction saveTransaction = new SaveTransaction();
            var transaction = new TransactionInfo() { Id = "001", AccountBalance = 123, AccountId = "A001" };
            Database.TransactionInfos = null;

            // Act
            var result = saveTransaction.Save(transaction);

            // Assert
            Assert.Equal("Object reference not set to an instance of an object.", result.Error);

            // Revert arrange
            Database.TransactionInfos = new List<TransactionInfo>();
        }

        [Fact]
        public void SaveTransaction_ExceptionOccured_ReturnsTransactionStatusWithTransactionInfo()
        {
            // Arrange
            SaveTransaction saveTransaction = new SaveTransaction();
            var transaction = new TransactionInfo() { Id = "001", AccountBalance = 123, AccountId = "A001" };
            Database.TransactionInfos = null;

            // Act
            var result = saveTransaction.Save(transaction);

            // Assert
            Assert.False(result.IsSaved);
            Assert.Equal("001", result.TransactionInfo.Id);
            Assert.Equal(123, result.TransactionInfo.AccountBalance);

            // Revert arrange
            Database.TransactionInfos = new List<TransactionInfo>();
        }
    }
}