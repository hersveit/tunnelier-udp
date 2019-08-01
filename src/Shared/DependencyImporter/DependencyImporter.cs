using System;
using System.Collections.Generic;

namespace Tunnelier.Shared.DependencyImporter {
  public class DependencyImporter {
    public delegate T ConstructorDelegate<T>();
    private readonly Dictionary<Type, object> deps = new Dictionary<Type, object>(){};

    public T Get<T>() {
      if (deps.ContainsKey(typeof(T))) {
        return (deps[typeof(T)] as Dependency<T>).Instance;
      }
      return default(T);
    }

    public void Set<T>(ConstructorDelegate<T> constructor) where T : class {
      if (deps.ContainsKey(typeof(T))) {
        return;
      }
      var dep = new Dependency<T>(constructor());
      deps.Add(typeof(T), dep);
    }
  }
}
