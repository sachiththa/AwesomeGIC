using AwesomeGIC.Domain.Entities.Statement;
namespace AwesomeGIC.Application.Statement
{
    public interface IStatementHandler
    {
        StatementInputValidity IsValidPrintStatementInput(string[] input);
        void PrintStatement(string accountId, int month);
    }
}
