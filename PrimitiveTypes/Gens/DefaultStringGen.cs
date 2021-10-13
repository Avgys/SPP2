using System;
using System.Collections.Generic;
using System.Linq;

using PrimitiveTypes;

namespace FakerLib
{
    public class DefaultStringGen : IPrimitiveGen
    {
        public Type CurType => typeof(string);

        public object Create()
        {
            return "default";
        }
    }
}