using System;

namespace Tunnelier {
  public class CommanLineArguments {
    private string configFilePath = null;

    public string ConfigFilePath {
      get {
        return this.configFilePath;
      }
    }

    public CommanLineArguments(string[] args) {
      if (args.Length > 0) {
        for (int i = 0; i < args.Length; i += 1) {
          switch (args[i]) {
            case "-config":
              this.configFilePath = args[i + 1];
              break;
            default:
              break;
          }
        }
      }
    }
  }
}