public class Dad : Parent
{
    public override string Role { get; set; } = "Dad";
    public void Block(User user)
    {
        user.IsBlocked = true;
    }
}
