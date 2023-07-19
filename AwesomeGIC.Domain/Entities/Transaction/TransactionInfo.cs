using AwesomeGIC.Domain.Enums;

namespace AwesomeGIC.Domain.Entities.Transaction
{
    public class TransactionInfo
    {
        public string Id { get; set; }
        //public Account Account { get; set; }
        public string AccountId { get; set; }
        public decimal AccountBalance { get; set; }
        public string Date { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }

    }
}