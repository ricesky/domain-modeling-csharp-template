using PersonalFinanceManagementSystem.Domain.Entity;
using PersonalFinanceManagementSystem.Domain.ValueObject;

namespace DomainModeling.Tests;

[TestClass]
public class PersonalFinanceTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Amount_ShouldThrowException_ForNegativeValue()
    {
        new Amount(-100);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TransactionCategory_ShouldThrowException_ForInvalidCategory()
    {
        new TransactionCategory("Food");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TransactionDate_ShouldThrowException_ForFutureDate()
    {
        new TransactionDate(DateTime.Now.AddDays(1));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TransactionDescription_ShouldThrowException_ForEmptyDescription()
    {
        new TransactionDescription("");
    }

    [TestMethod]
    public void Account_ShouldUpdateBalance_ForValidIncomeTransaction()
    {
        var account = new Account("Personal Savings", new Amount(1000));
        var transaction = new Transaction(new Amount(500), new TransactionCategory("Income"), new TransactionDate(DateTime.Now), new TransactionDescription("Freelance Project"));
        account.AddTransaction(transaction);
        Assert.AreEqual(1500, account.Balance.Value);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Account_ShouldThrowException_ForInsufficientBalance()
    {
        var account = new Account("Personal Savings", new Amount(1000));
        var transaction = new Transaction(new Amount(1500), new TransactionCategory("Expense"), new TransactionDate(DateTime.Now), new TransactionDescription("Laptop Purchase"));
        account.AddTransaction(transaction);
    }
}
