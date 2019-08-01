using System.Threading;
using Tunnelier.TunnelierCore;
using Tunnelier.Features.Proxy;

namespace Tunnelier {
  class Program {
    static void Main(string[] args) {
      Core.Initialize();
      Proxy proxy = new Proxy();
      Thread.Sleep(-1);
    }
  }
}
