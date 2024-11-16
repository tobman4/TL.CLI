namespace TL.CLI.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class CommandAttribute : Attribute {

  public readonly string Name;

  public CommandAttribute(string name) {
    Name = name;
  }
}
