public class BankAccount
{
    public long Id { get; set; }
    public string Password { get; set; }
    public double Balance { get; set; }
    public bool HaveEnoughMoney(double depositMoney)
    {
        return Balance >= depositMoney && Balance != 0;
    }

}