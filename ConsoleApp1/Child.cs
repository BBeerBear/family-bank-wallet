
public class Child : User
{
    public override string Role { get; set; } = "Child";
    public double UsedMoney { get; set; }
    private readonly double _maxPayOneDay = 50;
    public bool IsOverpayPermitted { get; set; } = false;
    public int AskPermissionTimes { get; set; }
    public bool IsPermittedToUseWallet { get; set; } = false;

    // two times a day child need to ask permission for using the wallet
    public void AskPermissionToUseWallet(string childName)
    {
        Console.WriteLine("Asking permission to use the wallet....");
        foreach (var parent in DataOperation.GetParentsData())
        {
            var notification = new Notification()
            {
                Type = NotificationType.ChildRequestToUse,
                ChildName = childName
            };
            parent.NotificationList.Add(notification);
        }
        AskPermissionTimes += 1;
    }

    public bool IsOverpay(double money)
    {
        return UsedMoney + money > _maxPayOneDay;
    }

    public void MessageParentsToDeposit()
    {
        var parents = DataOperation.GetParentsData();
        foreach (var parent in parents)
        {
            var notification = new Notification
            {
                Type = NotificationType.ChildRequestDeposit,
                ChildName = Name
            };
            parent.NotificationList.Add(notification);
        }
    }

    public bool RequestOverpay()
    {
        // add overpay request to notification list
        var mom = DataOperation.GetMomData();
        var list = mom.NotificationList;
        if (list.Count(n => n.ChildName == Name && n.Type == NotificationType.ChildRequestOverpay) != 0)
            return IsOverpayPermitted;
        var notification = new Notification()
        {
            ChildName = Name,
            Type = NotificationType.ChildRequestOverpay
        };
        list.Add(notification);

        // get reply of overpay request
        return IsOverpayPermitted;
    }
}
