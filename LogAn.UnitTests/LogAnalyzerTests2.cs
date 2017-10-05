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
    }
}
