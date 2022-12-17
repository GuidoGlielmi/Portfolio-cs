using Newtonsoft.Json;

namespace Portfolio.WebApi.Extensions;

public static class ObjectHandlerExtensions
{
  public static Dictionary<string, string> ToDictionary(this object obj)
  {
    var json = JsonConvert.SerializeObject(obj);
    var jsonConfig = new JsonSerializerSettings
    {
      Converters = new List<JsonConverter> { new LowerCaseStringConverter() },
      NullValueHandling = NullValueHandling.Ignore,
    };
    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    return new Dictionary<string, string>(dictionary.Where((kvp) => kvp.Value != null));
  }

  public static void SetTo(this object source, object target)
  {
    var targetProps = target.GetType().GetProperties();
    foreach (var targetProp in targetProps)
    {
      var targetPropValue = targetProp.GetValue(target);

      var sourcePropValue = source.GetType().GetProperty(targetProp.Name)?.GetValue(source);

      if (sourcePropValue?.GetType() == targetPropValue?.GetType())
      {
        targetProp.SetValue(target, sourcePropValue);
      }
    }
  }

  public static void SetFrom(this object target, object source)
  {
    var targetProps = target.GetType().GetProperties();
    foreach (var targetProp in targetProps)
    {
      var targetPropValue = targetProp.GetValue(target);

      var sourcePropValue = source.GetType().GetProperty(targetProp.Name)?.GetValue(source);

      if (sourcePropValue?.GetType() == targetPropValue?.GetType())
      {
        targetProp.SetValue(target, sourcePropValue);
      }
    }
  }
}
