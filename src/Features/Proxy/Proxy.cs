using System;
using System.Net;
using Tunnelier.TunnelierCore;
using Tunnelier.Shared.UdpSocket;
using Tunnelier.Services.Config;
using Tunnelier.Services.Logger;

namespace Tunnelier.Features.Proxy {
  public class Proxy: IDisposable {
    private readonly UdpSocket socket;
    private readonly Logger logger;

    private RemoteClient[] remoteClients = new RemoteClient[]{};

    public Proxy() {
      var config = Core.DI.Get<Config>();
      socket = new UdpSocket(
        config.Collection.InputPort,
        false,
        config.Collection.BlackList
      );
      socket.Listen(OnReceiveClient);

      logger = Core.DI.Get<Logger>();
      logger.Info(
        $"Proxy server was enabled on port {config.Collection.InputPort}. " +
        $"Redirecting to {config.Collection.RedirectIp}:{config.Collection.RedirectPort}"
      );
    }

    public void Dispose() {
      foreach(var rc in remoteClients) {
        rc.RedirectSocket.Dispose();
      }
    }

    private void OnReceiveClient(ClientMessage clientMessage) {
      RemoteClient client = remoteClients.Find(clientMessage.EndPoint)
        ?? MakeRemoteClient(clientMessage);
      var result = client.RedirectSocket.Send(clientMessage.Message);
    }

    private RemoteClient MakeRemoteClient(ClientMessage clientMessage) {
      logger.Info($"New client was received: {clientMessage.EndPoint.ToString()}");

      var config = Core.DI.Get<Config>();
      IPEndPoint redirectEndPoint = new IPEndPoint(
        IPAddress.Parse(config.Collection.RedirectIp),
        config.Collection.RedirectPort
      );

      UdpSocket redirectSocket = new UdpSocket(redirectEndPoint, true);
      RemoteClient remoteClient = new RemoteClient(redirectSocket, clientMessage.EndPoint);
      remoteClients = remoteClients.Concat(new RemoteClient[]{ remoteClient });
      ListenRedirectSocket(remoteClient);

      return remoteClient;
    }

    private void ListenRedirectSocket(RemoteClient client) {
      logger.Info(
        $"Start new listener for redirect endpoint for client {client.EndPoint.ToString()}"
      );
      client.RedirectSocket.Listen(
        (ClientMessage cm) => socket.Send(cm.Message, client.EndPoint)
      );
    }
  }
}
