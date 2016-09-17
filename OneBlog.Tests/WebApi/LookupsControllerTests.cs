using OneBlog.Tests.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OneBlog.Tests.WebApi
{
    [TestClass]
    public class LookupsControllerTests
    {
        [TestMethod]
        public void LookupControllerGet()
        {
            var ctrl = new LookupsController(new FakeLookupsRepository());
            var result = ctrl.Get();

            Assert.IsNotNull(result);
        }
    }
}
