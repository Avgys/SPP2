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

        [TestMethod]
        public void TestCycle()
        {
            A a = faker.Create<A>();
            Assert.IsNotNull(a.bClass);

            Assert.IsNotNull(a.bClass.cClass);
            Assert.IsNull(a.bClass.aClass);
        }

        [TestMethod]
        public void TestListCreator()
        {
            C c = faker.Create<C>();
            Assert.IsNotNull(c.list);
        }

        [TestMethod]
        public void TestClassWithPrivateConstuctorWithoutFullParameters()
        {
            ClassWithConstructor c = faker.Create<ClassWithConstructor>();
            Assert.IsNotNull(c.c);
        }


        [TestMethod]
        public void TestBoolFromDll()
        {
            ClassWithBool instance = faker.Create<ClassWithBool>();
            Assert.IsTrue(instance.boolean == false || instance.boolean == true);
        }

        [TestMethod]
        public void TestConfigOfFaker()
        {
            var config = new FakerConfig();
            config.add<TestClassWithString, string, DefaultStringGen>(instance => instance.str);

            faker = new Faker(config);

            TestClassWithString instance = faker.Create<TestClassWithString>();
            Assert.AreEqual("default", instance.str);

        }
    }
}
