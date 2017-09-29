﻿using NUnit.Framework;
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
        public void IsValidLogFileName_InvalidExtensions_ReturnFalse(string fileName)
        {
            LogAnalyzer analyzer = MakeLogAnalyzer();

            bool result = analyzer.IsValidLogFileName(fileName);

            Assert.False(result);
        }

        [TestCase("abc.slf")]
        [TestCase("abc.SLF")]
        public void IsValidLogFileName_ValidExtensions_ReturnTrue(string fileName)
        {
            LogAnalyzer analyzer = MakeLogAnalyzer();

            bool result = analyzer.IsValidLogFileName(fileName);

            Assert.True(result);
        }

        [Test]
        public void IsValidLogFileName_EmptyFileName_ThrowsException()
        {
            LogAnalyzer analyzer = MakeLogAnalyzer();

            var ex = Assert.Catch<Exception>(() => analyzer.IsValidLogFileName(""));

            StringAssert.Contains("filename has to be provided", ex.Message);
        }
    }
}
