using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using TL.CLI.Attributes;

namespace TL.CLI;

class Group : Command {
  public readonly Object Object;

  private readonly Dictionary<PropertyInfo, Option> _options = new();
  private readonly Dictionary<PropertyInfo, Argument> _arguments = new();

  public Group(string name, Object group): base(name) {
    Object = group;
    LoadCommands();
    LoadParamaters();
  }


  private void LoadCommands() {
    var groupType = Object.GetType();
    foreach(var method in groupType.GetMethods()) {
      var attr = method.GetCustomAttribute<CommandAttribute>();
      if(attr is null)
        continue;

      var command = new GroupMethod(attr.Name,this,method);
      AddCommand(command);
    }
  }

    
  private void LoadParamaters() {
    var groupType = Object.GetType();
    foreach(var propertie in groupType.GetProperties()) {
      TryAddParamater(propertie);
    }
  }


  private void TryAddParamater(PropertyInfo info) {
    var optAttr = info.GetCustomAttribute<OptionAttribute>();
    var argAttr = info.GetCustomAttribute<ArgumentAttribute>();
 

    if(optAttr is not null) {
      var opt = optAttr.Build(info);

      // TODO: How to get default value?
      AddGlobalOption(opt);
      _options.Add(info,opt);
    }

    else if(argAttr is not null) {
      var arg = argAttr.Build(info);

      AddArgument(arg);
      _arguments.Add(info,arg);
    }

    // else
    //   throw new Exception();
  }

  public void LoadValues(InvocationContext context) {

    foreach(var kv in _options) {
      var val = context.ParseResult.GetValueForOption(kv.Value);
      kv.Key.SetValue(Object,val);
    }

    foreach(var kv in _arguments) {
      var val = context.ParseResult.GetValueForArgument(kv.Value);
      kv.Key.SetValue(Object,val);
    }
  }
}
