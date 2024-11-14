using TL.CLI.Attributes;

public class Say {

  [Command("hello")]
  public void Hello([Option("--name")]string name = "Tom") {
    Console.WriteLine($"Hello {name}");
  }

}
