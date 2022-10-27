using AutoMapper;
using System.Reflection;

namespace Portfolio.WebApi.Mapper.Profiles;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
  }

  private void ApplyMappingsFromAssembly(Assembly assembly)
  {
    // This method is called ONLY once
    var mapFromType = typeof(IMapFrom<>);

    var mappingMethodName = nameof(IMapFrom<object>.Mapping);

    bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType; // if implements IMapFrom

    var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();
    // any type in the ExportedTypes that implements IMapFrom

    var argumentTypes = new Type[] { typeof(Profile) };

    foreach (var type in types)
    {
      // for each type that implements IMapFrom
      var instance = Activator.CreateInstance(type);

      var methodInfo = type.GetMethod(mappingMethodName);

      if (methodInfo != null) // checks if Mapping() method exists
      {
        // might be null because 
        methodInfo.Invoke(instance, new object[] { this }); // invokes Mapping
        // new object[] represents all the arguments
        // "this" represents the profile (its only argument)
      } else
      {
        // if .Mapping() is null, check again if the type implements IMapFrom 
        var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

        //if (interfaces.Count > 0)
        //{
        foreach (var @interface in interfaces) // @allows you to use a reserved word
        {
          // get the method .Mapping() with the profile as argument
          var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

          interfaceMethodInfo?.Invoke(instance, new object[] { this });
        }
        //}
      }
    }
  }
}