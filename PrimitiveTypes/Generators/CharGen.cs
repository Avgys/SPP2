using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveTypes.Generators
{
    class CharGen : IPrimitiveGen
    {
        public Type CurType => typeof(Char);

        public object Create()
        {
            return (char)(new Random().Next(100,255));
        }
    }
}
