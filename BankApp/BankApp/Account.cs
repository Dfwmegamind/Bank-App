public enum AccountType
{
    Current = 1,
    Savings = 2
}

public abstract class Account
{
    public string AccountNumber { get; private set; }
    public string AccountHolderName { get; private set; }
    protected decimal Balance { get; set; }
    public string Pin { get; private set; }
    public List<string> TransactionHistory { get; private set; }

    protected Account(string accountNumber, string accountHolderName, string pin)
    {
        AccountNumber = accountNumber;
        AccountHolderName = accountHolderName;
        Pin = pin;
        Balance = 0;
        TransactionHistory = new List<string>();
    }

    public string Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            return "Deposit amount must be positive.";
        }

        Balance += amount;
        TransactionHistory.Add($"Deposited: {amount}. New Balance: {Balance}");
        return "Deposit successful!";
    }

    public abstract string Withdraw(decimal amount);

    public string Transfer(Account targetAccount, decimal amount)
    {
        string withdrawalResult = Withdraw(amount);
        if (withdrawalResult != "Withdrawal successful!")
        {
            return withdrawalResult;
        }

        targetAccount.Deposit(amount);
        TransactionHistory.Add($"Transferred: {amount} to {targetAccount.AccountNumber}. New Balance: {Balance}");
        return "Transfer successful!";
    }

    public void ViewTransactionHistory()
    {
        Console.WriteLine($"Transaction History for {AccountHolderName} (Account: {AccountNumber}):");
        foreach (var transaction in TransactionHistory)
        {
            Console.WriteLine(transaction);
        }
    }

    public void ViewBalance()
    {
        Console.WriteLine($"Account Balance for {AccountHolderName} (Account: {AccountNumber}): {Balance}");
    }
}

public class CurrentAccount : Account
{
    private const decimal MinimumBalance = 5000;

    public CurrentAccount(string accountNumber, string accountHolderName, string pin)
        : base(accountNumber, accountHolderName, pin) { }

    public override string Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            return "Withdrawal amount must be positive.";
        }

        if (Balance - amount < MinimumBalance)
        {
            return $"Insufficient balance. Current account must maintain a minimum balance of {MinimumBalance}.";
        }

        Balance -= amount;
        TransactionHistory.Add($"Withdrew: {amount}. New Balance: {Balance}");
        return "Withdrawal successful!";
    }
}

public class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, string accountHolderName, string pin)
        : base(accountNumber, accountHolderName, pin) { }

    public override string Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            return "Withdrawal amount must be positive.";
        }

        if (Balance < amount)
        {
            return "Insufficient balance.";
        }

        Balance -= amount;
        TransactionHistory.Add($"Withdrew: {amount}. New Balance: {Balance}");
        return "Withdrawal successful!";
    }
}
