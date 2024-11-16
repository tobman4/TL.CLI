namespace TL.CLI.Attributes;


[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property)]
public class ParamaterAttribute : Attribute {

  public readonly string Name;
  public readonly string Desc = "";

  public ParamaterAttribute(string name) {
    Name = name;
  }

  public ParamaterAttribute(string name, string desc) {
    Name = name;
    Desc = desc;
  }

}
