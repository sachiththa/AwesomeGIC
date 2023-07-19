using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGIC.Application
{
    public interface IBankingOperations
    {
        void LaunchApp();
        void ShowBankingOptions();
        void HandleUserInput(string userChoice);
    }
}
