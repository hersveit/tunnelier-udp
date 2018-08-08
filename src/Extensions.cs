using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Tunnelier {
  public static class Extensions {
    public static T[] Concat<T>(this T[] array1, T[] array2) {
      int len = array1.Length;
      Array.Resize<T>(ref array1, array1.Length + array2.Length);
      Array.Copy(array2, 0, array1, len, array2.Length);
      return array1;
    }

    public static string RemoveLineEndings(this string value) {
      if (String.IsNullOrEmpty(value)) {
        return value;
      }
      string lineSeparator = ((char)0x2028).ToString();
      string paragraphSeparator = ((char)0x2029).ToString();

      return value.Replace("\r\n", string.Empty)
        .Replace("\n", string.Empty)
        .Replace("\r", string.Empty)
        .Replace(lineSeparator, string.Empty)
        .Replace(paragraphSeparator, string.Empty);
    }
  }

  public static class RemoteClientExtensions {
    public static RemoteClient? Find(this RemoteClient[] remoteClients, IPEndPoint endPoint) {
      for (int i = 0; i < remoteClients.Length; i += 1) {
        if (remoteClients[i].endPoint.Equals(endPoint)) {
          return remoteClients[i];
        }
      }
      return null;
    }
  }
}
