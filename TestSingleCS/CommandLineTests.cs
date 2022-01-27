using McMaster.Extensions.CommandLineUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSingleCS
{
    [TestClass]
    public class CommandLineTests
    {
        private CommandOption GetOption(string shortName)
         => Program.Application.Options.First(o => o.ShortName == shortName);

        private CommandArgument GetArgument(string name)
            => Program.Application.Arguments.First(a => a.Name == name);

        private void RunMain(string arguments)
        {
            var args = arguments.Split(' ', ',');
            Program.Main(args);
        }

        [TestMethod]
        public void Commandline_TestCase1_CorrectOptions()
        {
            RunMain(@"'c:\dir1\*' 'c:\dir2\*' -m  'c:\dir3\main.cs' -r -n -p");

            CollectionAssert.AreEqual(new[] { @"'c:\dir1\*'", @"'c:\dir2\*'" }, GetArgument("files").Values.ToArray());
            Assert.IsTrue(GetOption("m").HasValue());
            Assert.IsTrue(GetOption("m").Value() == @"'c:\dir3\main.cs'");
            Assert.IsTrue(GetOption("r").HasValue());
            Assert.IsTrue(GetOption("n").HasValue());
            Assert.IsTrue(GetOption("p").HasValue());
        }

        [TestMethod]
        public void Commandline_TestCase2_CorrectOptions()
        {
            RunMain(@"'c:\dir1\*' 'c:\dir2\*' -m 'c:\dir3\main.cs' -p");

            CollectionAssert.AreEqual(new[] { @"'c:\dir1\*'", @"'c:\dir2\*'" }, GetArgument("files").Values.ToArray());
            Assert.IsTrue(GetOption("m").HasValue());
            Assert.IsTrue(GetOption("m").Value() == @"'c:\dir3\main.cs'");
            Assert.IsFalse(GetOption("r").HasValue());
            Assert.IsFalse(GetOption("n").HasValue());
            Assert.IsTrue(GetOption("p").HasValue());
        }

        [TestMethod]
        public void Commandline_TestCase3_CorrectOptions()
        {
            RunMain(@"-m 'c:\dir3\main.cs' -r -n -p 'c:\dir1\*' 'c:\dir2\*'");

            CollectionAssert.AreEqual(new[] { @"'c:\dir1\*'", @"'c:\dir2\*'" }, GetArgument("files").Values.ToArray());
            Assert.IsTrue(GetOption("m").HasValue());
            Assert.IsTrue(GetOption("m").Value() == @"'c:\dir3\main.cs'");
            Assert.IsTrue(GetOption("r").HasValue());
            Assert.IsTrue(GetOption("n").HasValue());
            Assert.IsTrue(GetOption("p").HasValue());
        }

        [TestMethod]
        public void Commandline_TestCase4_CorrectOptions()
        {
            RunMain(@"'c:\dir1\*' 'c:\dir2\*' -m 'c:\dir3\main.cs' -m 'c:\dir4\main.cs' -r");

            CollectionAssert.AreEqual(new[] { @"'c:\dir1\*'", @"'c:\dir2\*'" }, GetArgument("files").Values.ToArray());
            Assert.IsTrue(GetOption("m").HasValue());
            CollectionAssert.AreEqual(GetOption("m").Values.ToArray(), new[] { @"'c:\dir3\main.cs'", @"'c:\dir4\main.cs'" });
            Assert.IsTrue(GetOption("r").HasValue());
            Assert.IsFalse(GetOption("n").HasValue());
            Assert.IsFalse(GetOption("p").HasValue());
        }
    }
}
