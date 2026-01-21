public class EconomyManager
{
    private int money;

    public EconomyManager()
    {
        money = 10;
    }

    public void SetMoney(int amount)
    {
        money = amount;
    }

    public int GetMoney()
    {
        return money;
    }

    public void AddMoney(int amount)
    {
        money = money + amount;
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money = money - amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}