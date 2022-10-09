
// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;

public static class Wallet
{
    public static double Balance { get; set; } = 500;

    public static bool WalletHasEnoughMoney(double payMoney)
    {
        return Balance >= payMoney && Balance != 0;
    }

    public static bool IsBalanceLessThan100()
    {
        return Balance < 100;
    }
    public static bool IsBalanceMoreThan100()
    {
        return Balance >= 100;
    }

    public static void DeductMoney(double money)
    {
        Balance -= money;
    }
    public static void AddMoney(double money)
    {
        Balance += money;
    }
    public static void SendBalanceNotification()
    {
        var parents = DataOperation.GetParentsData();
        foreach(var parent in parents)
        {
            var notificationList = parent.NotificationList;
            // NotificationType.BalanceLessThan100 exist, don't add
            if (notificationList.Count(n => n.Type == NotificationType.BalanceLessThan100) == 0)
            {
                var notification = new Notification()
                {
                    Type = NotificationType.BalanceLessThan100
                };
                notificationList.Add(notification);
            }
        }
    }
    public static void ClearBalanceNotification()
    {
        var parents = DataOperation.GetParentsData();
        foreach (var parent in parents)
        {
            var notificationList = parent.NotificationList;
            notificationList.RemoveAll(n => n.Type == NotificationType.BalanceLessThan100);
        }
    }
}


