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



        [Test]
        [Category("Fast Tests")]
        public void IsValidLogFileNameFromExternal_NameSupportedExtension_ReturnTrue()
        {
            AlwaysValidFakeExtensionManager manager = new AlwaysValidFakeExtensionManager();
            manager.WillBeValid = true;

            LogAnalyzer log = new LogAnalyzer(manager);

            bool result = log.IsValidLogFileNameFromExternal("short.ext");
            Assert.True(result);

        }


        [Test]
        [Category("Fast Tests")]
        public void IsValidLogFileNameFromExternal_ExtManagerThrowsException_ReturnFalse()
        {
            AlwaysValidFakeExtensionManager manager = new AlwaysValidFakeExtensionManager();
            manager.WillThrow = new Exception("this is fake.");

            LogAnalyzer log = new LogAnalyzer(manager);

            bool result = log.IsValidLogFileNameFromExternal("blahblah.ext");
            Assert.False(result);
        }


        [Test]
        [Category("Fast Tests")]
        public void overrideTest()
        {
            AlwaysValidFakeExtensionManager stub = new AlwaysValidFakeExtensionManager();
            stub.WillBeValid = true;

            TestableLogAnalyzer logan = new TestableLogAnalyzer(stub);

            bool result = logan.IsValidLogFileName("file.ext");

            Assert.True(result);



        }


        [Test]
        public void Analyze_TooShortFileName_CallWebService()
        {
            FakeWebService mockService = new FakeWebService();
            LogAnalyzer log = new LogAnalyzer();
            log.Service = mockService;
            string tooShortFileName = "abc.ext";

            log.Analyze(tooShortFileName);
            

            StringAssert.Contains("File name too short: abc.ext", mockService.LastError);

        }


        [Test]
        public void Analyze_WebServiceThrows_SendsEmail()
        {
            FakeWebService stubWebService = new FakeWebService();
            stubWebService.ToThrow = new Exception("fake exception");

            FakeEmailService mockEmail = new FakeEmailService();

            LogAnalyzer log = new LogAnalyzer(stubWebService, mockEmail);
            string tooShortFilaName = "abc.ext";
            log.Analyze(tooShortFilaName);

            StringAssert.Contains("someone@somewhere.com", mockEmail.To);
            StringAssert.Contains("fake exception", mockEmail.Body);
            StringAssert.Contains("can't log", mockEmail.Subject);

        }
    }

    class TestableLogAnalyzer : LogAnalyzerUsingFactoryMethod
    {
        public IFileExtensionManager Manager;

        public TestableLogAnalyzer(IFileExtensionManager manager)
        {
            this.Manager = manager;
        }

        public override IFileExtensionManager GetManager()
        {
            return Manager;
        }
    }

    internal class AlwaysValidFakeExtensionManager : IFileExtensionManager
    {
        public bool WillBeValid = false;
        public Exception WillThrow = null;

        public bool IsValid(string fileName)
        {
            if(WillThrow != null)
            {
                throw WillThrow;
            }

            return WillBeValid;
        }
    }

    internal class FakeWebService : IWebService
    {
        public Exception ToThrow;
        public string LastError;

        public void LogError(string message)
        {
            if(ToThrow != null)
            {
                throw ToThrow;
            }

            LastError = message;
        }
    }

    internal class FakeEmailService : IEmailService
    {
        public string To;
        public string Subject;
        public string Body;

        public void SendEmail(string to, string subject, string body)
        {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}
