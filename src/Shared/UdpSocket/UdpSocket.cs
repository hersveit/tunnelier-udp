using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Tunnelier.Services.Logger;

namespace Tunnelier.Shared.UdpSocket {
  public class UdpSocket: IDisposable {
    public delegate void DOnReceiveClient(ClientMessage clientMessage);

    private readonly UdpClient socket;
    private readonly IPEndPoint endPoint;
    private readonly string[] blackList;
    private Thread thread;

    public UdpSocket(
      IPEndPoint endPoint,
      bool connect = false,
      string[] blackList = null
    ) {
      this.blackList = blackList ?? new string[]{};
      this.endPoint = endPoint;

      if (connect) {
        socket = new UdpClient();
        socket.Connect(endPoint);
      } else {
        socket = new UdpClient(endPoint);
      }
    }

    public UdpSocket(int port, bool connect = false, string[] blackList = null) {
      this.blackList = blackList ?? new string[]{};
      this.endPoint = new IPEndPoint(IPAddress.Any, port);

      if (connect) {
        socket = new UdpClient();
        socket.Connect(endPoint);
      } else {
        socket = new UdpClient(endPoint);
      }
    }

    public void Listen(DOnReceiveClient onReceiveClient) {
      ParameterizedThreadStart listenThreadStart = new ParameterizedThreadStart(
        (obj) => ListenThread(obj as DOnReceiveClient)
      );
      thread = new Thread(listenThreadStart);
      thread.Start(onReceiveClient);
    }

    public void Stop() {
      thread.Abort();
    }

    public int Send(byte[] data) {
      return socket.Send(data, data.Length);
    }

    public int Send(byte[] data, IPEndPoint endPoint) {
      return socket.Send(data, data.Length, endPoint);
    }

    public void Dispose() {
      Stop();
      socket.Close();
      socket.Dispose();
    }

    private void ListenThread(DOnReceiveClient onReceiveClient) {
      while(true) {
        IPEndPoint endPoint = new IPEndPoint(0, 0);
        byte[] result = socket.Receive(ref endPoint);
        string endPointIp = endPoint.Address.ToString();

        if (Array.Exists(blackList, ip => endPointIp == ip)) {
          var logger = TunnelierCore.Core.DI.Get<Logger>();
          logger.Info($"Client was blocked by black list: {endPointIp.ToString()}");
          continue;
        }

        onReceiveClient(new ClientMessage(endPoint, result));
      }
    }
  }
}
