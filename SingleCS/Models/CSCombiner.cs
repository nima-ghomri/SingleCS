using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SingleCS.Models
{
    public class CSCombiner : ICSCombiner
    {
        public ICSFile Combine(CombineOptions options, params ICSFile[] files)
        {
            var head = string.Join("",files.Select(f => f.Head));
            var body = string.Join("",files.Select(f => f.Body));

            if (options.HasFlag(CombineOptions.RemoveDuplicates))
                head = RemoveDuplicates(head);

            return new CSFile(head, body);
        }

        private string RemoveDuplicates(string head)
        {
            throw new NotImplementedException();
        }
    }
}
