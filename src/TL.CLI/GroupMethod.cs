using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;
using TL.CLI.Attributes;

namespace TL.CLI;

class GroupMethod : Command {
  private readonly GroupCommand _group;
  private readonly MethodInfo _method;
  private readonly Dictionary<ParameterInfo, Option> _options = new();
  private readonly Dictionary<ParameterInfo, Argument> _arguments = new();

  public GroupMethod(string name, GroupCommand group, MethodInfo method): base(name) {
    _group = group;
    _method = method;
    
    this.SetHandler(InvokeAsync);
    
    foreach(var p in _method.GetParameters())
      AddParamater(p);
  }

  private void AddParamater(ParameterInfo parameter) {
    var optionAttribute = parameter.GetCustomAttribute<OptionAttribute>();
    var argumentAttribute = parameter.GetCustomAttribute<ArgumentAttribute>();


    if(optionAttribute is not null) {
      var option = optionAttribute.Build(parameter);
      
      AddOption(option);
      _options.Add(parameter, option);
    }

    else if(argumentAttribute is not null) {
      var argument = argumentAttribute.Build(parameter);
      
      AddArgument(argument);
      _arguments.Add(parameter, argument);
    }
    
    else
      throw new NotImplementedException("Parameter need to be argument or option");
  }

  public object? GetValue(ParameterInfo parameter, InvocationContext context) {
    if(_options.ContainsKey(parameter))
      return context.ParseResult.GetValueForOption(_options[parameter]);
    
    else if(_arguments.ContainsKey(parameter))
      return context.ParseResult.GetValueForArgument(_arguments[parameter]);

    throw new Exception("Parameter not argument or option");
  }

  public async Task InvokeAsync(InvocationContext context) {
    _group.LoadValues(context);
    var values = _method.GetParameters()
      .Select(e => GetValue(e,context))
      .ToArray();
    
    var result = _method.Invoke(_group.Object, values);

    await Task.Delay(1); 
  }

}
