using System;
using System.IO;

namespace ConsoleApp
{
    public static class ReportWriter
    {
        public static void WriteReport(TextWriter writer, int iterationNumber, string text)
        {
            writer.WriteLine($"Turn №{iterationNumber}:" + text);
        }
        public static void WriteReport(int iterationNumber, string text)
        {
            Console.WriteLine($"Turn №{iterationNumber}:" + text);
        }
    }
}