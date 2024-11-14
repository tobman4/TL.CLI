# TL.CLI


## Plan


```c#
[Group("test")]
class CommandGroup {

    [Option("--name")]
    public Name = null!;

    [Command("hello")]
    public async Task SayHello() {
        Console.WriteLine($"Hello {Name}");
    }

    public async Task SayBye([Option("--kys")]bool doKys) {
        if(doKys)
            Console.WriteLine("KYS!");

        Console.WriteLine($"Bye {Name}");
    }
}

```
