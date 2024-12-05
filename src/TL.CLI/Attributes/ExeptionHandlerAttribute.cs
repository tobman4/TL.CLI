namespace TL.CLI.Attributes;

public class ExceptionHandlerAttribute : Attribute {
  public readonly Type ExceptionType;

  public ExceptionHandlerAttribute(Type type) {
    ExceptionType = type;
  }
}
