using Stuff;
using System;
using System.Text;

namespace TestPlugin
{
    [PluginInfo("Example plugin for testing")] // Your description
    class Program
    {
        public static Action<byte[]> _send; // Send to server

        static void Main(Action<byte[]> send, string[] args)
        {
            _send = send;
            if (args == null || args.Length == 0)
            {
                ExecuteArg("default", ""); // Handle empty args
                return;
            }
            ParseArgs(args);
        }

        static void ParseArgs(string[] args)
        {
            string currentArg = null;
            StringBuilder valueBuilder = new StringBuilder();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                if (arg.StartsWith("-"))
                {
                    if (currentArg != null)
                        ExecuteArg(currentArg, valueBuilder.ToString());
                    currentArg = arg.ToLower();
                    valueBuilder.Clear();
                }
                else
                {
                    if (valueBuilder.Length > 0)
                        valueBuilder.Append(" ");
                    valueBuilder.Append(arg);
                }
            }

            if (currentArg != null)
                ExecuteArg(currentArg, valueBuilder.ToString());
        }

        static void ExecuteArg(string arg, string value)
        {
            Pack pack = new Pack();
            try
            {
                switch (arg)
                {
                    case "-msg":
                        pack.SetString("Packet", "UserMessage");
                        pack.SetString("Message", "Your message was: " + value);
                        pack.SetString("Status", "Success");
                        break;
                    default:
                        pack.SetString("Packet", "UserMessage");
                        pack.SetString("Message", "Executed without arguments (Try using -msg <text>)");
                        pack.SetString("Status", "Warning");
                        break;
                }
            }
            catch (Exception ex)
            {
                pack = new Pack();
                pack.SetString("Packet", "UserMessage");
                pack.SetString("Message", "Plugin error: " + ex.Message);
                pack.SetString("Status", "Error");
            }
            _send(pack.Pacc());
        }
    }
}