// See https://aka.ms/new-console-template for more information

using Microsoft.Win32.SafeHandles;

const decimal KwhHourCost = 0.11m;
//const decimal LargePackage = 1.15m;
//const decimal MediumPackage = 1;
//const int FreezeDryerWattsPerHour = 1210;
const decimal FreezeDryerKiloWattHour = 1.21m;
const decimal FreezeDryerKiloWattHourCost = FreezeDryerKiloWattHour * KwhHourCost;

string menuData = "menucosttime.txt";
string[] menuCostTime = File.ReadAllLines(menuData);
Console.WriteLine("which item from the menu are you freeze drying?");

Packaging();

static (string, int, decimal) SelectMenuItem(string[] menuCostTime)
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
    int time = int.Parse(selectedItemParts[1].Replace("m","").Trim());
    decimal cost = decimal.Parse(selectedItemParts[2].Trim());

    return (itemName, time, cost);
}

static (string,decimal) Packaging()
{
    Console.WriteLine("Select Packaging");
    Console.WriteLine("1. Mylar Medium");
    Console.WriteLine("2. Mylar Large");

    string? choice = Console.ReadLine();

    return choice switch
    {
        "1" => ("Mylar Medium",1.50m),
        "2" => ("Mylar Large",2.15m),
        _ => ("invalid", 0m)
        
    };
}

static decimal electricityCost(decimal kwhCost, int hours)
{
    return FreezeDryerKiloWattHourCost * hours;
}