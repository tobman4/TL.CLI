using System.CommandLine;
using System.Reflection;

namespace TL.CLI.Attributes;

public class OptionAttribute : ParamaterAttribute {

  public OptionAttribute(string name): base(name) {
  }

  public OptionAttribute(string name, string desc): base(name, desc) {
  }

  private Option GetObject(Type type) {
    var optionType = typeof(Option<>).MakeGenericType(type);
    var obj = Activator.CreateInstance(optionType, new [] { Name, Desc });

    if(obj is not Option)
      throw new Exception("Wrong type?????????");

    return (Option)obj;
  }

  // public Option Build(PropertyInfo info) => GetObject(info.PropertyType);
  public Option Build(PropertyInfo info) {
    var obj = GetObject(info.PropertyType);
    obj.IsRequired = true;

    return obj;
  }

  public Option Build(PropertyInfo info, Object obj) {
    if(!info.CanWrite)
      throw new Exception("Option need to have public set method");

    var option = GetObject(info.PropertyType);
    
    Console.WriteLine($"{info.Name} -> Can read: {info.CanRead}. Can set {info.CanWrite}");
    if(info.CanRead)
      option.SetDefaultValue(info.GetValue(obj));
    else
      option.IsRequired = true;


    return option;
  }

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
