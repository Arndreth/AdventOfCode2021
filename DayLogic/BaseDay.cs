using System;
using System.Diagnostics;
using System.IO;

namespace AoC2021.DayLogic
{
  
    public abstract class BaseDay
    {
        internal string[] GetInputFromFile()
        {
            return File.ReadAllLines($"Inputs/{this.GetType().Name}.txt");
        }

        internal void Log(string message)
        {
            StackTrace stackTrace = new();
            var methodName = stackTrace.GetFrame(stackTrace.FrameCount - 2)?.GetMethod()?.Name;
            Console.WriteLine($"[{this.GetType().Name}.{methodName}] {message}");
        }
    }
}