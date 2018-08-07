using System;
using System.Collections.Generic;
using System.Text;

namespace Tunnelier {
  public static class Logger {
    public static void Log(string msg) {
      Console.ResetColor();
      Console.WriteLine(msg);
    }

    public static void Info(string msg) {
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine(msg);
      Console.ResetColor();
    }

    public static void Error(int code, string msg) {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(string.Format("{0}: {1}", code, msg));
      Console.ResetColor();
    }
  }
}
