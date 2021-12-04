// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using AoC2021.DayLogic;

Console.WriteLine("*** AoC 2021 ***");

Console.Write("Which Day are you processing? ");
string? day = Console.ReadLine();

if (!string.IsNullOrEmpty(day))
{
    int dayNumber = int.Parse(day);

    Day? dayLogic = GetDay(dayNumber);
    
    dayLogic?.PartOne();
    dayLogic?.PartTwo();
}

Day? GetDay(int dayNumber)
{
    var dayQualifier = Type.GetType($"AoC2021.DayLogic.Day{dayNumber}");
    if (dayQualifier == null)
    {
        throw new Exception(
            $"[AOC-Exception] Cannot create instance of type `Day{dayNumber}` as it hasn't been defined");
    }
    return (Day)Activator.CreateInstance(dayQualifier)!;
}