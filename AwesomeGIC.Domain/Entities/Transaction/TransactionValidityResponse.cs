using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Domain.Entities.Transaction
{
    public class TransactionValidityResponse
    {
        public bool IsValid { get; set; }
        public string? Error { get; set; }
        public TransactionInfo? TransactionInfo { get; set; }

    }
}
