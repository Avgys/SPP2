using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveTypes
{
    class IntGen : IPrimitiveGen
    {
        public Type CurType => typeof(int);

        public object Create()
        {
            return new Random().Next();
        }
    }
}
