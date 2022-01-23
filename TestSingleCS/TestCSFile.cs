using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleCS.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSingleCS
{
    [TestClass]
    public class TestCSFile
    {
        private void AssertHead(string content, string expected)
        {
            using (var temp = new TempFile())
            {
                File.WriteAllText(temp.Path, content);
                var file = new CSFile(temp.Path);

                Assert.AreEqual(expected.Trim('\n'), file.Head.Trim('\n'));
            }
        }


        [TestMethod]
        public void Usings_CommentedUsing_ReturnsUsings()
        {
            AssertHead(@"
using System;
//using Simple.Models;
// using System.Linq;

namespace Simple
{
    public class MyClass;
    {
        
    }
}
", @"
using System;
//using Simple.Models;
// using System.Linq;"
);
        }


        [TestMethod]
        public void Usings_CommentBetween_ReturnsUsings()
        {

            AssertHead(@"
using System;
// This is a comment
using Simple.Models;
using System.Linq;

namespace Simple
{
    public class MyClass;
    {
        
    }
}
", @"
using System;
// This is a comment
using Simple.Models;
using System.Linq;"
);
        }


        [TestMethod]
        public void Usings_SimpleFile_ReturnsUsings()
        {
            AssertHead(@"
using System;
using Simple.Models;
using System.Linq;

namespace Simple
{
    public class MyClass;
    {
        
    }
}
",
@"
using System;
using Simple.Models;
using System.Linq;");
        }


    }
}
