using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogAn.UnitTests
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        [Test]
        public void IsValidLogFileName_BadExtenstion_ReturnFalse()
        {
            LogAnalyzer analyzer = new LogAnalyzer();

            bool result = analyzer.IsValidLogFileName("abc.foo");

            Assert.False(result);
        }

        [TestCase("abc.slf")]
        [TestCase("abc.SLF")]
        public void IsValidLogFileName_ValidExtensions_ReturnTrue(string fileName)
        {
            LogAnalyzer analyzer = new LogAnalyzer();

            bool result = analyzer.IsValidLogFileName(fileName);

            Assert.True(result);
        }
    }
}
