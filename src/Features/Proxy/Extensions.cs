using System;
using System.Net;

namespace Tunnelier.Features.Proxy {
  public static class Extensions {
    public static RemoteClient? Find(
      this RemoteClient[] remoteClients,
      IPEndPoint endPoint
    ) {
      foreach(var rc in remoteClients) {
        if (rc.EndPoint.Equals(endPoint)) {
          return rc;
        }
      }
      return null;
    }
  }
}
