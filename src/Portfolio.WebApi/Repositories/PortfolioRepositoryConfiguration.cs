using Portfolio.WebApi.Mapper;
using System.Reflection;

namespace Portfolio.WebApi.Repositories;

public static class PortfolioRepositoryConfiguration
{
  // sealed can also be used to cut the virtual chain on a property or method (because with override also applies virtual)
  private static readonly IEnumerable<Type> _implementableTypes = Assembly
    .GetExecutingAssembly()
    .GetTypes()
    .Where(t => !t.IsAbstract
    && !t.IsInterface
    && t.IsClass
    && !t.IsGenericParameter
    && t.IsPublic
    && !t.IsSealed)
    .ToList();
  // A closed generic type is not defined within either the assembly that defined the open generic type,
  // nor the assembly that creates that particular closed type.
  // This is because it can't find metadata for EVERY POSSIBLE closed definition
  public static void Configure(WebApplicationBuilder builder,
    IEnumerable<Type> genericInterfacesToImplement,
    IEnumerable<Type> genericClassesToImplement)
  {
    if (!genericInterfacesToImplement.All(gi => gi.IsGenericTypeDefinition))
    {
      throw new ArgumentException("Interfaces must be generic");
    }

    if (!genericClassesToImplement.All(gi => gi.IsGenericTypeDefinition))
    {
      throw new ArgumentException("Classes must be generic");
    }
    IEnumerable<Type> genericImplementations = FindGenericImplementations(genericClassesToImplement);
    AddGenericImplementations(builder, genericImplementations);

    List<ServiceImplementationPair> genericInterfaceImplementations = FindGenericInterfaceImplementations(genericInterfacesToImplement);
    AddGenericInterfaceImplementations(builder, genericInterfaceImplementations);
  }

  private static void AddGenericImplementations(WebApplicationBuilder builder, IEnumerable<Type> genericImplementations)
  {
    foreach (Type t in genericImplementations)
    {
      builder.Services.AddScoped(t);
    }
  }

  private static IEnumerable<Type> FindGenericImplementations(IEnumerable<Type> genericClassesToImplement)
  {
    return _implementableTypes.Where(it => it.IsGenericType && genericClassesToImplement.Contains(it.GetGenericTypeDefinition()));
  }

  private static void AddGenericInterfaceImplementations(WebApplicationBuilder builder, List<ServiceImplementationPair> genericInterfaceImplementations)
  {
    foreach (ServiceImplementationPair gsip in genericInterfaceImplementations)
    {
      builder.Services.AddScoped(gsip.Service, gsip.Implementation);
    }
  }
  private static List<ServiceImplementationPair> FindGenericInterfaceImplementations(IEnumerable<Type> genericInterfacesToImplement)
  {
    var serviceImplementationPairs = new List<ServiceImplementationPair>();
    // linq methods should always be pure (that's why it's advised to use foreach)
    foreach (Type t in _implementableTypes)
    {
      var implementedClosedGenericInterfaces = t.GetInterfaces().Where(i => i.IsGenericType);

      Type interfaceToImplement = implementedClosedGenericInterfaces.FirstOrDefault(icgi =>
        genericInterfacesToImplement.Contains(icgi.GetGenericTypeDefinition())
      );
      if (interfaceToImplement != null)
      {
        serviceImplementationPairs.Add(new ServiceImplementationPair(interfaceToImplement, t));
      }
    }
    return serviceImplementationPairs;
  }

  private struct ServiceImplementationPair
  {
    public Type Service { get; }
    public Type Implementation { get; }

    public ServiceImplementationPair(Type serviceType, Type implementationType)
    {
      // a generic isn't usually a type definition
      //if (implementationType.IsGenericType && implementationType.IsGenericTypeDefinition)
      //{
      //  throw new ArgumentException("Implementation must be a closed generic type");
      //}
      //// static classes are declared abstract and sealed at the IL level.
      //if (!implementationType.IsClass || implementationType.IsAbstract)
      //{
      //  throw new ArgumentException("Implementation must be an instanciable class");
      //}
      //if (serviceType.IsGenericType && serviceType.IsGenericTypeDefinition)
      //{
      //  throw new ArgumentException("Service must be closed generic");
      //}
      Implementation = implementationType;
      Service = serviceType;
    }
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