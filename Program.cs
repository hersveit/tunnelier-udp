using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Tunnelier {

  public struct RemoteClient {
    public UdpClient redirectSocket;
    public IPEndPoint endPoint;
    public Thread thread;
  }

  class Program {
    protected delegate void ParameterizedDelegate(object obj);

    protected static UdpClient inputSocket;
    protected static Settings settings;
    protected static RemoteClient[] remoteClients = new RemoteClient[0];

    static void Main(string[] args) {
      string configFilePath = args.Length > 0 ? args[0] : "config.xml";
      settings = new Settings(configFilePath);

      ThreadStart tunnelThreadStart = new ThreadStart(TunnelListener);
      Thread tunnelThread = new Thread(tunnelThreadStart);
      tunnelThread.Start();

      Logger.Info("Press Q to exit");
      while (true) {
        if (Console.ReadKey().Key == ConsoleKey.Q) {
          Environment.Exit(0);
        }
      }
    }

    private static void TunnelListener() {
      IPEndPoint tunnelIpEndPoint = new IPEndPoint(IPAddress.Any, settings.Collection.InputPort);
      inputSocket = new UdpClient(tunnelIpEndPoint);
      while (true) {
        IPEndPoint gameEndPoint = new IPEndPoint(0, 0);
        byte[] result = inputSocket.Receive(ref gameEndPoint);
        RemoteClient remoteClient = remoteClients.Find(gameEndPoint) ?? ApplyRemoteClient(gameEndPoint, udpServerThread, ref remoteClients);
        remoteClient.redirectSocket.Send(result, result.Length);
      }
    }

    private static void udpServerThread(object obj) {
      while (true) {
        RemoteClient remoteClient = (RemoteClient)obj;
        IPEndPoint source = new IPEndPoint(0, 0);
        byte[] result = remoteClient.redirectSocket.Receive(ref source);
        inputSocket.Send(result, result.Length, remoteClient.endPoint);
      }
    }

    private static RemoteClient ApplyRemoteClient(IPEndPoint endPoint, ParameterizedDelegate callback, ref RemoteClient[] remoteClients) {
      IPEndPoint redirectEndPoint = new IPEndPoint(IPAddress.Parse(settings.Collection.RedirectIp), settings.Collection.RedirectPort);
      UdpClient redirectSocket = new UdpClient();
      redirectSocket.Connect(redirectEndPoint);
      ParameterizedThreadStart udpServerThreadStart = new ParameterizedThreadStart(callback);
      var thread = new Thread(udpServerThreadStart);
      RemoteClient remoteClient = new RemoteClient() { redirectSocket = redirectSocket, thread = thread, endPoint = endPoint };
      thread.Start(remoteClient);
      remoteClients = remoteClients.Concat(new RemoteClient[] { remoteClient });
      return remoteClient;
    }
  }
}
