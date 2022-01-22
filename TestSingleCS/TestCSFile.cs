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
        private void Usings_Test(string fileContent, string[] expectedUsings)
        {

            using (var temp = new TempFile())
            {
                File.WriteAllText(temp.Path, fileContent);
                var file = new CSFile(temp.Path);

                CollectionAssert.AreEqual(expectedUsings, file.Usings);
            }
        }


        [TestMethod]
        public void Usings_CommentedUsing_ReturnsUsings()
        {
            Usings_Test(@"
using System;
//using Simple.Models;
// using System.Linq;

namespace Simple
{
    public class MyClass;
    {
        
    }
}
", new[]
{
"using System;",
});
        }


        [TestMethod]
        public void Usings_CommentBetween_ReturnsUsings()
        {

            Usings_Test(@"
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
", new[]
{
"using System;",
"using Simple.Models;",
"using System.Linq;",
});
        }


        [TestMethod]
        public void Usings_SimpleFile_ReturnsUsings()
        {
            Usings_Test(@"
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
new[]
{
"using System;",
"using Simple.Models;",
"using System.Linq;",
});
        }


    }
}
