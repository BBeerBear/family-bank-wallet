public class Parent : User
{
    // store notifications
    public List<Notification> NotificationList = new ();

    // linked BankAccount
    public BankAccount BankAccount { get; set; }

    //// pay
    //public bool Pay(double payMoney, string? shopName)
    //{
    //    // if wallet has enough money
    //    if (!Wallet.WalletHasEnoughMoney(payMoney)) return false;
    //    // refresh the balance of the wallet
    //    Wallet.DeductMoney(payMoney);

    //    // send notification if balance < $100
    //    if (Wallet.IsBalanceLessThan100())
    //    {
    //        Wallet.SendBalanceNotification();
    //    }

    //    // add transaction information
    //    var transaction = new Transaction()
    //    {
    //        UserName = Name,
    //        ShopName = shopName,
    //        Money = payMoney,
    //        TimeStamp = DateTime.Now
    //    };
    //    DataOperation.TransactionsList.Add(transaction);

    //    // pay succeeded
    //    return true;
    //}

    // withdraw
    public bool Withdraw(double withdrawMoney)
    {
        return Pay(withdrawMoney, null);
    }

    // deposit
    public bool Deposit(double depositMoney)
    {
        if (!BankAccount.HaveEnoughMoney(depositMoney)) return false;
        
        // refresh the balance of the bank account
        BankAccount.Balance -= depositMoney;

        // refresh the balance of the wallet
        Wallet.AddMoney(depositMoney);

        // if balance >= $100, clear notification
        if (Wallet.IsBalanceMoreThan100())
            Wallet.ClearBalanceNotification();

        // deposit succeeded
        return true;

    }

    // Permit child to use the wallet
    public void PermitChildToUseWallet(bool isAccepted, Child child)
    {
        child.IsPermittedToUseWallet = isAccepted;

        // clear the child in the permission list(both parents)
        foreach (var parent in DataOperation.GetParentsData())
        {
            var notification = NotificationList.Find(n =>
                n.ChildName == child.Name && n.Type == NotificationType.ChildRequestToUse);
            if (notification != null) parent.NotificationList.Remove(notification);
        }
    }

    // parent reply children overpay request
    public void ReplyOverpayRequest(bool isAccepted, Child child)
    {
        child.IsOverpayPermitted = isAccepted;
        // remove from the notification list
        var notification = NotificationList.Find(n => 
            n.ChildName == child.Name && n.Type == NotificationType.ChildRequestOverpay);
        if (notification != null) NotificationList.Remove(notification);
    }

    // get notifications according to type
    public List<Notification> GetNotificationsByType(NotificationType type)
    {
        var list = (from n in NotificationList
                    where n.Type == type
                    select n).ToList();
        return list;
    }
}
