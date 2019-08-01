using System.Threading;

namespace Tunnelier.Shared.Threader {
  public class ThreadController {
    private readonly Thread thread;
    private readonly AutoResetEvent autoEvent = new AutoResetEvent (false);

    public ThreadController (Thread thread) {
      this.thread = thread;
    }

    public void Pause () {
      if (thread.IsAlive) {
        autoEvent.WaitOne ();
      }
    }
     
    public void Resume () {
      if (thread.IsAlive) {
        autoEvent.Set ();
      }
    }

    public void Abort () {
      if (thread.IsAlive) {
        thread.Abort ();
      }
    }

    public void Start () {
      if (!thread.IsAlive) {
        thread.Start ();
      }
    }
  }
}