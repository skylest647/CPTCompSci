using UnityEngine;

public class EconomyManager
{
    private int money;  

    public void SetMoney(int amount)
    {
        money = amount;
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            return true; 
        }
        return false;   
    }

    // Check current money
    public int GetMoney()
    {
        return money;
    }
}
