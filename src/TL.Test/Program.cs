using TL.CLI;

Console.WriteLine("Hello, World!");
var app = new App();
app.AddGroup<Say>();

await app.InvokeAsync(args);
