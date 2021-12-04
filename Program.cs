// See https://aka.ms/new-console-template for more information

using System;
using System.IO;
using AoC2021.DayLogic;

Console.WriteLine("*** AoC 2021 ***");

Console.WriteLine("Which Day are you processing? ");
string? day = Console.ReadLine();

if (!string.IsNullOrEmpty(day))
{
    int dayNumber = int.Parse(day);

    IDay dayLogic = GetDay(dayNumber);
    
    dayLogic.PartOne();
    dayLogic.PartTwo();
}

IDay GetDay(int dayNumber)
{
    switch (dayNumber)
    {
        case 1: return new Day1();
        case 2: return new Day2();
        case 3: return new Day3();
        default: throw new Exception("No day found for {dayNumber}");
    }
}