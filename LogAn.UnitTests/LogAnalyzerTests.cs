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
 
        [TestCase("abc.foo")]
        [TestCase("abc.123")]
        public void IsValidLogFileName_InvalidExtensions_ReturnFalse(string fileName)
        {
            LogAnalyzer analyzer = new LogAnalyzer();

            bool result = analyzer.IsValidLogFileName(fileName);

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

        [Test]
        public void IsValidLogFileName_EmptyFileName_ThrowsException()
        {
            LogAnalyzer analyzer = new LogAnalyzer();

            var ex = Assert.Catch<Exception>(() => analyzer.IsValidLogFileName(""));

            StringAssert.Contains("filename has to be provided", ex.Message);
        }
    }
}
