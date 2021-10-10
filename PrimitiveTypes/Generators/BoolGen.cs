using System;

namespace PrimitiveTypes.Generators
{
    public class BoolGen : IPrimitiveGen
    {
        public Type CurType => typeof(bool);

        public object Create()
        {
            var number = new Random().Next();
            return number % 2 == 0;
        }
    }
}
