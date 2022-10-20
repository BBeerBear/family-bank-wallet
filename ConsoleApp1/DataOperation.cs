public static class DataOperation
{
    
    // store transactions
    public static readonly List<Transaction> TransactionsList = new(); 
    // store all the users 
    public static List<User> Users = new ();
    public static void InitializeData()
    {
        var mom = new Mom()
        {
            Id = 123,
            Password = "abc",
            Name = "Zhangsan",
            BankAccount = new BankAccount()
            {
                Id = 5555111122224444,
                Password = "abc",
                Balance = 100
            }
        };
        Users.Add(mom);

        var dad = new Dad() { 
            Id = 234,
            Password = "abc",
            Name = "Lisi",
            BankAccount = new BankAccount()
            {
                Id = 5555666688889999,
                Password = "abc",
                Balance = 1000
            }
        };
        Users.Add(dad);

        var child1 = new Child()
        {
            Id = 345,
            Name = "Billy",
            Password = "abc",
        };
        Users.Add(child1);

        var child2 = new Child()
        {
            Id = 456,
            Name = "Anna",
            Password = "abc",
        };
        Users.Add(child2);
        
        var child3 = new Child()
        {
            Id = 567,
            Name = "Apple",
            Password = "abc",
        };
        Users.Add(child3);

        var child4 = new Child()
        {
            Id = 678,
            Name = "John",
            Password = "abc",
        };
        Users.Add(child4);

        var child5 = new Child()
        {
            Id = 789,
            Name = "Micheal",
            Password = "abc",
        };
        Users.Add(child5);

        var child6 = new Child()
        {
            Id = 890,
            Name = "Jeff",
            Password = "abc",
        };
        Users.Add(child6);

        var child7 = new Child()
        {
            Id = 999,
            Name = "Alina",
            Password = "abc",
        };
        Users.Add(child7);

        var child8 = new Child()
        {
            Id = 888,
            Name = "Sheff",
            Password = "abc",
        };
        Users.Add(child8);

    }

    public static User? GetUserDataById(long userId)
    {
        return Users.SingleOrDefault(u => u.Id == userId);
    }

    public static User? GetUserDataByName(string userName)
    {
        return Users.SingleOrDefault(u => u.Name == userName);
    }

    public static List<User> GetUsersNotBlocked()
    {
        var usersNotBlocked = (from u in Users
                               where u.IsBlocked == false
                               select u).ToList();
        return usersNotBlocked;
    }

    public static Parent GetParentData(string momOrDad)
    {
        return (Parent)Users.Single(u => u.Role == momOrDad);
    }
    public static Mom GetMomData()
    {
        return (Mom)GetParentData("Mom");
    }
    public static Dad GetDadData()
    {
        return (Dad)GetParentData("Dad");
    }
    public static List<Parent> GetParentsData()
    {
        var parents = (from u in Users
                       where u.Role == "Mom" || u.Role == "Dad"
                       select (Parent)u).ToList();
        return parents;
    }

    public static List<Child> GetChildrenData()
    {
        var children = (from u in Users
                       where u.Role == "Child"
                       select (Child)u).ToList();
        return children;
    }


} 
