using TL.CLI.Attributes;

[Command("ss")]
public class Say {

  [Option("-c")]
  public int Count { get; set; } = 1;

  [Command("hello")]
  public void Hello([Argument("name", "Who to say hello to")]string name = "Tom") {
    for(int i = 0; i < Count; i++)
      Console.WriteLine($"Hello {name}");
  }
  
  [Command("bye")]
  public async Task Bye() {
    for(int i = 0; i < Count; i++) {
      Console.WriteLine("Bye :,(");
      await Task.Delay(250);
    }
  }

}
