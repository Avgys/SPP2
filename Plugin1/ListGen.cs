using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveTypes.Generators
{
    using PrimitiveTypes;

    public class ListGen : IGenericGen
    {
        public Type CurType => typeof(List<>);

        private Dictionary<Type, IPrimitiveGen> PrimitiveGenDict;

        public ListGen(Dictionary<Type, IPrimitiveGen> dict)
        {
           this.PrimitiveGenDict = dict;
        }

        public object Create(Type type)
        {
            IList result = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            var length = new Random().Next(1, 10);

            if (PrimitiveGenDict.TryGetValue(type, out IPrimitiveGen creator))
            {
                for (int i = 0; i < length; i++)
                {
                    result.Add(creator.Create());
                }
            }
            else
            {
                var defaultValue = "DEFAULT";
                for (int i = 0; i < length; i++)
                {
                    result.Add(defaultValue);
                }
            }

            return result;
        }
    }
}
