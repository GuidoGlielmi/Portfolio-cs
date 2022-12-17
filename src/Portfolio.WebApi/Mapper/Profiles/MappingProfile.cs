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

    // for each type that implements IMapFrom
    foreach (var type in types)
    {
      var instance = Activator.CreateInstance(type);

      var methodInfo = type.GetMethod(mappingMethodName); // methods "inherited" by IMapFrom doesn't show
      /*
      interfaces do not have "inheritance" the same way classes do.
      Inheritance implies the "is a kind of" relationship,
      but interface "inheritance" actually implies the "is required to provide this service" relationship.

      The fact that a subInterface inherits from a superInterface
      doesn’t mean the sub interface type inherits the properties of the super
      because interfaces do not define implementation.

      So, with default interface methods, as they are not declared directly in the inheriting class, they don't own them
      that's why with default (un-overrided) interface methods, it is necessary to look for it directly in
      the implemented interface that defines it.
      AND it the case it does find it, is because it WAS overrided
      */
      if (methodInfo != null) // checks if Mapping() method exists
      {
        // is null when it is not defined in the class directly (so, using the default behaviour)
        methodInfo.Invoke(instance, new object[] { this }); // invokes Mapping
        // new object[] represents all the arguments
        // "this" represents the profile (its only argument)
      } else
      {
        // is not null when it is defined directly int the class (so, not using the default behaviour)
        // if .Mapping() is null, check for the default implementation in the interface
        var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

        foreach (var @interface in interfaces) // @allows you to use a reserved word
        {
          // get the method .Mapping() with the profile as argument
          var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);
          interfaceMethodInfo?.Invoke(instance, new object[] { this });
        }
      }
    }
  }
}