using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TYSystem.BaseFramework.Extension
{
    /// <summary>
    /// 类型判断
    /// </summary>
    public static class TypeExtentions
    {
        public static bool AllowsNullValue(this Type type)
        {
            return (!type.IsValueType || type.IsNullableType());
        }

        public static object DefaultValue(this Type type)
        {
            return (type.IsValueType ? Activator.CreateInstance(type) : null);
        }

        public static Type GetUnderlyingType(this Type type)
        {
            if (!type.IsNullableType())
            {
                return type;
            }
            return Nullable.GetUnderlyingType(type);
        }

        public static bool IsBooleanType(this Type type)
        {
            return (type.GetUnderlyingType() == typeof(bool));
        }

        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)));
        }

        public static bool IsNumbericType(this Type destDataType)
        {
            return ((((((destDataType == typeof(int)) || (destDataType == typeof(uint))) || ((destDataType == typeof(double)) || (destDataType == typeof(short)))) || (((destDataType == typeof(ushort)) || (destDataType == typeof(decimal))) || ((destDataType == typeof(long)) || (destDataType == typeof(ulong))))) || ((destDataType == typeof(float)) || (destDataType == typeof(byte)))) || (destDataType == typeof(sbyte)));
        }

        public static bool IsPrimitive(this Type t)
        {
            if (t.IsGenericType)
            {
                return (t.IsNullableType() && Nullable.GetUnderlyingType(t).IsPrimitive());
            }
            return ((((((t == typeof(string)) || (t == typeof(short))) || ((t == typeof(ushort)) || (t == typeof(int)))) || (((t == typeof(uint)) || (t == typeof(long))) || ((t == typeof(ulong)) || (t == typeof(float))))) || ((((t == typeof(double)) || (t == typeof(decimal))) || ((t == typeof(char)) || (t == typeof(byte)))) || (((t == typeof(sbyte)) || (t == typeof(bool))) || (t == typeof(DateTime))))) || (t == typeof(Guid)));
        }

        public static bool IsStringType(this Type type)
        {
            return (((((type == typeof(string)) || (type == typeof(bool))) || ((type == typeof(DateTime)) || (type == typeof(Guid)))) || ((type == typeof(bool?)) || (type == typeof(DateTime?)))) || (type == typeof(Guid?)));
        }
    }
}
