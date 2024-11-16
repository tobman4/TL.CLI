using TL.CLI.Attributes;

public class Say {

  [Argument("count")]
  public int Count { get; set; } = 1;

  [Command("hello")]
  public void Hello([Argument("name", "Who to say hello to")]string name = "Tom") {
    for(int i = 0; i < Count; i++)
      Console.WriteLine($"Hello {name}");
  }
  
  [Command("bye")]
  public void Bye() {
    for(int i = 0; i < Count; i++)
      Console.WriteLine("Bye :,(");
  }

}
