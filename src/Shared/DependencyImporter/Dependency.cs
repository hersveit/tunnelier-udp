using System;

namespace Tunnelier.Shared.DependencyImporter {
  public class Dependency<T> {
    private readonly Type type;
    private readonly T instance;

    public Dependency(T instance) {
      this.instance = instance;
      this.type = typeof(T);
    }

    public T Instance => instance;

    public Type Type => type;
  }
}
