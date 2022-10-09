public class User
{
    public long Id { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public bool IsBlocked { get; set; } = false;
    public virtual string Role { get; set; }

    // pay
    public bool Pay(double payMoney, string? shopName)
    {
        // if wallet has enough money
        if (!Wallet.WalletHasEnoughMoney(payMoney)) return false;
        // refresh the balance of the wallet
        Wallet.DeductMoney(payMoney);

        // send notification if balance < $100
        if (Wallet.IsBalanceLessThan100())
        {
            Wallet.SendBalanceNotification();
        }

        // add transaction information
        var transaction = new Transaction()
        {
            UserName = Name,
            ShopName = shopName,
            Money = payMoney,
            TimeStamp = DateTime.Now
        };
        DataOperation.TransactionsList.Add(transaction);

        // pay succeeded
        return true;
    }
}
