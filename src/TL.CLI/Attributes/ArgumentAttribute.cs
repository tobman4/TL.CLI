using System.CommandLine;
using System.Reflection;

namespace TL.CLI.Attributes;

public class ArgumentAttribute : ParamaterAttribute {
  
  public ArgumentAttribute(string name): base(name) {
  }

  public ArgumentAttribute(string name, string desc): base(name,desc) {
  }


  private Argument GetObject(Type t) {
    var argumentType = typeof(Argument<>).MakeGenericType(t);
    var obj = Activator.CreateInstance(argumentType, new [] { Name, Desc });

    if(obj is not Argument)
      throw new Exception("Wrong type????");

    return (Argument)obj;
  }

  public Argument Build(PropertyInfo info) => GetObject(info.PropertyType);
  // public Argument Build(ParameterInfo info) => GetObject(info.ParameterType);
  public Argument Build(ParameterInfo info) {
    var obj = GetObject(info.ParameterType);
    if(info.HasDefaultValue) {
      obj.SetDefaultValue(info.DefaultValue);
    }

    return obj;
  }
}
