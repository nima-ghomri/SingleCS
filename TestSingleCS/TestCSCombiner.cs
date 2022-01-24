using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleCS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestSingleCS
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class TestCSCombiner
    {
        private ICSFile GetTemp(string content)
        {
            ICSFile result;
            using (var temp = new TempFile())
            {
                File.WriteAllText(temp.Path, content);
                result = new CSFile(temp.Path);
            }
            return result;
        }

        [TestMethod]
        public void Combine_TwoFiles_ReturnCombinedFile()
        {
            var file1 = GetTemp(@"
using Name.Space10;
using Name.Space11;
using 
Name.Space12;

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
");

            var file2 = GetTemp(@"
//using Name.Space20;
using Name.Space21;
using Name.Space22;
using Name.Space23;

namespace Complex.File
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
");
            string expected = @"
using Name.Space10;
using Name.Space11;
using 
Name.Space12;
//using Name.Space20;
using Name.Space21;
using Name.Space22;
using Name.Space23;

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

namespace Complex.File
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

            var combiner = new CSCombiner();
            var result = combiner.Combine(file1, file2);
            Assert.AreEqual($"{result.Head}{result.Body}", expected);
        }
    }
}
