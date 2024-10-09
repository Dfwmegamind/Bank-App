public class Program
{
    private static Bank bank = new Bank();

    public static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to the Console Bank Application!");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateAccount();
                    break;
                case "2":
                    Login();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press Enter to try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void CreateAccount()
    {
        Console.Clear();
        Console.WriteLine("Create Account:");
        Console.WriteLine("Select Account Type: 1 for Current, 2 for Savings:");

        if (Enum.TryParse(Console.ReadLine(), out AccountType accountType))
        {
            Console.Write("Enter Account Holder Name: ");
            string accountHolderName = Console.ReadLine();
            Console.Write("Enter PIN: ");
            string pin = Console.ReadLine();

            string accountNumber = bank.CreateAccount(accountType, accountHolderName, pin);
            Console.WriteLine($"Account created successfully! Your account number is: {accountNumber}");
        }
        else
        {
            Console.WriteLine("Invalid account type. Press Enter to try again.");
        }
        Console.ReadLine();
    }

    private static void Login()
    {
        Console.Clear();
        Console.WriteLine("Login:");
        Console.Write("Enter Account Number: ");
        string accountNumber = Console.ReadLine();
        Console.Write("Enter PIN: ");
        string pin = Console.ReadLine();

        var account = bank.Login(accountNumber, pin);
        if (account != null)
        {
            AccountMenu(account);
        }
        else
        {
            Console.WriteLine("Invalid account number or PIN. Press Enter to try again.");
            Console.ReadLine();
        }
    }

    private static void AccountMenu(Account account)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {account.AccountHolderName}!");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Transfer");
            Console.WriteLine("4. View Transaction History");
            Console.WriteLine("5. View Balance");
            Console.WriteLine("6. Logout");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Deposit(account);
                    break;
                case "2":
                    Withdraw(account);
                    break;
                case "3":
                    Transfer(account);
                    break;
                case "4":
                    account.ViewTransactionHistory();
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    break;
                case "5":
                    account.ViewBalance();
                    Console.WriteLine("Press Enter to continue.");
                    Console.ReadLine();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press Enter to try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void Deposit(Account account)
    {
        Console.Clear();
        Console.Write("Enter amount to deposit: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            string result = account.Deposit(amount);
            Console.WriteLine(result);
            bank.SaveAccounts();
        }
        else
        {
            Console.WriteLine("Invalid amount. Press Enter to try again.");
        }
        Console.ReadLine();
    }

    private static void Withdraw(Account account)
    {
        Console.Clear();
        Console.Write("Enter amount to withdraw: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            string result = account.Withdraw(amount);
            Console.WriteLine(result);
            bank.SaveAccounts();
        }
        else
        {
            Console.WriteLine("Invalid amount. Press Enter to try again.");
        }
        Console.ReadLine();
    }

    private static void Transfer(Account account)
    {
        Console.Clear();
        Console.Write("Enter target account number: ");
        string targetAccountNumber = Console.ReadLine();
        Console.Write("Enter amount to transfer: ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            var targetAccount = bank.Login(targetAccountNumber, account.Pin);
            if (targetAccount != null)
            {
                string result = account.Transfer(targetAccount, amount);
                Console.WriteLine(result);
                bank.SaveAccounts();
            }
            else
            {
                Console.WriteLine("Target account not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid amount. Press Enter to try again.");
        }
        Console.ReadLine();
    }
}
