using System.Collections.Generic;

namespace SingleCS.Models
{
    /// <summary>
    /// C# Source file interface. for feature support of .NET 6.0 or above
    /// </summary>
    public interface ICSFile
    {
        string Body { get; }
        string Head { get; }
        string Path { get; }
    }
}