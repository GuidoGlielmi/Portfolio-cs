using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Portfolio.WebApi.IRepositories;
using Portfolio.WebApi.Mapper;
using Portfolio.WebApi.Models;
using Portfolio.WebApi.PipelineBehaviors;
using Portfolio.WebApi.Repositories;
using Portfolio.WebApi.Security.AuthenticationService;
using Portfolio.WebApi.Security.Token;
using System.Reflection;

namespace Portfolio.WebApi.Extensions;

public class PortfolioServicesAggregator
{
  // sealed can also be used to cut the virtual chain on a property or method (because with override also applies virtual)
  private readonly IEnumerable<Type> _implementableTypes = Assembly
    .GetExecutingAssembly()
    .GetTypes()
    .Where(t => !t.IsAbstract
      && !t.IsInterface
      && t.IsClass
      && t.IsPublic
      && !t.IsSealed)
    .ToList();
  // A closed generic type is not defined within either the assembly that defined the open generic type
  // nor the assembly that creates that particular closed type.
  // This is because it can't find metadata for EVERY POSSIBLE closed definition

  private readonly IServiceCollection _collection;

  private readonly Type[] _genericServices = new Type[] {
    typeof(IPortfolioService<,>), // implemented by many types
  };

  private readonly Type[] _transientWithFirstInterfaceImplementations = new Type[] {
    typeof(ProjectRepo),
    typeof(TechnologyRepo),
    typeof(HttpContextAccessor),
    typeof(AuthenticationService),
    typeof(ValidationBehavior<,>),
    //typeof(AuthorizeAdminBehavior<,>),
    typeof(PasswordHasher<User>),
    //typeof(SearcheableBehavior<,>)
  };

  private readonly Type[] _implementations = new Type[] {
    typeof(PortfolioMapper<,,>),
  };

  private readonly Type[] _singletonWithFirstInterfaceImplementations = new Type[] {
    typeof(TokenHandler), // must be a singleton to save all the current refreshToken's
    typeof(FileExtensionContentTypeProvider)
  };

  public PortfolioServicesAggregator(IServiceCollection collection)
  {
    _collection = collection;
    AddPortfolioServices();
  }

  public void AddPortfolioServices()
  {
    AddTransientImplementations();
    AddSingletonImplementations();

    AddAbstractGenericImplementations();
    List<ServiceImplementationPair> genericInterfaceImplementations = FindImplementationsFromGenericInterfaces();
    AddAbstractGenericImplementation(genericInterfaceImplementations);
  }

  private void AddTransientImplementations()
  {
    // generic services implementing generic interfaces must always be open
    foreach (Type implementation in _transientWithFirstInterfaceImplementations)
    {
      var typeToImplement = implementation;
      Type service;
      if (implementation.IsGenericType)
      {
        typeToImplement = typeToImplement.GetGenericTypeDefinition();
        service = typeToImplement.GetInterfaces().First().GetGenericTypeDefinition();
      } else
      {
        service = typeToImplement.GetInterfaces().First();
      }

      _collection.AddTransient(service, typeToImplement);
    }
  }

  private void AddSingletonImplementations()
  {
    foreach (Type implementation in _singletonWithFirstInterfaceImplementations)
    {
      var service = implementation.GetInterfaces().First();
      _collection.AddSingleton(service, implementation);
    }
  }

  private List<ServiceImplementationPair> FindImplementationsFromGenericInterfaces()
  {
    var serviceImplementationPairs = new List<ServiceImplementationPair>();
    // linq methods should always be pure (that's why it's advised to use foreach)
    foreach (Type t in _implementableTypes)
    {
      var implementedClosedGenericInterfaces = t.GetInterfaces().Where(i => i.IsGenericType);

      Type interfaceToImplement = implementedClosedGenericInterfaces.FirstOrDefault(icgi =>
        _genericServices.Contains(icgi.GetGenericTypeDefinition()));

      if (interfaceToImplement != null)
      {
        serviceImplementationPairs.Add(new ServiceImplementationPair(interfaceToImplement, t));
      }
    }
    return serviceImplementationPairs;
  }


