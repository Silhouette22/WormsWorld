using System;
using System.IO;

namespace ConsoleApp
{
    public class ReportWriter : IReportWriter
    {
        private readonly TextWriter _textWriter;
        public ReportWriter(TextWriter textWriter) => _textWriter = textWriter;
        public void WriteReportConsole(int iterationNumber, string text)
        {
            Console.WriteLine($"Turn №{iterationNumber}:" + text);
        }
        public void WriteReport(int iterationNumber, string text)
        {
            _textWriter.WriteLine($"Turn №{iterationNumber}:" + text);
        }

        public void Dispose()
        {
            _textWriter.Dispose();
        }
    }

    public interface IReportWriter : IDisposable
    {
        public void WriteReport(int iterationNumber, string text);

        public void WriteReportConsole(int iterationNumber, string text);
    }
}