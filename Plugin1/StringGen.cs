using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveTypes.Generators
{
    class StringGen : IPrimitiveGen
    {
        public Type CurType => typeof(string);

        object IPrimitiveGen.Create()
        {
            var random = new Random();
            byte[] bytesArray = new byte[random.Next(30)];
            random.NextBytes(bytesArray);
            return (string)bytesArray.ToString();
        }
    }    
}
