using Microsoft.VisualStudio.TestTools.UnitTesting;
using FakerLib;
using TestProject1.Examples;

using System; 

namespace TestProject1
{
    [TestClass]
    public class FakerTest
    {

        private Faker faker;

        public FakerTest()
        {            
            this.faker = new Faker();            
        }

        [TestMethod]
        public void TestList()
        {
            C c = faker.Create<C>();
            Assert.IsNotNull(c.list);
        }


        public void TestCycle()
        {
            A a = faker.Create<A>();
            Assert.IsNotNull(a.bClass);

            Assert.IsNotNull(a.bClass.cClass);
            Assert.IsNull(a.bClass.aClass);

        }
    }
}
