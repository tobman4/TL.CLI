using System.CommandLine;

namespace TL.CLI;

public class App {


  private readonly RootCommand _root;

  public App() {
    _root = new();
  }

  public App(string description) {
    _root = new(description);
  }

  public App(RootCommand root) {
    _root = root;
  }


  public void AddGroup<T>() {
    string name = typeof(T).Name;
    var groupObject = Activator.CreateInstance<T>();
    if(groupObject is null)
      throw new Exception($"Faild to create command group: {name}");

    var group = new Group(name, groupObject);
    _root.Add(group);
  }

  // public void AddGroup<T>(T group) {
  //   _root.Add(group);
  // }

  public async Task InvokeAsync(string[] args) =>
    await _root.InvokeAsync(args);
}
