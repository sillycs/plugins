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
            switch (arg)
            {
                case "-msg": // Argument example: -msg Hello there!
                    Pack pack = new Pack();
                    pack.SetString("Packet", "UserMessage");
                    pack.SetString("Message", "Your message was: " + value);
                    pack.SetString("Status", "Success");
                    _send(pack.Pacc());
                    break;

                default: // No arguments
                    Pack pack2 = new Pack();
                    pack2.SetString("Packet", "UserMessage");
                    pack2.SetString("Message", "Executed without arguments (Try using -msg <text>)");
                    pack2.SetString("Status", "Warning");
                    _send(pack2.Pacc());
                    break;
            }
        }
    }
}