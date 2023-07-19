using AwesomeGIC.Domain.Entities.Rule;
using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Domain.Enums;

namespace AwesomeGIC.Infrastructure.DBContext
{
    public static class Database
    {
        // Static arrays to store accounts, transactions and rules
        public static List<Domain.Entities.Account> Accounts = new List<Domain.Entities.Account>()
        { 
            new Domain.Entities.Account{ Id="AC001", Balance=230.00M }
        };
        public static List<TransactionInfo> TransactionInfos = new List<TransactionInfo>()
        {
            new TransactionInfo(){ Id = "20230505-01" , Date="20230505", Type=TransactionType.D, Amount=100.00M, AccountId="AC001", AccountBalance=100.00M },
            new TransactionInfo(){ Id = "20230601-01" , Date="20230601", Type=TransactionType.D, Amount=150.00M, AccountId="AC001", AccountBalance=250.00M },
            new TransactionInfo(){ Id = "20230626-01" , Date="20230626", Type=TransactionType.W, Amount=20.00M, AccountId="AC001", AccountBalance=230.00M },
        };
        public static List<Rule> Rules = new List<Rule>() 
        { 
            new Rule(){ Id = "RULE01", Date="20230101", InterestPercentage=1.95M },
            new Rule(){ Id = "RULE02", Date="20230520", InterestPercentage=1.90M }
        };
    }
}
