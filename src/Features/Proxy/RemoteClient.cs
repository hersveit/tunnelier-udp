using System.Net;
using Tunnelier.Shared.UdpSocket;

namespace Tunnelier.Features.Proxy {
  public struct RemoteClient {
    private readonly UdpSocket redirectSocket;
    private readonly IPEndPoint endPoint;

    public UdpSocket RedirectSocket => redirectSocket;
    public IPEndPoint EndPoint => endPoint;

    public RemoteClient(
      UdpSocket redirectSocket,
      IPEndPoint endPoint
    ) {
      this.redirectSocket = redirectSocket;
      this.endPoint = endPoint;
    }
  }
}
