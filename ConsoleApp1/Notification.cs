
public class Notification
{
    public string? ChildName { get; set; }
    public NotificationType Type { get; set; }

    public string Content
    {
        get
        {
            return Type switch
            {
                NotificationType.BalanceLessThan100 => "The balance of the Family Wallet is less than $100!",
                NotificationType.ChildRequestDeposit => ChildName +
                                                        ": The balance is not enough for me to use, please make a money deposit!",
                NotificationType.ChildRequestToUse => ChildName + ": Please give me permission to use the wallet.",
                NotificationType.ChildRequestOverpay => ChildName + ": Please give me permission to overpay.",
                _ => ""
            };
        } 
    }

   
}
