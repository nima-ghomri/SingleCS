using System.Collections.Generic;
using System.Text;

namespace SingleCS.Models
{
    public interface ICSCombiner
    {
        ICSFile Combine(CombineOptions options, params ICSFile[] files);
    }
}
