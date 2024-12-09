// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

const decimal KwhHourCost = 0.11m;
const decimal FreezeDryerKiloWattHour = 1.21m;
const decimal FreezeDryerKiloWattHourCost = FreezeDryerKiloWattHour * KwhHourCost;

string menuData = "menucosttime.txt";
string[] menuCostTime = File.ReadAllLines(menuData);

Console.WriteLine("This is a program that will calculate production costs for\nfreeze drying and packaging the menu items");

(string selectedItem, decimal cost, int time) = SelectMenuItem(menuCostTime);
(string packagingType, decimal packagingCost) = Packaging();

decimal totalEnergyCost = electricityCost(FreezeDryerKiloWattHourCost, time);
decimal totalCost = cost + packagingCost + totalEnergyCost;

Console.Clear();
Console.WriteLine("\nFor your selection:");
Console.WriteLine($"\nFreeze dry the {selectedItem} for {time} hours");
Console.WriteLine($"Total Cost: ${totalCost:F2}");

decimal sellingCost = CalculateSellingCost(totalCost);
Console.WriteLine($"Selling Cost (20% profit): ${sellingCost:F2}");

decimal profit = myProfit(sellingCost, totalCost);
Console.WriteLine($"Profit: ${profit:F2}");


string saveChoice;
while(true)

{
    //Console.Clear();
    Console.Write("\nSave order? Y/N:");
    saveChoice = Console.ReadLine()?.ToLower();
    if (saveChoice == "y")
    {
        SaveOrder(selectedItem, packagingType, packagingCost, totalEnergyCost, totalCost, sellingCost, profit);
        break;
    }
    else if (saveChoice == "n")
    {
        Console.WriteLine("This order will not be saved");
        break;
    }
    else
    {
        Console.Clear();
        Console.WriteLine("\nFor your selection:");
        Console.WriteLine($"\nFreeze dry the {selectedItem} for {time} hours");
        Console.WriteLine($"Total Cost: ${totalCost:F2}");
        Console.WriteLine($"Selling Cost (20% profit): ${sellingCost:F2}");
        Console.WriteLine($"Profit: ${profit:F2}");
        
        continue;
    }

}


static (string, decimal, int) SelectMenuItem(string[] menuCostTime)
{
    while(true)
    {
    Console.WriteLine("\nMenu:");
    for (int i = 0; i < menuCostTime.Length; i++)
    {
        string[] parts = menuCostTime[i].Split(',');
        string menuName = parts[0].Trim();
        Console.WriteLine($"{i + 1}. {menuName}");
    }
    
    Console.Write("\nSelect an item by number: ");
    //int choice = int.Parse(Console.ReadLine() ?? "0");
    if(int.TryParse(Console.ReadLine(),out int choice) && choice >= 1 && choice <= menuCostTime.Length)
    {

    string[] selectedItemParts = menuCostTime[choice - 1].Split(',');
    string itemName = selectedItemParts[0].Trim();
    int time = int.Parse(selectedItemParts[2].Trim());
    decimal cost = decimal.Parse(selectedItemParts[1].Trim());

    Debug.Assert(choice >= 1 && choice <= menuCostTime.Length, "ERROR WITH: invaid selection");
    return(itemName,cost, time);
    }
  
}
}

static (string,decimal) Packaging()
{
while(true)
{
    Console.Clear();
    Console.WriteLine("Select Packaging");
    Console.WriteLine("1. Mylar Medium");
    Console.WriteLine("2. Mylar Large");
    Console.WriteLine("3. No Packaging");

    Console.Write("select an item by number: ");
    string? choice = Console.ReadLine();

    switch (choice)
    {
        case "1": return ("Mylar Medium",1.50m);
        case "2": return ("Mylar Large",2.15m);
        case "3": return ("No packaging", 0m);
        default: break;
        
    };
}
}

static decimal electricityCost(decimal FreezeDryerKiloWattHourCost, int hours)
{
   
    decimal energyCost = FreezeDryerKiloWattHourCost * hours;
    Debug.Assert(energyCost >= 0, "Energy cost can not be nagative");
    return energyCost;
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
    string[] savedOrders = File.ReadAllLines(purchaseHistory);
    Console.WriteLine($"Order saved to {purchaseHistory}");

    Debug.Assert(savedOrders.Contains(order), "ERROR: order not saved correctly");
}

static decimal CalculateSellingCost(decimal totalCost)
{
    
    decimal sellingCost = totalCost * 1.2m;
    Debug.Assert(sellingCost>totalCost,"total cost less than selling cost");
    return sellingCost;
}

static decimal myProfit(decimal sellingCost, decimal totalCost)
{
    return sellingCost - totalCost;
}


