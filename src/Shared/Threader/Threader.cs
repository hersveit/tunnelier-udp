using System.Threading;

namespace Tunnelier.Shared.Threader {
  public class Threader {
    public delegate void ThreaderDelegate (ThreadController threadController);

    private readonly ThreaderDelegate action;
    private readonly ThreadController controller;
    private readonly Thread thread;

    public ThreadController Controller => controller;

    public Threader (ThreaderDelegate action) {
      this.action = action;

      var threadStart = new ParameterizedThreadStart (
        (obj) => this.action (obj as ThreadController)
      );
      thread = new Thread (threadStart);

      controller = new ThreadController (thread);
      thread.Start (controller);
    }
  }
}
