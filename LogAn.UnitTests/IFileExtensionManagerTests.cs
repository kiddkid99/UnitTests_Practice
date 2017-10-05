using System;
using NSubstitute;
using NUnit.Framework;
namespace LogAn.UnitTests
{
    [TestFixture]
    [Category("NSub")]
    public class IFileExtensionManagerTests
    {
        [Test]
        public void Returns_ByDefault_WorksForHardCodedArgument()
        {
            IFileExtensionManager fakeManger = Substitute.For<IFileExtensionManager>();

            //不考慮輸入參數，模擬回傳 true
            fakeManger.IsValid(Arg.Any<string>()).Returns(true);

            Assert.IsTrue(fakeManger.IsValid("abc.txt"));

        }

        [Test]
        public void Returns_ArgAny_Throws()
        {
            IFileExtensionManager fakeManager = Substitute.For<IFileExtensionManager>();

            //不考慮輸入參數，模擬丟出例外
            fakeManager.When(x => x.IsValid(Arg.Any<string>()))
                .Do(context =>
                {
                    throw new Exception("fake exception");
                });

            Assert.Throws<Exception>(() =>
            {
                fakeManager.IsValid("abc.txt");
            });
        }
    }
}
