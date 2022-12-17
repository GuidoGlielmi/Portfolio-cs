using Microsoft.CodeAnalysis.Differencing;
using Newtonsoft.Json;
using Portfolio.WebApi.Errors;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Portfolio.WebApi.Extensions;

public static class DynamicWhereExtension
{


  public static IEnumerable<T> DynamicWhereAll<T, T2>(this IEnumerable<T> src, T2 comparator)
  {
    var filteredList = src.ToList();
    var keyValuePairs = comparator.ToDictionary();
    foreach (var (key, value) in keyValuePairs)
    {
      filteredList = DynamicWhere(filteredList, key, value).ToList();
    }
    return filteredList;
  }

  public static IEnumerable<T> DynamicWhereAll<T>(this IEnumerable<T> src, Dictionary<string, string> keyValuePairs)
  {
    var filteredList = src.ToList();
    foreach (var (key, value) in keyValuePairs)
    {
      filteredList = DynamicWhere(filteredList, key, value).ToList();
    }
    return filteredList;
  }
  public static IEnumerable<T> DynamicWhere<T>(this IEnumerable<T> src, string propertyName, string propertyValue)
  {
    // T could be Education (the parameter used in a linq).

    //              |
    //              |
    //              V
    //tuvieja.Where(T => ...)

    // propertyExpression represent a property on the parameter, which is created from the linq parameter and
    // a name

    //                    |
    //                    |
    //                    V
    //tuvieja.Where(T => T.prop)

    // a constant expression is a constant made an expression, through a value and a type

    // containsMethodExpression is a representation of the call to propertyExpression.Contains(propertyValue)

    // predicate is a representation of a Lambda where the body is containsMethodExpression and the
    // parameter is parameterExpression

    try
    {
      var parameterExpression = Expression.Parameter(typeof(T), "type");
      var propertyExpression = Expression.Property(parameterExpression, propertyName);

      MethodInfo containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
      var propertyValueExpression = Expression.Constant(propertyValue, typeof(string));
      var containsMethodExpression = Expression.Call(propertyExpression, containsMethod, propertyValueExpression);
      var predicate = Expression.Lambda<Func<T, bool>>(containsMethodExpression, parameterExpression);
      return src.AsQueryable().Where(predicate);
    } catch (ArgumentException)
    {
      throw new ArgumentException($"The \"{propertyName}\" property does not exist in {typeof(T).Name} type");
    }
  }


}

public class LowerCaseStringConverter : JsonConverter<string>
{
  public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
  {
    writer.WriteValue(value.ToLower());
  }

  public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
  {
    return (string)reader.Value;
  }
}