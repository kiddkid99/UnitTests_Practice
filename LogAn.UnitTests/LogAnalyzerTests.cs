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
        private LogAnalyzer MakeLogAnalyzer()
        {
            return new LogAnalyzer();
        }
        
        [TestCase("abc.foo")]
        [TestCase("abc.123")]
        [Category("Fast Tests")]
        public void IsValidLogFileName_InvalidExtensions_ReturnFalse(string fileName)
        {
            LogAnalyzer analyzer = MakeLogAnalyzer();

            bool result = analyzer.IsValidLogFileName(fileName);

            Assert.False(result);
        }

        [TestCase("abc.slf")]
        [TestCase("abc.SLF")]
        [Category("Fast Tests")]
        public void IsValidLogFileName_ValidExtensions_ReturnTrue(string fileName)
        {
            LogAnalyzer analyzer = MakeLogAnalyzer();

            bool result = analyzer.IsValidLogFileName(fileName);

            Assert.True(result);
        }

        [Test]
        [Category("Fast Tests")]
        public void IsValidLogFileName_EmptyFileName_ThrowsException()
        {
            LogAnalyzer analyzer = MakeLogAnalyzer();

            var ex = Assert.Catch<Exception>(() => analyzer.IsValidLogFileName(""));

            StringAssert.Contains("filename has to be provided", ex.Message);
        }

        [TestCase("badfile.foo", false)]
        [TestCase("goodfile.slf", true)]
        [Category("Fast Tests")]
        public void IsValidLogFileName_WhenCalled_ChangesWasLastFileNameValid(string fileName, bool expected)
        {
            LogAnalyzer analyzer = MakeLogAnalyzer();

            analyzer.IsValidLogFileName(fileName);

            Assert.AreEqual(expected, analyzer.WasLastFileNameValid);
        }


    }
}
