using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.Examples
{
    class ClassWithBool
    {
        public bool boolean;
    }

    class ClassWithConstructor
    {
        private int a;
        public DateTime b;
        public C c;

        private ClassWithConstructor(int a, DateTime b)
        {
            this.a = a;
            this.b = b;

        }
    }

    class TestClassWithString
    {
        public string str { get; }

        public TestClassWithString(string str)
        {
            this.str = str;
        }
    }

    class A
    {
        public int integer;
        public B bClass;
    }

    class B
    {
        public char character;
        public A aClass;
        public C cClass;
    }

    public class C
    {
        public List<int> list;
    }
}
