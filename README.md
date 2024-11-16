# TL.CLI


## How use?


```c#
# Say.cs

using TL.CLI.Attributes;

public class Say {

  [Option("count")]
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
```

```c#
# Program.cs

using TL.CLI;

Console.WriteLine("Hello, World!");
var app = new App();
app.AddGroup<Say>();

await app.InvokeAsync(args);
```
