using System;

[AttributeUsage(AttributeTargets.Class)]
public class PluginInfoAttribute : Attribute
{
    public string Description { get; }
    public string Provider { get; }

    public PluginInfoAttribute(string description)
    {
        Provider = "Raton"; // DON'T CHANGE
        Description = description;
    }
}