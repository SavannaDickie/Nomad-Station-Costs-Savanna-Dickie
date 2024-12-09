// See https://aka.ms/new-console-template for more information

using Microsoft.Win32.SafeHandles;

const decimal KwhHourCost = 0.11m;
const decimal FreezeDryerKiloWattHour = 1.21m;
const decimal FreezeDryerKiloWattHourCost = FreezeDryerKiloWattHour * KwhHourCost;

string menuData = "menucosttime.txt";
string[] menuCostTime = File.ReadAllLines(menuData);

Console.WriteLine("This is a program that will calculate production costs for\nfreeze drying and packaging the menu items");

(string selectedItem, decimal cost, int time) = SelectMenuItem(menuCostTime);
(string packagingType, decimal packagingCost) = Packaging();
//Packaging();
decimal totalEnergyCost = electricityCost(FreezeDryerKiloWattHourCost, time);
decimal totalCost = cost + packagingCost + totalEnergyCost;

Console.Clear();
Console.WriteLine("\nFor your selection:");
Console.WriteLine($"\nFreeze dry the {selectedItem} for {time} hours");
Console.WriteLine($"Total Cost: ${totalCost:F2}");

//decimal profit = myProfit(sellingCost, totalCost);
decimal sellingCost = CalculateSellingCost(totalCost);
Console.WriteLine($"Selling Cost (20% profit): ${sellingCost:F2}");

decimal profit = myProfit(sellingCost, totalCost);
Console.WriteLine($"Profit: ${profit:F2}");

Console.Write("\nSave order? Y/N: ");
string saveChoice = Console.ReadLine()?.ToLower();

if (saveChoice == "y")
{
    SaveOrder(selectedItem, packagingType, packagingCost, totalEnergyCost, totalCost, sellingCost, profit);
}
else
{
    Console.WriteLine("This order will not be saved");
}


static (string, decimal, int) SelectMenuItem(string[] menuCostTime)
{
    Console.WriteLine("\nMenu:");
    for (int i = 0; i < menuCostTime.Length; i++)
    {
        string[] parts = menuCostTime[i].Split(',');
        string menuName = parts[0].Trim();
        Console.WriteLine($"{i + 1}. {menuName}");
    }
    
    Console.Write("\nSelect an item by number: ");
    int choice = int.Parse(Console.ReadLine() ?? "0");

    if (choice < 1 || choice > menuCostTime.Length)
    {
        Console.WriteLine("Invalid selection.");
    }

    string[] selectedItemParts = menuCostTime[choice - 1].Split(',');
    string itemName = selectedItemParts[0].Trim();
    int time = int.Parse(selectedItemParts[2].Replace("m","").Trim());
    decimal cost = decimal.Parse(selectedItemParts[1].Trim());

    return (itemName, cost, time);
}

static (string,decimal) Packaging()
{
    Console.Clear();
    Console.WriteLine("Select Packaging");
    Console.WriteLine("1. Mylar Medium");
    Console.WriteLine("2. Mylar Large");

    Console.Write("select an item by number: ");
    string? choice = Console.ReadLine();

    return choice switch
    {
        "1" => ("Mylar Medium",1.50m),
        "2" => ("Mylar Large",2.15m),
        _ => ("invalid", 0m)
        
    };
}

static decimal electricityCost(decimal FreezeDryerKiloWattHourCost, int hours)
{
    //return FreezeDryerKiloWattHourCost * hours;
    return FreezeDryerKiloWattHourCost * hours;
}

static void SaveOrder(string item, string packaging, decimal packagingCost, decimal energyCost, decimal totalCost, decimal sellingCost, decimal profit)
{
    string order = $"{item}, {packaging}, Packaging Cost: ${packagingCost:F2}, Energy Cost: ${energyCost:F2}, Total Cost: ${totalCost:F2}, Selling Cost: ${sellingCost:F2}, Profit: {profit:F2}";
    string purchaseHistory = "purchasehistory.txt";

    List<string> orders = new();
    if(File.Exists(purchaseHistory))
    {
        orders.AddRange(File.ReadAllLines(purchaseHistory));
    }
    orders.Add(order);
    
    File.WriteAllLines(purchaseHistory, orders);
    Console.WriteLine($"Order saved to {purchaseHistory}");
}

static decimal CalculateSellingCost(decimal totalCost)
{
    return totalCost * 1.20m;
}

static decimal myProfit(decimal sellingCost, decimal totalCost)
{
    return sellingCost - totalCost;
}


