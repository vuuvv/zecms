using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vuuvv.db
{
    public static class TypeConvert
    {
        public static object to_number(object obj, Type type)
        {
            Type t = obj.GetType();
            if (t == type)
                return obj;
            string method_name = string.Format("To{0}", type.Name);
            var method = typeof(Convert).GetMethod(method_name, new[] {t});
            if (method != null)
                return method.Invoke(null, new[] { obj });
            throw new InvalidCastException(string.Format("Can't convert type {0} in Convert.{1}()", t.Name, method_name));
        }

        public static T to_number<T>(object obj)
        {
            return (T)to_number(obj, typeof(T));
        }

        public static object GetDefault(Type t)
        {
            if (!t.IsValueType)
                return null;
            return Activator.CreateInstance(t);
        }

        public static int to_enum(object obj, Type t)
        {
            return (int)Enum.Parse(t, obj.ToString());
        }

        public static T to_enum<T>(object obj)
        {
            return (T)(object)to_enum(obj, typeof(T));
        }
    }
}
