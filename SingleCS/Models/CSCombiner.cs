using System;
using System.Collections.Generic;
using System.Text;

namespace SingleCS.Models
{
    public class CSCombiner : ICSCombiner
    {
        public ICSFile Combine(CombineOptions options, params ICSFile[] files)
        {
            throw new Exception();
        }
    }
}
