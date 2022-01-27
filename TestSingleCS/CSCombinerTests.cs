using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleCS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestSingleCS
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class CSCombinerTests
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

        public ICSFile File1 => GetTemp(@"
using Name.Space1;
using Name.Space1;
using 
Name.Space3;

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

        public ICSFile File2 => GetTemp(@"
//using Name.Space4;
using Name.Space5;
using Name.Space3;
using Name.Space3;

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

        private string expected = @"
using Name.Space1;
using Name.Space1;
using 
Name.Space3;
//using Name.Space4;
using Name.Space5;
using Name.Space3;
using Name.Space3;

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

        [TestMethod]
        public void Combine_TwoFiles_ReturnCombinedFile()
        {
            var combiner = new CSCombiner();
            var result = combiner.Combine(CombineOptions.None, File1, File2);
            Assert.AreEqual($"{result.Head}{result.Body}", expected);
        }

        [TestMethod]
        public void RemoveDuplicates_TwoFiles_ReturnCombinedFile()
        {
            var combiner = new CSCombiner();
            var result = combiner.Combine(CombineOptions.Refactor, File1, File2);
            Assert.AreEqual($"{result.Head}{result.Body}", expected);
        }
    }
}
