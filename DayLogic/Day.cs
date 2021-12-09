using System;
using System.Diagnostics;
using System.IO;

namespace AoC2021.DayLogic
{
  
    public abstract class Day
    {
        private string[] m_fileInput;
        

        public abstract void PartOne();

        public abstract void PartTwo();
        internal string[] GetInputFromFile(bool exampleData = false)
        {
            return File.ReadAllLines(exampleData ? $"Inputs/ExampleInputs/{this.GetType().Name}Example.txt" : $"Inputs/{this.GetType().Name}.txt");
        }

        internal void Log(string message)
        {
            StackTrace stackTrace = new();
            var methodName = stackTrace.GetFrame(stackTrace.FrameCount - 2)?.GetMethod()?.Name;
            string msg = $"[{this.GetType().Name}.{methodName}] {message}";
            if (msg.StartsWith('\r'))
            {
                Console.Write(msg);
                return;
            }
            Console.WriteLine(msg);
        }

        internal void LogFormat(string message, params object[] msgParams)
        {
            StackTrace stackTrace = new();
            var methodName = stackTrace.GetFrame(stackTrace.FrameCount - 2)?.GetMethod()?.Name;
            bool rewriteLine = message.StartsWith('\r');
            string msg = $"[{this.GetType().Name}.{methodName}] {String.Format(message.TrimStart('\r'), msgParams)}";
            if (rewriteLine)
            {
                Console.Write($"\r{msg}");
                return;
            }
            Console.WriteLine(msg);

        }
    }
}