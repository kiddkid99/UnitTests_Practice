using System;
using NSubstitute;
using NUnit.Framework;

namespace LogAn.UnitTests
{
    [TestFixture]
    [Category("NSub")]
    public class LogAnalyzerTests2
    {
        [Test]
        public void Analyze_TooShortFileName_CallWebService()
        {
            IWebService service = Substitute.For<IWebService>();
            LogAnalyzer analyzer = new LogAnalyzer();
            analyzer.Service = service;

            analyzer.MinNameLength = 6;
            analyzer.Analyze("a.txt");

            service.Received().LogError("File name too short: a.txt");
                
        }

        [Test]
        public void Analyze_WebServiceThrow_SendsEmail()
        {
            var stubWebService = Substitute.For<IWebService>();
            var mockEmail = Substitute.For<IEmailService>();
            stubWebService.When(service => service.LogError(Arg.Any<string>()))
                .Do(info => 
                {
                    throw new Exception("fake exception");
                });


            var analyzer = new LogAnalyzer(stubWebService, mockEmail);

            analyzer.MinNameLength = 10;
            analyzer.Analyze("short.txt");

            mockEmail.Received().SendEmail(
                Arg.Is<string>(s => s.Contains("someone@somewhere.com")),
                Arg.Is<string>(s => s.Contains("can't log")),
                Arg.Is<string>(s => s.Contains("fake exception"))
                );

        }
    }
}
