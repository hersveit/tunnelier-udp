using System;

namespace Tunnelier {
  public class CommanLineArguments {
    private string configFilePath = null;
    private bool isLogging = false;

    public string ConfigFilePath {
      get {
        return this.configFilePath;
      }
    }

    public bool IsLogging {
      get {
        return this.isLogging;
      }
    }

    public CommanLineArguments(string[] args) {
      if (args.Length > 0) {
        for (int i = 0; i < args.Length; i += 1) {
          switch (args[i]) {
            case "-config":
              this.configFilePath = args[i + 1];
              break;
            case "-log":
              this.isLogging = true;
              break;
            default:
              break;
          }
        }
      }
    }
  }
}