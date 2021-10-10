using System;
using System.Collections.Generic;

using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Reflection;

namespace FakerLib
{
    using PrimitiveTypes;
    public class FakerConfig
    {
        public Dictionary<PropertyInfo, IPrimitiveGen> Creators;

        public void add<TClass, TProperty, TPrimitive>(Expression<Func<TClass, TProperty>> expression)
           where TClass : class
           where TPrimitive : IPrimitiveGen
        {
            Expression expressionBody = expression.Body;
            IPrimitiveGen creator = (IPrimitiveGen)Activator.CreateInstance(typeof(TPrimitive));
            if (!creator.CurType.Equals(typeof(TProperty)))
            {
                throw new ArgumentException("Types of creators aren't match");
            }
            Creators.Add((PropertyInfo)((MemberExpression)expressionBody).Member, creator);
        }

        public FakerConfig()
        {
            Creators = new Dictionary<PropertyInfo, IPrimitiveGen>();
        }
    }
}
