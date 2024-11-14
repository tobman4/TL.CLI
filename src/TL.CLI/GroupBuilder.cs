using System.CommandLine;
using System.Reflection;
using TL.CLI.Attributes;

namespace TL.CLI;

public class GroupCommand : Command {
  public readonly Object Object;

  private readonly Dictionary<PropertyInfo, Option> _propOptions = new();


  public GroupCommand(string name, Object group): base(name) {
    Object = group;
    
    var groupType = group.GetType();
    foreach(var method in groupType.GetMethods()) {
      var attr = method.GetCustomAttribute<CommandAttribute>();
      if(attr is null)
        continue;

      var command = new GroupMethod(attr.Name,this,method);
      AddCommand(command);
    }

  }
}
