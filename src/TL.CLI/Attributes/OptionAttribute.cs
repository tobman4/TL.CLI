using System.CommandLine;
using System.Reflection;

namespace TL.CLI.Attributes;

public class OptionAttribute : ParamaterAttribute {

  public OptionAttribute(string name): base(name) {
  }

  private Option GetObject(Type type) {
    var optionType = typeof(Option<>).MakeGenericType(type);
    var obj = Activator.CreateInstance(optionType, new [] { Name, null });

    if(obj is not Option)
      throw new Exception("Wrong type?????????");

    return (Option)obj;
  }

  public Option Build(PropertyInfo info) => GetObject(info.PropertyType);
  public Option Build(ParameterInfo parameter) {
    var opt = GetObject(parameter.ParameterType);
    if(parameter.HasDefaultValue) {
      opt.SetDefaultValue(parameter.DefaultValue);
    } else {
      opt.IsRequired = true;
    }

    return opt;
  }
}
