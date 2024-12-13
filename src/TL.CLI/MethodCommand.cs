using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using TL.CLI.Attributes;

namespace TL.CLI;

public delegate void PreAction(InvocationContext context);

class MethodCommand : Command {
  
  private readonly MethodInfo _method;
  private readonly Object? _host;

  private readonly List<PreAction> _preActions = new();
  private readonly Dictionary<ParameterInfo, Option> _options = new();
  private readonly Dictionary<ParameterInfo, Argument> _arguments = new();

  public MethodCommand(MethodInfo method) : base("XXX") {
    _method = method;
    LoadParameters();
    this.SetHandler(InvokeAsync);

    if(_method.GetCustomAttribute<CommandAttribute>() is CommandAttribute attr)
      this.Name = attr.Name;
    else 
      this.Name = _method.Name;
  }

  public MethodCommand(MethodInfo method, Object host) : this(method) {
    _host = host;
  }

  public void AddPreAction(PreAction action) =>
    _preActions.Add(action);

  private void LoadParameters() {
    foreach(var parameter in _method.GetParameters()) {
      var optattr = parameter.GetCustomAttribute<OptionAttribute>();
      var argAttr = parameter.GetCustomAttribute<ArgumentAttribute>();

      if(optattr is not null) {
        var opt = optattr.Build(parameter);
        AddOption(opt);
        _options.Add(parameter, opt);
      }

      else if(argAttr is not null) {
        var arg = argAttr.Build(parameter);
        AddArgument(arg);
        _arguments.Add(parameter, arg);
      }

      else
        throw new Exception("Parameter need to be argument or option");
    }
  }

  private Object?[] GetParameters(InvocationContext context) {
    var parameters = new List<Object?>();
    foreach(var param in _method.GetParameters()) {
      
      if(_arguments.ContainsKey(param)) {
        var arg = _arguments[param];
        parameters.Add(context.ParseResult.GetValueForArgument(arg));
      }

      else if(_options.ContainsKey(param)) {
        var opt = _options[param];
        parameters.Add(context.ParseResult.GetValueForOption(opt));
      }

    }

    return parameters.ToArray();
  }

  private async Task InvokeAsync(InvocationContext context) {
    foreach(var action in _preActions)
      action(context);

    var parameters = GetParameters(context);
    var result = _method.Invoke(_host, parameters);

    if(result is Task t)
      await t;
  }

}
