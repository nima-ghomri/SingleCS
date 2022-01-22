using System.Collections.Generic;

namespace SingleCS.Models
{
    public interface ICSFile
    {
        string Body { get; }
        List<string> Usings { get; }
    }
}