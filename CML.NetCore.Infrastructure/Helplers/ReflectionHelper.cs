﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CML.Lib.Utils
{
    /// <summary>
    /// Copyright (C) 2017 cml 版权所有。
    /// 类名：ReflectionHelper.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：ReflectionHelper
    /// 创建标识：cml 2018/1/29 14:05:33
    /// </summary>
    public class ReflectionHelper
    {
        /// <summary>
        /// 获取类型描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static string GetDescription<T>()
        {
            return GetDescription(CommonHelper.GetType<T>());
        }

        /// <summary>
        /// 获取类型成员描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="memberName">成员名称</param>
        public static string GetDescription<T>(string memberName)
        {
            return GetDescription(CommonHelper.GetType<T>(), memberName);
        }

        /// <summary>
        /// 获取类型成员描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="memberName">成员名称</param>
        public static string GetDescription(Type type, string memberName)
        {
            if (type == null)
                return string.Empty;
            if (string.IsNullOrWhiteSpace(memberName))
                return string.Empty;
            return GetDescription(type.GetTypeInfo().GetMember(memberName).FirstOrDefault());
        }

        /// <summary>
        /// 获取类型成员描述，使用DescriptionAttribute设置描述
        /// </summary>
        /// <param name="member">成员</param>
        public static string GetDescription(MemberInfo member)
        {
            if (member == null)
                return string.Empty;
            return member.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute attribute ? attribute.Description : member.Name;
        }

        /// <summary>
        /// 获取类型显示名称，使用DisplayNameAttribute设置显示名称
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static string GetDisplayName<T>()
        {
            return GetDisplayName(CommonHelper.GetType<T>());
        }

        /// <summary>
        /// 获取类型显示名称，使用DisplayNameAttribute设置显示名称
        /// </summary>
        private static string GetDisplayName(Type type)
        {
            if (type == null)
                return string.Empty;
            return type.GetCustomAttribute(typeof(DisplayNameAttribute)) is DisplayNameAttribute attribute ? attribute.DisplayName : string.Empty;
        }

        /// <summary>
        /// 获取类型显示名称或描述,使用DisplayNameAttribute设置显示名称,使用DescriptionAttribute设置描述
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        public static string GetDisplayNameOrDescription<T>()
        {
            var type = CommonHelper.GetType<T>();
            var result = GetDisplayName(type);
            return string.IsNullOrWhiteSpace(result) ? GetDescription(type) : result;
        }

        /// <summary>
        /// 获取属性显示名称或描述,使用DisplayNameAttribute或DisplayAttribute设置显示名称,使用DescriptionAttribute设置描述
        /// </summary>
        public static string GetDisplayNameOrDescription(PropertyInfo member)
        {
            var result = GetDisplayName(member);
            return string.IsNullOrWhiteSpace(result) ? GetDescription(member) : result;
        }

        /// <summary>
        /// 获取类型成员显示名称，使用DisplayNameAttribute或DisplayAttribute设置显示名称
        /// </summary>
        private static string GetDisplayName(PropertyInfo member)
        {
            if (member == null)
                return string.Empty;
            if (member.GetCustomAttribute(typeof(DisplayNameAttribute)) is DisplayNameAttribute displayNameAttribute)
                return displayNameAttribute.DisplayName;
            return member.GetCustomAttribute(typeof(DisplayAttribute)) is DisplayAttribute displayAttribute ? displayAttribute.Name : string.Empty;
        }

        /// <summary>
        /// 获取实现了接口的所有具体类型
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="assembly">在该程序集中查找</param>
        public static List<TInterface> GetTypesByInterface<TInterface>(Assembly assembly)
        {
            var typeInterface = typeof(TInterface);
            return assembly.GetTypes()
                .Where(t => typeInterface.GetTypeInfo().IsAssignableFrom(t) && t != typeInterface && t.GetTypeInfo().IsAbstract == false)
                .Select(t => CreateInstance<TInterface>(t)).ToList();
        }

        /// <summary>
        /// 动态创建实例
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="type">类型</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static T CreateInstance<T>(Type type, params object[] parameters)
        {
            return ConvertHelper.To<T>(Activator.CreateInstance(type, parameters));
        }

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        public static Assembly GetAssembly(string assemblyName)
        {
            return Assembly.Load(new AssemblyName(assemblyName));
        }

        /// <summary>
        /// 是否布尔类型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsBool(MemberInfo member)
        {
            if (member == null)
                return false;
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Boolean";
                case MemberTypes.Property:
                    return IsBool((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否布尔类型
        /// </summary>
        private static bool IsBool(PropertyInfo property)
        {
            if (property.PropertyType == typeof(bool))
                return true;
            if (property.PropertyType == typeof(bool?))
                return true;
            return false;
        }

        /// <summary>
        /// 是否枚举类型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsEnum(MemberInfo member)
        {
            if (member == null)
                return false;
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return ((TypeInfo)member).IsEnum;
                case MemberTypes.Property:
                    return IsEnum((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否枚举类型
        /// </summary>
        private static bool IsEnum(PropertyInfo property)
        {
            if (property.PropertyType.GetTypeInfo().IsEnum)
                return true;
            var value = Nullable.GetUnderlyingType(property.PropertyType);
            if (value == null)
                return false;
            return value.GetTypeInfo().IsEnum;
        }

        /// <summary>
        /// 是否日期类型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsDate(MemberInfo member)
        {
            if (member == null)
                return false;
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.DateTime";
                case MemberTypes.Property:
                    return IsDate((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否日期类型
        /// </summary>
        private static bool IsDate(PropertyInfo property)
        {
            if (property.PropertyType == typeof(DateTime))
                return true;
            if (property.PropertyType == typeof(DateTime?))
                return true;
            return false;
        }

        /// <summary>
        /// 是否整型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsInt(MemberInfo member)
        {
            if (member == null)
                return false;
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Int32" || member.ToString() == "System.Int16" || member.ToString() == "System.Int64";
                case MemberTypes.Property:
                    return IsInt((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否整型
        /// </summary>
        private static bool IsInt(PropertyInfo property)
        {
            if (property.PropertyType == typeof(int))
                return true;
            if (property.PropertyType == typeof(int?))
                return true;
            if (property.PropertyType == typeof(short))
                return true;
            if (property.PropertyType == typeof(short?))
                return true;
            if (property.PropertyType == typeof(long))
                return true;
            if (property.PropertyType == typeof(long?))
                return true;
            return false;
        }

        /// <summary>
        /// 是否数值类型
        /// </summary>
        /// <param name="member">成员</param>
        public static bool IsNumber(MemberInfo member)
        {
            if (member == null)
                return false;
            switch (member.MemberType)
            {
                case MemberTypes.TypeInfo:
                    return member.ToString() == "System.Double" || member.ToString() == "System.Decimal" || member.ToString() == "System.Single";
                case MemberTypes.Property:
                    return IsNumber((PropertyInfo)member);
            }
            return false;
        }

        /// <summary>
        /// 是否数值类型
        /// </summary>
        private static bool IsNumber(PropertyInfo property)
        {
            if (property.PropertyType == typeof(double))
                return true;
            if (property.PropertyType == typeof(double?))
                return true;
            if (property.PropertyType == typeof(decimal))
                return true;
            if (property.PropertyType == typeof(decimal?))
                return true;
            if (property.PropertyType == typeof(float))
                return true;
            if (property.PropertyType == typeof(float?))
                return true;
            return false;
        }

        /// <summary>
        /// 是否泛型集合
        /// </summary>
        /// <param name="type">类型</param>
        public static bool IsGenericCollection(Type type)
        {
            if (!type.IsGenericType)
                return false;
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(IEnumerable<>)
                   || typeDefinition == typeof(IReadOnlyCollection<>)
                   || typeDefinition == typeof(IReadOnlyList<>)
                   || typeDefinition == typeof(ICollection<>)
                   || typeDefinition == typeof(IList<>)
                   || typeDefinition == typeof(List<>);
        }

        /// <summary>
        /// 从目录中获取所有程序集
        /// </summary>
        /// <param name="directoryPath">目录绝对路径</param>
        public static List<Assembly> GetAssemblies(string directoryPath)
        {
            return Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories).ToList()
                .Where(t => t.EndsWith(".exe") || t.EndsWith(".dll"))
                .Select(path => Assembly.Load(new AssemblyName(path))).ToList();
        }

        /// <summary>
        /// 获取类成员和成员类型的Attribute集合
        /// </summary>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="type">Type</param>
        /// <param name="inherit">Inherit attribute from base classes</param>
        public static List<object> GetAttributesOfMemberAndType(MemberInfo memberInfo, Type type, bool inherit = true)
        {
            var attributeList = new List<object>();
            attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));
         //   attributeList.AddRange(type.GetTypeInfo().GetCustomAttributes(inherit));
            return attributeList;
        }
    }
}
