using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eir.DevTools
{
    public static class TypeExtensions
    {
        static NullabilityInfoContext nullabilityContext = new NullabilityInfoContext();

        static bool IsType(this Type t, String typeName)
        {
            if (t is null)
                throw new ArgumentNullException(nameof(t));
            return t.Name == typeName;
        }

        static String CleanName(String name)
        {
            name = name
                .RemovePrefix("Hl7.Fhir.Model.")
            ;
            return name;
        }

        public static bool IsList(this Type t) => t.IsType("List`1");
        public static bool IsCode(this Type t) => t.IsType("Code`1");
        public static bool IsNullable(this Type t) => t.IsType("Nullable`1");

        public static bool IsNullable(this PropertyInfo p)
        {
            NullabilityInfo nullabilityInfo = nullabilityContext.Create(p);
            return nullabilityInfo.WriteState is NullabilityState.Nullable;
        }

        public static String Default(this Type t)
        {
            if (t == typeof(string))
                return "String.Empty";
            return "default";
        }

        public static String Default(this PropertyInfo p)
        {
            NullabilityInfo nullabilityInfo = nullabilityContext.Create(p);
            if (nullabilityInfo.WriteState is NullabilityState.Nullable)
                return "null";
            return p.PropertyType.Default();
        }

        public static string FriendlyName(this Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (type.IsArray)
                return type.GetFriendlyNameOfArrayType();
            if (type.IsGenericType)
                return type.GetFriendlyNameOfGenericType();
            if (type.IsPointer)
                return type.GetFriendlyNameOfPointerType();
            String name = CleanName(type.Name);
            if (type.DeclaringType == null)
                return name;

            String declaringName = FriendlyName(type.DeclaringType);
            return $"{declaringName}.{name}";
        }

        private static string GetFriendlyNameOfArrayType(this Type type)
        {
            string arrayMarker = string.Empty;
            while (type!.IsArray == true)
            {
                string commas = new string(Enumerable.Repeat(',', type.GetArrayRank() - 1).ToArray());
                arrayMarker += $"[{commas}]";
                type = type.GetElementType()!;
            }
            return type.FriendlyName() + arrayMarker;
        }

        private static string GetFriendlyNameOfGenericType(this Type type)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return type.GetGenericArguments().First().FriendlyName() + "?";
            string friendlyName = type.Name;
            int indexOfBacktick = friendlyName.IndexOf('`');
            if (indexOfBacktick > 0)
                friendlyName = friendlyName.Remove(indexOfBacktick);
            IEnumerable<string> typeParameterNames = type
                .GetGenericArguments()
                .Select(typeParameter => typeParameter.FriendlyName());
            string joinedTypeParameters = string.Join(", ", typeParameterNames);
            return string.Format("{0}<{1}>", friendlyName, joinedTypeParameters);
        }

        private static string GetFriendlyNameOfPointerType(this Type type) =>
            type.GetElementType()!.FriendlyName() + "*";
    }
}