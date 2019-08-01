using System.Net;

namespace Tunnelier.Shared.UdpSocket {
  public struct ClientMessage {
    private readonly IPEndPoint endPoint;
    private readonly byte[] message;

    public IPEndPoint EndPoint => endPoint;
    public byte[] Message => message;

    public ClientMessage(IPEndPoint endPoint, byte[] message) {
      this.endPoint = endPoint;
      this.message = message;
    }
  }
}