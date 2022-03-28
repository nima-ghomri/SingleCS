using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleCS;
using SingleCS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestSingleCS
{
    [TestClass]
    public class OptionsTests
    {
        //private ICSFile GetTemp(string content)
        //{
        //    ICSFile result;
        //    using (var temp = new TempFile())
        //    {
        //        File.WriteAllText(temp.Path, content);
        //        result = new CSFile(temp.Path);
        //    }
        //    return result;
        //}

        private static TempFile GetTemp(string content)
        {
            var result = new TempFile();
            File.WriteAllText(result.Path, content);
            return result;
        }
        private void RunMain(string arguments)
        {
            var args = arguments.Split(' ', ',');
            Program.Main(args);
        }

        private TempFile file1 = GetTemp(
            @"using MyUsing;
using MyUsing.  Test1.  Test2;
using MyUsing.
    Test3;
using MyUsing.Repeat1;
using  MyUsing.  Repeat2;

namespace Namespace1
{
    // File1
}

namespace Namespace2
{
    // File1
}");
        private TempFile file2 = GetTemp(
            @"using MyUsing.Repeat1;
using MyUsing.Repeat2;

namespace Namespace1
{
    // File2
}

namespace Namespace2
{
    // File2
}");
        private TempFile result = new TempFile();
        private string directory => Path.GetPathRoot(file1.Path);
        private string path1 => Path.GetRelativePath(directory, file1.Path);
        private string path2 => Path.GetRelativePath(directory, file2.Path);


        [TestMethod]
        public void Refactor_MultipleFiles_CorrectUsings()
        {
            RunMain(@$"{path1} {path2} -r -d {directory} -o {result.Path}");
            var actual = File.ReadAllText(result.Path);
            var expected = @"using MyUsing;
using MyUsing.Test1.Test2;
using MyUsing.Test3;
using MyUsing.Repeat1;
using MyUsing.Repeat2;

namespace Namespace1
{
    // File1
}

namespace Namespace2
{
    // File1
}

namespace Namespace1
{
    // File2
}

namespace Namespace2
{
    // File2
}";
        Assert.AreEqual(expected, actual);
        }
    }
}
