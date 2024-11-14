namespace TL.CLI.Attributes;


[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class ParamaterAttribute : Attribute {

  public readonly string Name;

  public ParamaterAttribute(string name) {
    Name = name;
  }

}
