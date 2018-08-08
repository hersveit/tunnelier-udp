﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Tunnelier {
  class Program {
    protected static UdpClient tunnelUdp;
    protected static Settings settings;

    protected struct SClient {
      public UdpClient udpClient;
      public IPEndPoint endPoint;
      public Thread thread;
    }

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

    static SClient[] clients = new SClient[0];
    static void TunnelListener() {
      IPEndPoint tunnelIpEndPoint = new IPEndPoint(IPAddress.Any, settings.Collection.InputPort);
      tunnelUdp = new UdpClient(tunnelIpEndPoint);
      while (true) {
        IPEndPoint gameEndPoint = new IPEndPoint(0, 0);
        byte[] result = tunnelUdp.Receive(ref gameEndPoint);
        string msg = Encoding.ASCII.GetString(result, 0, result.Length);
        var udp = GetUdpClient(gameEndPoint, ref clients);
        udp.Send(result, result.Length);
      }
    }

    static void udpServerThread(object obj) {
      while (true) {
        SClient client = (SClient)obj;
        IPEndPoint source = new IPEndPoint(0, 0);
        byte[] result = client.udpClient.Receive(ref source);
        tunnelUdp.Send(result, result.Length, client.endPoint);
      }
    }

    static UdpClient GetUdpClient(IPEndPoint endPoint, ref SClient[] clients) {
      for (int i = 0; i < clients.Length; i += 1) {
        if (clients[i].endPoint.Equals(endPoint)) {
          return clients[i].udpClient;
        }
      }
      IPEndPoint newIpServerPoint = new IPEndPoint(IPAddress.Parse(settings.Collection.RedirectIp), settings.Collection.RedirectPort);
      UdpClient udp = new UdpClient();
      udp.Connect(newIpServerPoint);
      ParameterizedThreadStart udpServerThreadStart = new ParameterizedThreadStart(udpServerThread);
      var thread = new Thread(udpServerThreadStart);
      SClient sc = new SClient() { endPoint = endPoint, udpClient = udp, thread = thread };
      clients = clients.Concat(new SClient[] { sc });
      thread.Start(sc);
      return udp;
    }
  }
}
