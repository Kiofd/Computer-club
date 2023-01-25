using System.Collections.Generic;

class Test
{
    static void Main()
    {
        Console.WriteLine("Change");
        ComputerClub computerClub = new ComputerClub(5);
        computerClub.Work();
    }
}

class ComputerClub
{
    private int _money = 0;
    private List<Computer> _computers = new List<Computer>();
    private Queue<Client> _clients = new Queue<Client>();

    public ComputerClub(int computersCount)
    {
        Random random = new Random();

        for (int i = 0; i < computersCount; i++)
        {
            _computers.Add(new Computer(random.Next(5, 15)));
        }
        CreatNewClient(25, random);
    }
    public void CreatNewClient(int count, Random random)
    {
        for (int i = 0; i < count; i++)
        {
            _clients.Enqueue(new Client(random.Next(100, 200), random.Next(5, 15)));
        }
    }
    public void Work()
    {
        while (_clients.Count > 0)
        {
            Client newClient = _clients.Dequeue();
            Console.WriteLine($"Computer Club moneys: {_money}");
            Console.WriteLine($"You have new client he have {newClient.Money} money and want {newClient.DesiredTime} minutes");
            ShowAllComputersInfo();
            Console.WriteLine(new String('-', 46));

            Console.Write($"\nFot which computer do you want to put your client on: ");

            string userComputer = Console.ReadLine();

            if (int.TryParse(userComputer, out int computerNumber))
            {
                computerNumber -= 1;
                if (computerNumber >= 0 && computerNumber < _computers.Count) //WTF why we i change the condition the program did not work like i need
                {
                    if (_computers[computerNumber].IsTaken)
                    {
                        Console.WriteLine("This computer is taken, clien go home");
                    }
                    else
                    {
                        if (newClient.AbleToPay(_computers[computerNumber].PricePerMinute))
                        {
                            Console.WriteLine("Client pay for computer");
                            _money += newClient.Pay();
                            _computers[computerNumber].BecomeTaken(newClient);
                        }
                        else
                        {
                            Console.WriteLine("Client don`t have too much money");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("We don`t have that computer");
                }

            }

            else
            {
                Console.WriteLine("You input wrong option. Try Again");
                CreatNewClient(1, new Random());
            }

            Console.ReadKey(true);
            Console.Clear();
            SpendOneMinute();
        }
    }
    private void ShowAllComputersInfo()
    {
        for (int i = 0; i < _computers.Count; i++)
        {
            Console.Write(i + 1 + " - ");
            _computers[i].ShowComputerInfo();
        }
    }
    private void SpendOneMinute()
    {
        foreach (var compute in _computers)
        {
            compute.SpendOneMinute();
        }
    }
}
class Computer
{
    private Client _clients;
    private int _minutesRemaining;
    public int PricePerMinute { get; private set; }

    public bool IsTaken
    {
        get { return _minutesRemaining > 0; }
    }

    public Computer(int pricePerMinute)
    {
        PricePerMinute = pricePerMinute;
    }
    public void ShowComputerInfo()
    {
        if (IsTaken)
            Console.WriteLine($"Computer is busy, remaining {_minutesRemaining} minutes");
        else
            Console.WriteLine($"Computer is free, the price {PricePerMinute} per minute");
    }
    public void BecomeTaken(Client client)
    {
        _clients = client;
        _minutesRemaining = _clients.DesiredTime;
    }
    public void BecomeEmpty()
    {
        _clients = null;
    }
    public void SpendOneMinute()
    {
        _minutesRemaining--;
    }

}

class Client
{
    public int Money { get; private set; }
    private int _moneyToPay;
    public int DesiredTime { get; private set; }

    public Client(int money, int desiredTime)
    {
        Money = money;
        DesiredTime = desiredTime;
    }
    public void ShowClientInfo()
    {
        Console.WriteLine($"Client have {Money} money and want {DesiredTime} minutes to sit");
    }
    public bool AbleToPay(int pricePerMinute)
    {
        _moneyToPay -= pricePerMinute * DesiredTime;
        if (Money > _moneyToPay)
            return true;
        else
            return false;
    }
    public int Pay()
    {
        Money -= _moneyToPay;
        return _moneyToPay;
    }
}