# Plugin Development
> Build powerful plugins for Raton in minutes. · `C# 7.3 on NET 4.7.2`

---

## Welcome

Welcome to the Raton Plugin Guide. This guide walks you through everything needed to create, test, and publish plugins that extend Raton's functionality. Plugins are compiled C# assemblies loaded at runtime, also, they can receive arguments from the UI and communicate back through a structured packet system :)

**Quick start**

1. Create a new .NET Class Library project
2. Use the test plugin source to guide yourself
3. Decorate your class with `[PluginInfo]` and implement `Main`
4. Compile, drop the `.dll` into Raton's `/plugins` folder, done

---

## 01 — Project structure

Your plugin is a standard .NET Class Library project. Raton discovers it as a compiled DLL. Minimum layout:

```
MyPlugin/
├── MyPlugin.csproj
├── Main.cs
└── Networking/
    └── Default classes
```

> **Tip:** Set output type to Class Library in your `.csproj` so the compiler emits a `.dll` instead of an `.exe`

---

## 02 — [PluginInfo] Attribute

Every plugin class must be decorated with `[PluginInfo]`. Raton reads this to register your plugin in the UI and display a description to the user.

```csharp
using System;
[AttributeUsage(AttributeTargets.Class)]
public class PluginInfoAttribute : Attribute
{
    public string Description { get; }
    public string Provider    { get; }

    public PluginInfoAttribute(string description)
    {
        Provider    = "Raton";
        Description = description;
    }
}
```

> ⚠️ **Warning:** Never change `Provider = "Raton"`. Raton uses this field to verify the plugin origin.

Usage on your own class:

```csharp
[PluginInfo("Example plugin for testing")]
class Program
{
    // ...
}
```

---

## 03 — Entry point: Main

Raton calls a static `Main` method on your class. The signature is different — Raton injects two parameters:

| Parameter | Description |
|---|---|
| `Action<byte[]> send` | Delegate to send a serialized Pack back to the client. Store it in a static field. |
| `string[] args` | Arguments the user typed in the plugin prompt. May be null if invoked with no arguments. |

```csharp
public static Action<byte[]> _send;

static void Main(Action<byte[]> send, string[] args)
{
    _send = send;
    if (args == null || args.Length == 0)
    {
        ExecuteArg("default", "");
        return;
    }
    ParseArgs(args);
}
```

---

## 04 — Pack & Send

Communication back to the client is done through the `Pack` class (from `Stuff.dll`). Fill key-value pairs, serialize with `.Pacc()`, and pass the resulting `byte[]` to `_send`.

```csharp
Pack pack = new Pack();
pack.SetString("Packet",  "UserMessage");
pack.SetString("Message", "Hello from my plugin!");
pack.SetString("Status",  "Success");
_send(pack.Pacc());
```

**Available Status values:**

| Value | Logs information |
|---|---|
| `"Information"` | Sky blue log entry — Send information or current status |
| `"Success"` | Green log entry — operation completed successfully |
| `"Warning"` | Yellow log entry — non-critical notice |
| `"Error"` | Red log entry — something went wrong |

---

## 05 — Parsing arguments

Arguments follow a `-flag value` convention. Iterate the args array, detect tokens starting with `-` as flag names, and accumulate subsequent tokens as the value. Use a `StringBuilder` for multi-word values.

```csharp
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
            if (valueBuilder.Length > 0) valueBuilder.Append(" ");
            valueBuilder.Append(arg);
        }
    }
    if (currentArg != null)
        ExecuteArg(currentArg, valueBuilder.ToString());
}
```

> **Tip:** Always use the `"default"` case in your switch. When invoked with no args, use it to print a usage hint.

---

## 06 — Pack method reference

| Method | Description |
|---|---|
| `SetString(key, value)` | Store a string value under key |
| `SetInt(key, value)` | Store an integer value under key |
| `SetBytes(key, value)` | Store a byte array under key (e.g. file data) |
| `Pacc()` | Serialize the Pack to `byte[]` — pass result to `_send()` |

---

## 07 — Full example

The complete example plugin. Responds to `-msg <text>` and prints a usage hint when called without arguments.

```csharp
using Stuff;
using System;
using System.Text;

namespace TestPlugin
{
    [PluginInfo("Example plugin for testing")]
    class Program
    {
        public static Action<byte[]> _send;

        static void Main(Action<byte[]> send, string[] args)
        {
            _send = send;
            if (args == null || args.Length == 0)
            {
                ExecuteArg("default", "");
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
                    if (valueBuilder.Length > 0) valueBuilder.Append(" ");
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
                case "-msg":
                    Pack pack = new Pack();
                    pack.SetString("Packet",  "UserMessage");
                    pack.SetString("Message", "Your message was: " + value);
                    pack.SetString("Status",  "Success");
                    _send(pack.Pacc());
                    break;
                default:
                    Pack pack2 = new Pack();
                    pack2.SetString("Packet",  "UserMessage");
                    pack2.SetString("Message", "Try: -msg <text>");
                    pack2.SetString("Status",  "Warning");
                    _send(pack2.Pacc());
                    break;
            }
        }
    }
}
```

---

## 08 — Tips & tricks

**Always guard args for null**
Raton may call your plugin with a null array when no input is given. Check `args == null` before everything.

**Store _send as static**
The delegate is injected once in `Main`. Store it as a static field so all your methods can call it without passing it around.

**Use Status semantically**
`Success`, `Warning`, and `Error` map to colored log entries. Don't mark everything as Success.

**Keep plugins focused**
One plugin = one purpose. Small, composable plugins are easier to debug and update. Or whatever.

**Keep it safe**
Be carefully when executing code from other people, this may cause RCE attempts or client stealing.
