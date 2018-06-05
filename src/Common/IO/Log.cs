using System;
using System.Net;
using System.Text;
using System.Diagnostics;

namespace Destiny.IO
{
    public static class Log
    {
        private const byte LabelWidth = 10;
        private static bool Entitled;

        public static string Margin
        {
            get
            {
                return new string(' ', LabelWidth);
            }
        }

        public static string MaskString(string input, char mask = '*')
        {
            return new string(mask, input.Length);
        }

        public static void WriteItem(string label, ConsoleColor labelColor, string value, params object[] args)
        {
            lock (typeof(Log))
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(' ', LabelWidth - label.Length - 3);
                sb.Append("[");
                sb.Append(label);
                sb.Append("]");
                sb.Append(" ");

                label = sb.ToString();
                value = string.Format(value, args);

                Console.ForegroundColor = labelColor;
                Console.Write(label);
                Console.ForegroundColor = ConsoleColor.Gray;

                bool first = true;

                foreach (string s in value.Split('\n'))
                {
                    string[] lines = new string[(int) Math.Ceiling((float)s.Length / (float)(Console.BufferWidth - LabelWidth))];

                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i == lines.Length - 1)
                        {
                            lines[i] = s.Substring((Console.BufferWidth - LabelWidth) * i);
                        }
                        else
                        {
                            lines[i] = s.Substring((Console.BufferWidth - LabelWidth) * i, Console.BufferWidth - LabelWidth);
                        }
                    }

