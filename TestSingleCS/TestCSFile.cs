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
        public void Body_FileWithoutNamespace_ReturnsBody()
        {
            var content = @"
using System;
using System.Linq;
using 
Simple.Models;

    public class MyClass;
    {
//using this method to test regex
        private void Method()
		  {
			 using(var stream = new StringReader())
			 {
			 }
		  }
    }
";

            var expected = @"

    public class MyClass;
    {
//using this method to test regex
        private void Method()
		  {
			 using(var stream = new StringReader())
			 {
			 }
		  }
    }
";
            using (var temp = new TempFile())
            {
                File.WriteAllText(temp.Path, content);
                var file = new CSFile(temp.Path);

                Assert.AreEqual(file.Body, expected);
            }
        }

        [TestMethod]
        public void CSFile_ComplexFile_BothMatchOriginal()
        {
            var content = @"
using System;
//using System.Linq;
using 
Simple.Models;
using System.Linq;
using System.Linq;using System.Linq;using System.Linq;
//using System.Linq;
//  using System.Linq;

namespace Simple
{
    public class MyClass;
    {
//using this method to test regex
        private void Method()
		  {
			 using(var stream = new StringReader())
			 {
			 }
		  }
    }
}
";
            using (var temp = new TempFile())
            {
                File.WriteAllText(temp.Path, content);
                var file = new CSFile(temp.Path);

                Assert.AreEqual($"{file.Head}{file.Body}", content);
            }
        }

        [TestMethod]
        public void Head_CommentedUsing_ReturnsHead()
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
        public void Head_CommentBetween_ReturnsHead()
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
        public void Head_SimpleFile_ReturnsHead()
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
