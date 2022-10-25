using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using System.Reflection;

namespace Portfolio.WebApi.Repositories;

public static class RepositoryConfiguration
{
  public static void AddRepositoryInjections(WebApplicationBuilder builder)
  {
    List<Type> types = Assembly.GetExecutingAssembly()
      .GetTypes() // gets all types
      .Where(type =>
        (!type.IsAbstract // that are not abstract
        && !type.IsInterface // and are not an interface
        && type
          .GetInterfaces() // that implement an interface
          .Where(i => i.IsGenericType) // that is generic
          .Any(i => i.GetGenericTypeDefinition() == typeof(IService<,>))) // of type IService or any of its subtypes
        || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(PortfolioMapper<,,>))
       )
      .ToList();

    types.ForEach(typeImplementation =>
    {
      if (typeImplementation.IsGenericType)
      {
        builder.Services.AddScoped(typeImplementation, typeImplementation.GetGenericTypeDefinition());
      } else
      {
        Type iServiceGeneric = typeof(IService<,>);
        // retrieves its implemented interface whose definition matches IService<,> or any subtype
        Type serviceType = typeImplementation.GetInterfaces().First(i =>
        {
          IEnumerable<Type> inheritedInterfaces = i.GetInterfaces().Select(i => i.GetGenericTypeDefinition());
          return inheritedInterfaces.Contains(iServiceGeneric) || i.GetGenericTypeDefinition() == iServiceGeneric;
          // IsSubClass doesnt work with interfaces
          // isAssignableFrom/To doesn't work with open generics because by being generic,
          // their type cannot be inferred separately 
          // IsAssignableFrom yields true if any of the following conditions is true:
          // c and the current instance represent the same type.
          // c inherits either directly or indirectly from the current instance.
          // The current instance is an interface that c implements.
          // c is a generic type parameter, and the current instance represents one of the constraints of c.
        });
        //builder.Services.AddScoped(typeof(IService<,>), typeImplementation); // this doesnt work as the generics need to be passed
        builder.Services.AddScoped(serviceType, typeImplementation);
      }
    });
  }

}
