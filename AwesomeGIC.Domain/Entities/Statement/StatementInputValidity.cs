namespace AwesomeGIC.Domain.Entities.Statement
{
    public class StatementInputValidity
    {
        public string AccountId { get; set; }
        public int Month { get; set; }
        public bool IsValid { get; set; }   
        public string Error { get; set; }   

    }
}
