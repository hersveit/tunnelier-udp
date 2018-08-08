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
    private delegate void ParameterizedDelegate(object obj);

    private static UdpClient inputSocket;
    private static Settings settings;
    private static RemoteClient[] remoteClients = new RemoteClient[0];

    static void Main(string[] args) {
      CommanLineArguments arguments = new CommanLineArguments(args);
      settings = new Settings(arguments.ConfigFilePath ?? "config.xml");

      ThreadStart InputSocketListenerStart = new ThreadStart(InputSocketListener);
      Thread thread = new Thread(InputSocketListenerStart);
      thread.Start();

      Logger.Info("Press 'Q' to exit");
      while (true) {
        if (Console.ReadKey().Key == ConsoleKey.Q) {
          Environment.Exit(0);
        }
      }
    }

    private static void InputSocketListener() {
      IPEndPoint tunnelIpEndPoint = new IPEndPoint(IPAddress.Any, settings.Collection.InputPort);
      inputSocket = new UdpClient(tunnelIpEndPoint);

      while (true) {
        IPEndPoint gameEndPoint = new IPEndPoint(0, 0);
        byte[] result = inputSocket.Receive(ref gameEndPoint);
        RemoteClient remoteClient = remoteClients.Find(gameEndPoint) ?? ApplyRemoteClient(gameEndPoint, redirectSocketListener, ref remoteClients);
        remoteClient.redirectSocket.Send(result, result.Length);
      }
    }

    private static void redirectSocketListener(object obj) {
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

      ParameterizedThreadStart redirectSocketListenerStart = new ParameterizedThreadStart(callback);
      var thread = new Thread(redirectSocketListenerStart);

      RemoteClient remoteClient = new RemoteClient() { redirectSocket = redirectSocket, thread = thread, endPoint = endPoint };
      thread.Start(remoteClient);
      remoteClients = remoteClients.Concat(new RemoteClient[] { remoteClient });
      return remoteClient;
    }
  }
}
