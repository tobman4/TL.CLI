using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using TL.CLI.Attributes;

namespace TL.CLI;

class Group : Command {
  private readonly Object _host;
  private Type _hostType => _host.GetType();

  private readonly Dictionary<PropertyInfo, Option> _options = new();
  private readonly Dictionary<PropertyInfo, Argument> _arguments = new();
  private readonly Dictionary<Type, MethodInfo> _exceptionHandlers = new();

  public Group(string name, Object group): base(name) {
    _host = group;
    LoadCommands();
    LoadParamaters();
    LoadHandlers();
  }


  private void LoadCommands() {
    foreach(var method in _hostType.GetMethods()) {
      var attr = method.GetCustomAttribute<CommandAttribute>();
      if(attr is null)
        continue;

      // var command = new GroupMethod(attr.Name,this,method);
      var command = new MethodCommand(method, _host);
      command.AddPreAction(LoadValues);
      command.OnError(TryHandleException);
      AddCommand(command);
    }
  }

    
  private void LoadParamaters() {
    foreach(var propertie in _hostType.GetProperties()) {
      TryAddParamater(propertie);
    }
  }

  private void LoadHandlers() {
    foreach(var method in _hostType.GetMethods()) {
      var attr = method!.GetCustomAttribute<ExceptionHandlerAttribute>();
      if(attr is null)
        continue;

      _exceptionHandlers.Add(attr.ExceptionType,method);
    }
  }

  private void TryAddParamater(PropertyInfo info) {
    var optAttr = info.GetCustomAttribute<OptionAttribute>();
    var argAttr = info.GetCustomAttribute<ArgumentAttribute>();
 

    if(optAttr is not null) {
      var opt = optAttr.Build(info, _host);

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
      kv.Key.SetValue(_host,val);
    }

    foreach(var kv in _arguments) {
      var val = context.ParseResult.GetValueForArgument(kv.Value);
      kv.Key.SetValue(_host,val);
    }
  }


  public void TryHandleException(Exception err) {
    if(!_exceptionHandlers.ContainsKey(err.GetType()))
      return;

    var method = _exceptionHandlers[err.GetType()]!;
    method.Invoke(_host,new Object[] { err });
  }
}
