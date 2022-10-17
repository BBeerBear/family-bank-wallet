
public static class WalletUI
{

    private static readonly DateTime currentTime = DateTime.Now;

    public static void UserLoginUI()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("-------------------Login-----------------------");
            Console.WriteLine("Welcome to the Family Wallet!\n");
            Console.WriteLine("Login your account:");

            // Add user id
            Console.Write("Please enter you id: ");
            if (!long.TryParse(Console.ReadLine(), out var id))
            {
                continue;
            }

            // Find the login user
            var user = DataOperation.GetUserDataById(id);

            // if user exists
            if (user == null)
            {
                Console.WriteLine("\n!!! Sorry, the user doesn't exist");
                continue;
            }

            // if user is blocked
            if (user.IsBlocked)
            {
                Console.WriteLine("\n!!! Sorry, you are not allowed to use the wallet.");
                continue;
            }

            // Add password
            Console.Write("Please enter you password: ");
            var password = Console.ReadLine();

            // if user password correct
            if (user.Password != password)
            {
                Console.WriteLine("\n!!! Sorry, your password is wrong.");
                continue;
            }

            //Enter the user interface by user role
            UserUI(user);
            break;
        }
    }

    public static void UserUI(User user)
    {
        Console.WriteLine("\n-----------------------------------------------");
        Console.WriteLine("Hello, " + user.Name + "!(Time: " + currentTime + ")");

        // show balance
        switch (user.Role)
        {
            case "Mom":
            case "Dad":
                ParentUI(user);
                break;
            case "Child":
                ChildUI(user);
                break;
        }
    }

    public static void ParentUI(User user)
    {
        while (true)
        {
            var parent = (Parent)user;

            // show the amount of the balance
            Console.WriteLine("--------------------Home Page------------------------");
            Console.WriteLine("The balance of the Family Wallet:" + Wallet.Balance);

            // show notification if balance is less than 100
            if (Wallet.IsBalanceLessThan100()) Console.WriteLine("!!! Warnings: " + parent.GetNotificationsByType(NotificationType.BalanceLessThan100).SingleOrDefault()?.Content);

            // Show available operations
            if (parent.Role == "Dad")
            {
                Console.WriteLine("0 Block");
            }

            Console.WriteLine("1 Pay");
            Console.WriteLine("2 Withdraw");
            Console.WriteLine("3 Deposit");
            Console.WriteLine("4 View all transactions");
            Console.WriteLine("5 View my notifications (" + parent.NotificationList.Count() + ")");
            Console.WriteLine("6 Give children permission to use the wallet");
            Console.WriteLine("7 Reply children overpay request");
            Console.WriteLine("8 Log out");
            Console.WriteLine("9 Exit");

            Console.Write("Input your operation number: ");
            if (int.TryParse(Console.ReadLine(), out var optId))
            {
                Console.WriteLine("-----------------------------------------------------");
                switch (optId)
                {
                    // Block
                    case 0 when parent.Role == "Dad":
                        DadBlockUI((Dad)parent);
                        break;

                    // Pay
                    case 1:
                        ParentPayUI(parent);
                        break;

                    // Withdraw
                    case 2:
                        ParentWithdrawUI(parent);
                        break;

                    // deposit
                    case 3:
                        ParentDepositUI(parent);
                        break;

                    // View all transactions
                    case 4:
                        ParentViewTransactionsUI();
                        break;

                    // view my notifications
                    case 5:
                        ParentNotificationsUI(parent);
                        break;

                    // give child permission to use the wallet
                    case 6:
                        ParentGiveChildPermissionUI(parent);
                        break;

                    // reply children request of overpay
                    case 7:
                        ParentReplyChildOverpayUI(parent);
                        break;

                    // log out
                    case 8:
                        UserLoginUI();
                        return;

                    // exit
                    case 9:
                        Environment.Exit(0);
                        break;
                }
            }

            // return to user interface
            user = parent;
        }
    }

    private static void ParentNotificationsUI(Parent parent)
    {
        Console.WriteLine("------------------My Notifications------------------");
        var list = parent.NotificationList;
        foreach (var notification in list)
        {
            Console.WriteLine(notification.Content);
        }
        Console.WriteLine();
    }

    private static void ParentPayUI(User user)
    {

        // input the amount of money
        Console.Write("Input the amount of pay money:");
        if (!double.TryParse(Console.ReadLine(), out var payMoney)) return;
        // input shop name
        Console.Write("Input the shop name: ");
        var shopName = Console.ReadLine();

        Console.WriteLine();
        // pay money
        Console.WriteLine(user.Pay(payMoney, shopName)
            ? "!!! Pay succeeded"
            : "!!! Pay failed, the balance of the wallet is not sufficient.");
    }

    private static void ParentWithdrawUI(Parent parent)
    {
        // input the amount of money
        Console.Write("Input the amount of withdraw money:");
        if (!double.TryParse(Console.ReadLine(), out var withdrawMoney)) return;
        // withdraw money
        Console.WriteLine(parent.Withdraw(withdrawMoney)
            ? "\n!!! Withdraw succeeded"
            : "\n!!! Withdraw failed, the balance is not sufficient.");
    }

    private static void ParentDepositUI(Parent parent)
    {
        // input the amount of money
        Console.Write("Input the amount of deposit money: ");
        if (double.TryParse(Console.ReadLine(), out var depositMoney))
        {
            // deposit money
            Console.WriteLine(parent.Deposit(depositMoney)
                ? "!!! Deposit succeeded."
                : "!!! Deposit failed. No enough money in your bank account.");
        }
        Console.WriteLine();
    }

    private static void DadBlockUI(Dad dad)
    {
        while (true)
        {
            // show all users who are not blocked
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Users who are not blocked");
            Console.WriteLine("--------------------------------");
            Console.WriteLine("User Role |User Id   |User Name");
            Console.WriteLine("--------------------------------");
            var users = DataOperation.GetUsersNotBlocked();
            foreach (var user in users)
            {
                Console.WriteLine($"{user.Role,-10}|{user.Id,-10}|{user.Name,-10}");
            }

            // select the user you want to block
            Console.Write("Input the user id that you want to block: ");
            if (!long.TryParse(Console.ReadLine(), out var userId)) return;
            var selectedUser = DataOperation.GetUserDataById(userId);
            if (selectedUser == null)
            {
                Console.WriteLine("!!! Sorry, the user is not found.");
                // return block UserUI
                continue;
            }

            dad.Block(selectedUser);
            Console.WriteLine("Block succeeded!");
            break;
        }
    }

    public static void ParentReplyChildOverpayUI(Parent parent)
    {
        // show children who have requested for overpay
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Children who have requested for overpay");
        Console.WriteLine("----------------------------------------");
        var notifications = parent.GetNotificationsByType(NotificationType.ChildRequestOverpay);
        var childList = new List<string>();
        foreach(var notification in notifications)
        {
            Console.WriteLine(notification.ChildName);
            if (notification.ChildName != null) childList.Add(notification.ChildName);
        }
        Console.WriteLine("\n1. Reply children overpay request");
        Console.WriteLine("2. Return to home page");
        Console.Write("Input operation number:");
        if (!int.TryParse(Console.ReadLine(), out var optId)) return;
        switch (optId)
        {
            case 1:
                // select a child to accept his overpay
                Console.WriteLine("Select a child you want to accept or reject request");
                Console.Write("Input child name: ");
                var childName = Console.ReadLine();
                if (childName == null || !childList.Contains(childName))
                {
                    Console.WriteLine("!!! The child name doesn't exist or he didn't request overpay.\n");
                    return;
                }

                var selectedChild = (Child)DataOperation.GetUserDataByName(childName)!;
                // if the user is mom
                if (parent.Role == "Mom")
                {
                    Console.WriteLine("Do you want to transfer the child's request to dad?");
                    Console.Write("Input Y(Yes) or N(No): ");
                    var res = Console.ReadLine();
                    switch (res)
                    {
                        // transfer overpay request to dad
                        case "Y":
                            ((Mom)parent).TransferOverpayRequestToDad(selectedChild);
                            break;

                        // decide by herself whether accept overpay
                        case "N":
                            ParentAcceptOverpayOrNotUI(parent, selectedChild);
                            break;
                    }
                }

                // if the user is dad
                else
                {
                    ParentAcceptOverpayOrNotUI(parent, selectedChild);
                }
                break;
            case 2:
                ParentUI(parent);
                break;
        }

    }

    private static void ParentAcceptOverpayOrNotUI(Parent parent, Child selectedChild)
    {
        Console.WriteLine("Accept overpay or not?");
        Console.Write("Input Y(Yes) or N(No): ");
        var res = Console.ReadLine();
        switch (res)
        {
            case "Y":
                parent.ReplyRequestByType(true, selectedChild, NotificationType.ChildRequestOverpay);
                break;
            case "N":
                parent.ReplyRequestByType(false, selectedChild, NotificationType.ChildRequestOverpay);
                break;
        }
    }

    public static void ParentGiveChildPermissionUI(Parent parent)
    {
        // show children who ask permission to use the wallet
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine("Children who ask permission to use the wallet:");
        Console.WriteLine("------------------------------------------------");
        var childList = new List<string>();
        var notifications = parent.GetNotificationsByType(NotificationType.ChildRequestToUse);
        foreach (var notification in notifications)
        {
            Console.WriteLine(notification.ChildName);
            if (notification.ChildName != null) childList.Add(notification.ChildName);
        }

        // operations
        Console.WriteLine("\n1. Reply children requests of using the wallet");
        Console.WriteLine("2. Return to home page");
        Console.Write("Input operation number:");

        if (!int.TryParse(Console.ReadLine(), out var optId)) return;
        switch (optId)
        {
            case 1:
                // select a child to accept his overpay
                Console.WriteLine("Select a child you want to accept or reject request");
                Console.Write("Input child name: ");
                var childName = Console.ReadLine();
                if (childName == null || !childList.Contains(childName))
                {
                    Console.WriteLine("!!! The child name doesn't exist or he didn't ask permission.\n");
                    return;
                }
                var selectedChild = (Child)DataOperation.GetUserDataByName(childName)!;
                ParentAcceptRightToUserNotUI(parent, selectedChild);
                break;
            case 2:
                ParentUI(parent);
                break;
        }
    }

    private static void ParentAcceptRightToUserNotUI(Parent parent, Child selectedChild)
    {
        Console.WriteLine("Accept permission or not?");
        Console.Write("Input Y(Yes) or N(No): ");
        var res = Console.ReadLine();
        switch (res)
        {
            case "Y":
                parent.ReplyRequestByType(true, selectedChild, NotificationType.ChildRequestToUse);
                break;
            case "N":
                parent.ReplyRequestByType(false, selectedChild, NotificationType.ChildRequestToUse);
                break;
        }
    }
    public static void ParentViewTransactionsUI()
    {
        Console.WriteLine("------------------------------------------------------");
        Console.WriteLine("DateTime                 |ShopName       |Money     ");
        Console.WriteLine("------------------------------------------------------");
        var list = DataOperation.TransactionsList;
        foreach (var transaction in list)
        {
            Console.WriteLine($"{transaction.TimeStamp,-25}|{transaction.ShopName,-15}|{transaction.Money,-10}");
        }

        Console.WriteLine();
        Console.Write("Press any key to home page:");
        Console.ReadKey();
    }

    // child home page
    public static void ChildUI(User user)
    {
        while (true)
        {
            var child = (Child)user;

            // child should ask permission if his/her permission times is less than 2 and he/she haven't request a permission
            var count = DataOperation.GetMomData().NotificationList.Count(n => n.ChildName == child.Name && n.Type == NotificationType.ChildRequestToUse);
            if (child.AskPermissionTimes < 2 && count == 0)
            {
                child.IsPermittedToUseWallet = false;
                child.AskPermissionToUseWallet(child.Name);
            }

            // still not permitted, then return to login interface
            if (!child.IsPermittedToUseWallet)
            {
                Console.WriteLine("!!! Sorry, you are not allowed to use the wallet.");
                UserLoginUI();
                return;
            }

            // home page
            Console.WriteLine("--------------------Home Page------------------------");
            Console.WriteLine("The balance of the Family Wallet:" + Wallet.Balance);
            Console.WriteLine("1. Pay");
            Console.WriteLine("2. Log out");
            Console.WriteLine("3. Exit");
            Console.Write("Input your operation(number):");
            if (int.TryParse(Console.ReadLine(), out int optId))
            {
                Console.WriteLine("-----------------------------------------------------");
                switch (optId)
                {
                    // pay
                    case 1:
                        ChildPayUI(child);
                        break;

                    // log out
                    case 2:
                        UserLoginUI();
                        return;

                    // exit
                    case 3:
                        Environment.Exit(0);
                        break;
                }
            }

            //return to child home page
            user = child;
        }
    }

    private static void ChildPayUI(Child child)
    {

        // Input the amount of money
        Console.Write("Input the amount of pay money:");
        if (!double.TryParse(Console.ReadLine(), out var payMoney)) return;
        // Determine if child overpay, if overpay, send overpay request to mom
        if (child.IsOverpay(payMoney) && !child.IsOverpayPermitted)
        {
            Console.WriteLine("Sending a overpay request to mom....");
            var overpayAccepted = child.RequestOverpay();

            // overpay rejected
            if (!overpayAccepted)
            {
                Console.WriteLine("!!! Pay failed, you are not allowed to overpay\n");
                return;
            }
        }

        // pay
        // add shop name
        Console.Write("Input the shop name: ");
        var shopName = Console.ReadLine();
        Console.WriteLine();

        // Determine if there is enough money in the wallet
        if (!child.Pay(payMoney, shopName))
        {
            Console.WriteLine("!!! Pay failed, the balance is not sufficient.");
            Console.WriteLine("Requesting a money deposit....");
            child.MessageParentsToDeposit();
            return;
        }

        Console.WriteLine("!!! Pay succeeded");
    }
}


