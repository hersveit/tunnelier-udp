using System;
using System.Collections.Generic;
using System.IO;
using Tunnelier.Shared.Threader;

namespace Tunnelier.Services.Logger {
  public class Logger {
    private readonly string path;
    private readonly ELogDestination logDestination;
    private readonly Threader logThreader;

    private readonly Queue<StackCommand> commandsQueue = new Queue<StackCommand>();

    private readonly Dictionary<ELogType, string> logTypes = new Dictionary<ELogType, string>(){
        {ELogType.Error, "Error"},
        {ELogType.Info, "Info"},
        {ELogType.Log, "Log"},
        {ELogType.Warning, "Warning"},
      };

    public Logger (ELogDestination logDestination) {
      this.logDestination = logDestination;

      if (logDestination == ELogDestination.File || logDestination == ELogDestination.All) {
        string date = GetLocalDateString();
        path = Directory.GetCurrentDirectory() + $"/logs/{date}.log";
        Directory.CreateDirectory (new FileInfo (path).Directory.FullName);
      }

      logThreader = new Threader (LogThread);
    }
   
    public void Error(string message) {
      PushCommand(MakeLogMessage(message, ELogType.Error));
    }

    public void Info(string message) {
      PushCommand(MakeLogMessage(message, ELogType.Info));
    }

    public void Log(string message) {
      PushCommand(MakeLogMessage(message, ELogType.Log));
    }

    public void Warning(string message) {
      PushCommand(MakeLogMessage(message, ELogType.Warning));
    }

    private void PushCommand(string message) {
      commandsQueue.Enqueue(new StackCommand(message, logDestination));
      logThreader.Controller.Resume();
    }

    private void PrintToConsole(string message) {
      Console.WriteLine (message);
    }

    private void PrintToFile(string message) {
      using (StreamWriter sw = File.AppendText(path)) {
        sw.WriteLine(message);
      }
    }

    private void LogThread (ThreadController controller) {
      while (true) {
        if (commandsQueue.Count > 0) {
          var command = commandsQueue.Dequeue();
          switch(command.LogDestination) {
            case ELogDestination.File:
              PrintToFile(command.Message);
              break;
            case ELogDestination.Console:
              PrintToConsole(command.Message);
              break;
            case ELogDestination.All:
              PrintToConsole(command.Message);
              PrintToFile(command.Message);
              break;
          }
        } else {
          controller.Pause();
        }
      }
    }

    private string MakeLogMessage(string message, ELogType logType) {
      string date = GetLocalDateString();
      return $"[{date} {logTypes[logType]}]: {message}";
    }

    private string GetLocalDateString() {
      return DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
    }
  }
}
