// See https://aka.ms/new-console-template for more information
using System.CommandLine;

Console.WriteLine("Hello, World!");

RootCommand root = new();
root.SetHandler(() => Console.WriteLine("ROOT"));

Command test = new("test");
test.SetHandler(() => Console.WriteLine("TEST"));
root.Add(test);


await root.InvokeAsync(args);

