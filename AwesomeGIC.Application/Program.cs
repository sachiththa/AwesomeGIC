using AwesomeGIC.Application;
using AwesomeGIC.Application.Rules;
using AwesomeGIC.Application.Statement;
using AwesomeGIC.Application.Transactions;
using AwesomeGIC.Infrastructure.Account.Commands;
using AwesomeGIC.Infrastructure.Account.Queries;
using AwesomeGIC.Infrastructure.InterestRule.Commands;
using AwesomeGIC.Infrastructure.InterestRule.Queries;
using AwesomeGIC.Infrastructure.Transaction.Commands;
using AwesomeGIC.Infrastructure.Transaction.Queries;
using AwesomeGIC.Infrastructure.Utils;
using Microsoft.Extensions.DependencyInjection;

// Create the service container and register dependencies
var builder = new ServiceCollection()
    .AddSingleton<IBankingOperations, BankingOperations>()
    .AddTransient<IDateHandler, DateHandler>()
    // Transaction dependencies
    .AddTransient<ISaveTransaction, SaveTransaction>()
    .AddTransient<IReadTransaction, ReadTransaction>()
    .AddTransient<ITransactionHandler, TransactionHandler>()
    // Account dependencies
    .AddTransient<ICreateAccount, CreateAccount>()
    .AddTransient<IReadAccount, ReadAccount>()
    .AddTransient<IUpdateAccount, UpdateAccount>()
    // Rule dependencies 
    .AddTransient<IRuleHandler, RuleHandler>()
    .AddTransient<ISaveInterestRule, SaveInterestRule>()
    .AddTransient<IReplaceInterestRule, ReplaceInterestRule>()
    .AddTransient<IReadInterestRule, ReadInterestRule>()
    // Statement dependencies
    .AddTransient<IStatementHandler, StatementHandler>()
    .BuildServiceProvider();
IBankingOperations bankingOperations = builder.GetRequiredService<IBankingOperations>();
bankingOperations.LaunchApp();


