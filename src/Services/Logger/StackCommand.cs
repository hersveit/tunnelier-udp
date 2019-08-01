namespace Tunnelier.Services.Logger {
  public class StackCommand {
    private readonly string message;
    private readonly ELogDestination logDestination;

    public string Message => message;

    public ELogDestination LogDestination => logDestination;

    public StackCommand(string message, ELogDestination logDestination) {
      this.message = message;
      this.logDestination = logDestination;
    }
  }
}
