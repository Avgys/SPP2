using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimitiveTypes
{
    public interface IPrimitiveGen : IType
    {
        object Create();
    }
}
