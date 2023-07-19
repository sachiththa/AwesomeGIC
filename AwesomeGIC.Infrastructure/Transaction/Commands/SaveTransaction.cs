using AwesomeGIC.Domain.Entities.Transaction;
using AwesomeGIC.Infrastructure.DBContext;

namespace AwesomeGIC.Infrastructure.Transaction.Commands
{
    public class SaveTransaction : ISaveTransaction
    {

        public TransactionSavedStatus Save(TransactionInfo transactionToSave)
        {
            try
            {
                //save
                Database.TransactionInfos.Add(transactionToSave);
                return new TransactionSavedStatus() { IsSaved = true, TransactionInfo = transactionToSave, Error = "" };

            }
            catch (Exception ex)
            {
                return new TransactionSavedStatus() { IsSaved = false, Error = ex.Message, TransactionInfo = transactionToSave };
            }
        }
    }
}
