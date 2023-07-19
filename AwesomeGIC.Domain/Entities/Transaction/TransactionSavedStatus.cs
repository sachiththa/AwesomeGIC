using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Domain.Entities.Transaction
{
    public class TransactionSavedStatus
    {
        public bool IsSaved { get; set; }
        public TransactionInfo TransactionInfo { get; set; }
        public string Error { get; set; }
    }
}
