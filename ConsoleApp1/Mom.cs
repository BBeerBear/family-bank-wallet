public class Mom : Parent
{
    public override string Role { get; set; } = "Mom";

    public void TransferOverpayRequestToDad(Child child)
    {
        var notification = NotificationList.Find(n => n.Type == NotificationType.ChildRequestOverpay && n.ChildName == child.Name);
        if (notification != null)
        {
            NotificationList.Remove(notification);
            DataOperation.GetDadData().NotificationList.Add(notification);
        }
    }

}