  private void AddAbstractGenericImplementation(List<ServiceImplementationPair> genericInterfaceImplementations)
  {
    foreach (ServiceImplementationPair gsip in genericInterfaceImplementations)
    {
      _collection.AddScoped(gsip.Service, gsip.Implementation);
    }
  }

  private void AddAbstractGenericImplementations()
  {
    foreach (var t in _implementations)
    {
      _collection.AddScoped(t);
    }
  }


}
internal readonly struct ServiceImplementationPair
{
  public Type Service { get; }
  public Type Implementation { get; }

  public ServiceImplementationPair(Type serviceType, Type implementationType)
  {
    // a generic isn't usually a type definition
    if (!implementationType.IsClass || implementationType.IsAbstract)
    {
      throw new ArgumentException("Implementation must be an instanciable class");
    }
    // static classes are declared abstract and sealed at the IL level.
    if (serviceType.IsGenericTypeDefinition)
    {
      throw new ArgumentException("Service must be a closed generic type");
    }
    Implementation = implementationType;
    Service = serviceType;
  }
}

public static class ResponseDtoWrapperExtensions
{
  public static void AddPortfolioServices(this IServiceCollection collection)
  {
    new PortfolioServicesAggregator(collection);
  }
}


//var serviceImplementationPairs = new List<ServiceImplementationPair>();
//foreach (Type it in _implementableTypes)
//{
//  it
//  .GetInterfaces()
//  .Where(i => i.IsGenericType)
//  .FirstOrDefault(implementedClosedGenericInterface =>
//    genericInterfacesToImplement.Contains(implementedClosedGenericInterface.GetGenericTypeDefinition())
//  )
//  .ToList()
//  .ForEach(i => serviceImplementationPairs.Add(new ServiceImplementationPair(i, it)));
//}


//public static void AddServices(WebApplicationBuilder builder,
//  IEnumerable<Type> genericInterfacesToImplement,
//  IEnumerable<Type> genericClassesToImplement)
//{
//  if (!genericInterfacesToImplement.All(gi => gi.IsGenericTypeDefinition))
//  {
//    throw new ArgumentException("Interfaces must be generic");
//  }

//  if (!genericClassesToImplement.All(gi => gi.IsGenericTypeDefinition))
//  {
//    throw new ArgumentException("Classes must be generic");
//  }
//  //IEnumerable<Type> genericImplementations = FindGenericImplementations(genericClassesToImplement);
//  //AddGenericImplementations(builder, genericImplementations);

//  //List<ServiceImplementationPair> genericInterfaceImplementations = FindGenericInterfaceImplementations(genericInterfacesToImplement);
//  //AddGenericInterfaceImplementations(builder, genericInterfaceImplementations);
//}

//private static void AddGenericImplementations(WebApplicationBuilder builder, IEnumerable<Type> genericImplementations)
//{
//  foreach (Type t in genericImplementations)
//  {
//    builder.Services.AddScoped(t);
//  }
//}
//private static void AddGenericInterfaceImplementations(WebApplicationBuilder builder, List<ServiceImplementationPair> genericInterfaceImplementations)
//{
//  foreach (ServiceImplementationPair gsip in genericInterfaceImplementations)
//  {
//    builder.Services.AddScoped(gsip.Service, gsip.Implementation);
//  }
//}


//private IEnumerable<Type> FindGenericImplementations()
//{
//  return _implementableTypes.Where(it => it.IsGenericType && _genericImplementations.Contains(it.GetGenericTypeDefinition()));
//}

//private void AddGenericImplementations(IEnumerable<Type> genericImplementations)
//{
//  foreach (Type t in genericImplementations)
//  {
//    // to add a generic class, only the service is needed,
//    // in the shape of the open generic class
//    _collection.AddScoped(t);
//  }
//}




//private void AddGenericInterfaceImplementations(IEnumerable<Type> abstractGenericImplementations)
//{
//  foreach (Type t in abstractGenericImplementations)
//  {
//    // to add a generic class, only the service is needed,
//    // in the shape of the open generic class
//    _collection.AddScoped(t.BaseType, t);
//  }
//}