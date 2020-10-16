using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper.Internal;

namespace CustomMapping.Service.DataShaping
{
    public static class ObjectExtensions
    {
        public static bool HasFields<TSource>(this TSource source, string members) where TSource :class,new()
        {
          

            var fields = members.Split(",").AsQueryable().Where(c=>!string.IsNullOrEmpty(c)).ToList();

            return fields.Select(field => source.GetType()
                .GetProperty(field.Trim(), BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance))
                .All(property => property != null);
        }

        public static ExpandoObject ShapeData<TSource>(this TSource source,string members) where TSource : class, new()
        {
            var shapedData = new ExpandoObject();

            var properties = source.GetType().GetProperties();

            if (string.IsNullOrEmpty(members))
            {
                foreach (var propertyInfo in properties)
                {
                    ((IDictionary<string,object>)shapedData).Add(propertyInfo.Name,propertyInfo.GetValue(source));
                }

                return shapedData;
            }

            var fields = members.Split(",").AsQueryable().Where(c=> !string.IsNullOrEmpty(c)).ToList();

            try
            {
                foreach (var propertyInfo in fields.Select(field => source.GetType().GetProperty(field.Trim(),
                    BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)))
                {
                    ((IDictionary<string,object>)shapedData).Add(propertyInfo.Name,propertyInfo.GetValue(source));
                }

                return shapedData;
            }
            catch 
            {
                return null;
            }
        }
    }
}