                    foreach (string line in lines)
                    {
                        if (!first)
                        {
                            Console.Write(Margin);
                        }

                        if (line.Length + LabelWidth < Console.BufferWidth)
                        {
                            Console.WriteLine(line);
                        }
                        else if (line.Length + LabelWidth == Console.BufferWidth)
                        {
                            Console.Write(line);
                        }

                        first = false;
                    }
                }
            }
        }

        public static void SkipLine()
        {
            Console.WriteLine();
        }

        public static void Entitle(string value, params object[] args)
        {
            lock (typeof(Log))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine((Entitled ? "\n\n" : "") + string.Format(value, args) + '\n');
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Title = string.Format(value, args);

                Entitled = true;
            }

            WriteToFile(value, args);
        }

        public static string Input(string label)
        {
            lock (typeof(Log))
            {
                WriteItem("Input", ConsoleColor.Cyan, string.Empty);
                Console.Write(label);
                return Console.ReadLine();
            }
        }

        public static string Input(string label, string defaultValue)
        {
            lock (typeof(Log))
            {
                WriteItem("Input", ConsoleColor.Cyan, string.Empty);
                Console.Write(label);
                string result = Console.ReadLine();

                if (result == string.Empty)
                {
                    result = defaultValue;
                    Console.CursorTop--;
                    Console.CursorLeft = Margin.Length + label.Length;

                    Console.WriteLine(defaultValue == string.Empty ? "(None)" : result);
                }

                return result;
            }
        }

        public static IPAddress Input(string label, IPAddress defaultValue)
        {
            IPAddress result;

        input:
            try
            {
                result = IPAddress.Parse(Input(label, defaultValue.ToString()));
            }
            catch
            {
                goto input;
            }

            return result;
        }

        public static int Input(string label, int defaultValue)
        {
            int result;

        input:
            try
            {
                result = int.Parse(Input(label, defaultValue.ToString()));
            }
            catch
            {
                goto input;
            }

            return result;
        }

        public static bool YesNo(string label)
        {
            lock (typeof(Log))
            {
                string result;

                do
                {
                    WriteItem("Yes/No", ConsoleColor.Cyan, string.Empty);
                    Console.Write(label);
                    result = Console.ReadLine().ToLower();
                }
                while (!(result == "yes" || result == "no" || result == "y" || result == "n"));

                return (result == "yes" || result == "y");
            }
        }

        public static bool YesNo(string label, bool defaultValue)
        {
            lock (typeof(Log))
            {
                string result;

                do
                {
                    WriteItem("Yes/No", ConsoleColor.Cyan, string.Empty);
                    Console.Write(label);
                    result = Console.ReadLine().ToLower();

                    if (result == string.Empty)
                    {
                        result = defaultValue ? "yes" : "no";
                        Console.CursorTop--;
                        Console.CursorLeft = Margin.Length + label.Length;
                        Console.WriteLine(defaultValue ? "Yes" : "No");
                    }
                }
                while (!(result == "yes" || result == "no" || result == "y" || result == "n"));

                return (result == "yes" || result == "y");
            }
        }

        public static void Inform(string value, params object[] args)
        {
            WriteItem("Info", ConsoleColor.White, value, args);
        }

        public static void Inform(object value)
        {
            Inform(value.ToString());
        }

        // TODO: Cross-process lock/wait for file writability access. (IMPORTANT! else server may crash!)

        private static void WriteToFile(string value, params object[] args)
        {
            /*lock (typeof(Log))
            {
                using (TextWriter fileWriter = new StreamWriter(Application.ExecutablePath + "Error.log", true))
                {
                    fileWriter.WriteLine(DateTime.Now.ToString() + " " + string.Format(value, args) + "\r\n");
                }
            }*/
        }

        public static void Warn(string value, params object[] args)
        {
            WriteItem("Warning", ConsoleColor.Yellow, value, args);
            WriteToFile("Warning: " + value, args);
        }

        public static void Warn(object value)
        {
            Warn(value.ToString());
        }

        public static void Error(string value, params object[] args)
        {
            WriteItem("Error", ConsoleColor.Red, value, args);
            WriteToFile(value, args);
        }

        public static bool ShowStackTrace { get; set; }

        public static void Error(Exception exception)
        {
            WriteItem("Error", ConsoleColor.Red, ShowStackTrace ? exception.ToString() : exception.Message);
            WriteToFile(exception.ToString());
        }

        public static void Error(string label, Exception exception, params object[] args)
        {
            try
            {
                WriteItem("Error", ConsoleColor.Red, "{0}\n{1}", string.Format(label, args), ShowStackTrace ? exception.ToString() : exception.Message);

            }

            catch (FormatException ex)
            {
                SkipLine();
                Tracer.TraceErrorMessage(ex, "Incorrect string format! or out of bounds!");
                SkipLine();
            }

            WriteToFile(exception.ToString());
        }

        public static void Success(string value, params object[] args)
        {
            WriteItem("Success", ConsoleColor.Green, value, args);
        }

        public static void Success(object value)
        {
            Success(value.ToString());
        }

        public static void Sql(string value, params object[] args)
        {
            WriteItem("Sql", ConsoleColor.Magenta, value, args);
        }

        public static void Hex(string label, byte[] value, params object[] args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format(label, args));
            sb.Append('\n');

            if (value == null || value.Length == 0)
            {
                sb.Append("(Empty)");
            }
            else
            {
                int lineSeparation = 0;

                foreach (byte b in value)
                {
                    if (lineSeparation == 16)
                    {
                        sb.Append('\n');
                        lineSeparation = 0;
                    }

                    sb.AppendFormat("{0:X2} ", b);
                    lineSeparation++;
                }
            }

            WriteItem("Hex", ConsoleColor.Magenta, sb.ToString());
        }

        public static void Hex(string label, byte b, params object[] args)
        {
            Hex(label, new [] { b }, args);
        }

        public static LoadingIndicator Load(string header)
        {
            return new LoadingIndicator(header);
        }
    }

    public class LoadingIndicator : IDisposable
    {
        public static bool ShowTime { get; set; }

        private readonly Stopwatch mWatch;

        internal LoadingIndicator(string header)
        {
            mWatch = new Stopwatch();

            mWatch.Start();

            lock (typeof(Log))
            {
                Console.Write("{0}  {1}... ", Log.Margin, header);
            }
        }

        public void Dispose()
        {
            lock (typeof(Log))
            {
                mWatch.Stop();

                if (ShowTime)
                {
                    Console.WriteLine("({0}ms)", mWatch.ElapsedMilliseconds);
                }

                else
                {
                    Console.WriteLine();
                }
            }
        }
    }
}