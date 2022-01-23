using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCS.Models
{
    public interface ICSCombiner
    {
        ICSFile Combine(ICSFile file1, ICSFile file2);
    }
}
