using AwesomeGIC.Domain.Entities;
using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Infrastructure.DBContext;
using AwesomeGIC.Infrastructure.Utils;
using System;

namespace AwesomeGIC.Infrastructure.Transaction.Queries
{
    public class ReadTransaction : IReadTransaction
    {
        private readonly IDateHandler _dateHandler;
        public ReadTransaction(IDateHandler dateHandler)
        {
            _dateHandler = dateHandler;
        }

        public List<TransactionInfo> GetAllTransactions()
        {
            return Database.TransactionInfos.ToList();
        }

        public List<TransactionInfo> GetTransactionsByAccount(string accountID)
        {
            return Database.TransactionInfos.FindAll(t => t.AccountId.Equals(accountID));
        }

        public List<TransactionInfo> GetTransactionsByDate(string date)
        {
            return Database.TransactionInfos.FindAll(t => t.Date.Equals(date));
        }

        public List<TransactionInfo> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            return Database.TransactionInfos.FindAll(t => _dateHandler.ConvertDateTime(t.Date, "yyyyMMdd") >= startDate
                && _dateHandler.ConvertDateTime(t.Date, "yyyyMMdd") < endDate);
        }

        public List<TransactionInfo> GetTransactionsOfAccountByMonth(string accountID, int month)
        {
            return Database.TransactionInfos.FindAll(t => t.AccountId.Equals(accountID) && _dateHandler.ConvertDateTime(t.Date, "yyyyMMdd").Month == month);
        }
    }
}
