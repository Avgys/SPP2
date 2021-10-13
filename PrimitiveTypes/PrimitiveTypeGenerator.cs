using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PrimitiveTypes
{
    using Generators;

    public static class PrimitiveTypeGenerator
    {

        public static Dictionary<Type, IPrimitiveGen> Dict { get;  private set; }

        static PrimitiveTypeGenerator()
        {            
            FullFilDictionary();
        }
           

        static void AddToDictionary(IPrimitiveGen gen, Dictionary<Type, IPrimitiveGen> dict = null)
        {
            if (dict == null)
            {
                Dict.Add(gen.CurType, gen);
            }
            else
            {
                dict.Add(gen.CurType, gen);
            }
        }

        public static void FullFilDictionary()
        {
            Dict = new Dictionary<Type, IPrimitiveGen>();

            AddToDictionary(new IntGen());
            AddToDictionary(new DateGen());
            AddToDictionary(new CharGen());
        }

    }
}
