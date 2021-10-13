using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace FakerLib
{
    using PrimitiveTypes;
    //using PrimitiveTypes.Generators;

    public class Faker
    {
        private Dictionary<Type, IPrimitiveGen> PrimitiveGenDict;
        private Dictionary<Type, IGenericGen> GenericGenDict;
        private Dictionary<PropertyInfo, IPrimitiveGen> CustomPrimitiveGenDict = new Dictionary<PropertyInfo, IPrimitiveGen>();
        private HashSet<Type> CreatedTypesInClass;

        public Faker()
        {
            PrimitiveTypeGenerator.FullFilDictionary();
            this.PrimitiveGenDict = PrimitiveTypeGenerator.Dict;
            this.CreatedTypesInClass = new HashSet<Type>();
            this.GenericGenDict = new Dictionary<Type, IGenericGen>();
            //var temp = new ListGen();
            //this.GenericGenDict.Add(temp.CurType, temp);

            var assemblies = new List<Assembly>();
            var path = "C:\\Users\\ilyuh\\Desktop\\Main Projects For Study\\Новая папка\\Plugins";

            try
            {
                foreach (string file in Directory.GetFiles(path, "*.dll"))
                {
                    try
                    {
                        assemblies.Add(Assembly.LoadFile(file));
                    }
                    catch (BadImageFormatException)
                    { }
                    catch (FileLoadException)
                    { }
                }
            }
            catch (DirectoryNotFoundException)
            { }

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (Type typeInterface in type.GetInterfaces())
                    {
                        if (typeInterface.Equals(typeof(IGenericGen)))
                        {
                            var creator = (IGenericGen)Activator.CreateInstance(type, this.PrimitiveGenDict);
                            GenericGenDict.Add(creator.CurType, creator);
                        }
                        else if (typeInterface.Equals(typeof(IPrimitiveGen)))
                        {
                            var creator = (IPrimitiveGen)Activator.CreateInstance(type);
                            PrimitiveGenDict.Add(creator.CurType, creator);
                        }
                    }
                }
            }
        }

        public Faker(FakerConfig config) : this()
        {
            this.CustomPrimitiveGenDict = config.Creators;
        }

        public T Create<T>()
        {
            return (T)Create(typeof(T));
        }

        object Create(Type type)
        {
            object result = null;

            if (PrimitiveGenDict.TryGetValue(type, out IPrimitiveGen creator)){
                result = creator.Create();
            }
            else if(type.IsValueType)
            {
                result = Activator.CreateInstance(type);
            }
            else if (type.IsGenericType && GenericGenDict.TryGetValue(type.GetGenericTypeDefinition(), out IGenericGen genCreator))
            {
                result = genCreator.Create(type.GenericTypeArguments[0]); //type of object in collection
            }
            else if (type.IsClass && !type.IsArray && !type.IsPointer && !type.IsAbstract && !type.IsGenericType)
            {
                if (!CreatedTypesInClass.Contains(type))
                {
                    result = createClass(type);
                }
                else
                {
                    result = null;
                }
            }

            return result;
        }

        private object createClass(Type type)
        {
            object createdClass = null;

            int largestConstructor = 0;
            ConstructorInfo constructor = null;
            var constructorsOfClass = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (ConstructorInfo curConstructor in constructorsOfClass)
            {
                var curCount = curConstructor.GetParameters().Length;
                if (curCount > largestConstructor)
                {
                    largestConstructor = curCount;
                    constructor = curConstructor;
                }
            }

            CreatedTypesInClass.Add(type);


            if (constructor != null)
            {
                createdClass = CreateFromConstructor(constructor, type);
            }

            createdClass = CreateFromProperties(type, createdClass);

            CreatedTypesInClass.Remove(type);

            return createdClass;
        }

        private object CreateFromProperties(Type type, object createdObject)
        {
            object created = null;
            if (createdObject == null)
            {
                created = Activator.CreateInstance(type);
            }
            else
            {
                created = createdObject;
            }

            foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (fieldInfo.GetValue(created) == null)
                {
                    object value = null;
                    if (!CreateByCustomCreator(fieldInfo, out value))
                    {
                        value = Create(fieldInfo.FieldType);
                    }
                    fieldInfo.SetValue(created, value);
                }
            }

            foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
            {
                if (propertyInfo.CanWrite)
                {
                    if (propertyInfo.GetValue(created) == null)
                    {
                        object value = null;
                        if (!CreateByCustomCreator(propertyInfo, out value))
                        {
                            value = Create(propertyInfo.PropertyType);
                        }
                        propertyInfo.SetValue(created, value);
                    }
                }
            }

            return created;
        }

        private object CreateFromConstructor(ConstructorInfo constructor, Type type)
        {
            var parametersValues = new List<object>();

            foreach (ParameterInfo parameterInfo in constructor.GetParameters())
            {
                object value = null;
                if (!CreateByCustomCreator(parameterInfo, type, out value))
                {
                    value = Create(parameterInfo.ParameterType);
                }
                parametersValues.Add(value);
            }

            try
            {
                return constructor.Invoke(parametersValues.ToArray());
            }
            catch (TargetInvocationException)
            {
                return null;
            }
        }


        private bool CreateByCustomCreator(ParameterInfo parameterInfo, Type type, out object created)
        {
            foreach (KeyValuePair<PropertyInfo, IPrimitiveGen> keyValue in CustomPrimitiveGenDict)
            {
                if (keyValue.Key.Name == parameterInfo.Name && keyValue.Value.CurType.Equals(parameterInfo.ParameterType) && keyValue.Key.ReflectedType.Equals(type))
                {
                    created = keyValue.Value.Create();
                    return true;
                }
            }
            created = null;
            return false;
        }

        private bool CreateByCustomCreator(PropertyInfo propertyInfo, out object created)
        {
            if (CustomPrimitiveGenDict.TryGetValue(propertyInfo, out IPrimitiveGen creator))
            {
                created = creator.Create();
                return true;
            }
            else
            {
                created = null;
                return false;
            }
        }

        private bool CreateByCustomCreator(FieldInfo fieldInfo, out object created)
        {
            foreach (KeyValuePair<PropertyInfo, IPrimitiveGen> keyValue in CustomPrimitiveGenDict)
            {
                if (keyValue.Key.Name == fieldInfo.Name && keyValue.Value.CurType.Equals(fieldInfo.FieldType) && keyValue.Key.ReflectedType.Equals(fieldInfo.ReflectedType))
                {
                    created = keyValue.Value.Create();
                    return true;
                }
            }
            created = null;
            return false;
        }

    }

}
