using System;
using System.Collections;
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
        public static bool HasFields<TSource>(this TSource source, string members) where TSource : class, new()
        {
            Type type = source.GetType();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                type = source.GetType().GetGenericArguments()[0];

            var fields = members.Split(",").AsQueryable().Where(c => !string.IsNullOrEmpty(c)).ToList();

            return fields.Select(field => type
                .GetProperty(field.Trim(), BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance))
                .All(property => property != null);
        }

        public static ExpandoObject ShapeData<TSource>(this TSource source, string members) where TSource : class, new()
        {
            var shapedData = new ExpandoObject();
            var type = source.GetType();
            var properties = type.GetProperties();

            if (string.IsNullOrEmpty(members))
            {
                foreach (var propertyInfo in properties)
                {
                    ((IDictionary<string, object>)shapedData).Add(propertyInfo.Name, propertyInfo.GetValue(source));
                }

                return shapedData;
            }

            var fields = members.Split(",").AsQueryable().Where(c => !string.IsNullOrEmpty(c)).ToList();
            fields.ForEach(f => f.ToLower());
            try
            {

                foreach (var propertyInfo in fields.Select(field => type.GetProperty(field.Trim(),
                    BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)))
                {
                    ((IDictionary<string, object>)shapedData).Add(propertyInfo.Name, propertyInfo.GetValue(source));
                }


                return shapedData;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static IEnumerable<ExpandoObject> ShapeListData<TSource>(this TSource source, string members) where TSource:IEnumerable
        {
            var type = source.GetType();
            var properties = type.GetProperties();
            var shapedData = new ExpandoObject();


            if (string.IsNullOrEmpty(members))
            {
                foreach (var propertyInfo in properties)
                {
                    ((IDictionary<string, object>)shapedData).Add(propertyInfo.Name, propertyInfo.GetValue(source));
                }

                yield return shapedData;
                ((IDictionary<string, object>)shapedData).Clear();
            }

            var listObject = (IEnumerable)source;

            var fields = members?.Split(",");

            foreach (var item in listObject)
            {
                type = item.GetType();

                if (fields == null)
                {
                    foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance))
                    {
                        ((IDictionary<string, object>)shapedData).Add(propertyInfo.Name, propertyInfo.GetValue(item));
                        yield return shapedData;
                        ((IDictionary<string, object>)shapedData).Clear();
                    }
                    yield break;
                }


                foreach (var propertyInfo in fields.Select(field => type.GetProperty(field.Trim(),
                    BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance)))
                {
                    ((IDictionary<string, object>)shapedData).Add(propertyInfo.Name, propertyInfo.GetValue(item));
                }

                yield return shapedData;
                ((IDictionary<string, object>)shapedData).Clear();

            }

        }
    }

}
