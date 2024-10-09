using Newtonsoft.Json;

public class Bank
{
    private List<Account> accounts;
    private readonly string dataFile = "accounts.json";
    private int accountCounter;

    public Bank()
    {
        accountCounter = 0;
        LoadAccounts();
    }

    public string CreateAccount(AccountType accountType, string accountHolderName, string pin)
    {
        // Generate a unique account number
        string accountNumber = $"ACC{(accountCounter + 1).ToString("D8")}";

        // Ensure the account doesn't already exist
        if (accounts.Any(a => a.AccountHolderName == accountHolderName && a.AccountNumber == accountNumber))
        {
            return "Account with this holder and account number already exists.";
        }

        // Create the account based on type
        Account account = accountType switch
        {
            AccountType.Current => new CurrentAccount(accountNumber, accountHolderName, pin),
            AccountType.Savings => new SavingsAccount(accountNumber, accountHolderName, pin),
            _ => throw new InvalidOperationException("Invalid account type.")
        };

        // Add the account and increment the counter
        accounts.Add(account);
        accountCounter++;
        SaveAccounts();

        return accountNumber;
    }

    public Account Login(string accountNumber, string pin)
    {
        var account = accounts.FirstOrDefault(a => a.AccountNumber == accountNumber && a.Pin == pin);
        return account ?? null;
    }

    public void SaveAccounts()
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { Accounts = accounts, AccountCounter = accountCounter },
                        new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(dataFile, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving accounts: {ex.Message}");
        }
    }

    public void LoadAccounts()
    {
        try
        {
            if (File.Exists(dataFile))
            {
                var json = File.ReadAllText(dataFile);
                var deserialized = JsonConvert.DeserializeObject<BankData>(json,
                                      new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
                accounts = deserialized.Accounts ?? new List<Account>();
                accountCounter = deserialized.AccountCounter;
            }
            else
            {
                accounts = new List<Account>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading accounts: {ex.Message}");
            accounts = new List<Account>();
        }
    }
}

public class BankData
{
    public List<Account> Accounts { get; set; }
    public int AccountCounter { get; set; }
}
