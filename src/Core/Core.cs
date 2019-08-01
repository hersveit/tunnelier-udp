using Tunnelier.Shared.DependencyImporter;
using Tunnelier.Services.Logger;
using Tunnelier.Services.Config;

namespace Tunnelier.TunnelierCore {
  public static class Core {
    private static readonly DependencyImporter di = new DependencyImporter();
    private static bool isInitialized = false;

    public static DependencyImporter DI => di;

    public static void Initialize() {
      if (isInitialized) {
        return;
      }

      InitializeDependencies();

      isInitialized = true;
    }

    private static void InitializeDependencies() {
      di.Set<Config>(() => new Config());
      di.Set<Logger>(() => new Logger(ELogDestination.All));
    }
  }
}
