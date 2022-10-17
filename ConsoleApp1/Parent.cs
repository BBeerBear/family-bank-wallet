public class Parent : User
{
    // store notifications
    public List<Notification> NotificationList = new ();

    // linked BankAccount
    public BankAccount BankAccount { get; set; }

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

    // Reply children children request
    public void ReplyRequestByType(bool isAccepted, Child child, NotificationType notificationType)
    {
        switch (notificationType)
        {
            case NotificationType.ChildRequestToUse:
                child.IsPermittedToUseWallet = isAccepted;
                break;
            case NotificationType.ChildRequestOverpay:
                child.IsOverpayPermitted = isAccepted;
                break;
        }

        // clear the child in the permission list(both parents) by notificationType
        foreach (var parent in DataOperation.GetParentsData())
        {
            var notification = parent.NotificationList.Find(n =>
                n.ChildName == child.Name && n.Type == notificationType);
            if (notification != null) parent.NotificationList.Remove(notification);
        }
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
