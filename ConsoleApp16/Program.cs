// See https://aka.ms/new-console-template for more information


#if NET_CORE
using System.Runtime.Loader;
#endif
using LibraryWithType;

if (args.Length == 0)
    throw new ArgumentException("Number of threads not specified");

var threadsToUse = int.Parse(args[0]);

var threads = new Thread[threadsToUse];
for (int i = 0; i < threads.Length; i++)
    threads[i] = new Thread(Worker);

for (int i = 0; i < threads.Length; i++)
    threads[i].Start();

for (int i = 0; i < threads.Length; i++)
    threads[i].Join();

static void Worker()
{
    var asm1 = typeof(Class1).Assembly;
    var asm2 = typeof(Class1).Assembly;

    if (asm1 != asm2)
    {
        Console.WriteLine("Assemblies were not equal!!!");
        Console.WriteLine($"asm1.GetHashCode() = {asm1.GetHashCode()}");
        Console.WriteLine($"asm2.GetHashCode() = {asm2.GetHashCode()}");

#if NET_CORE
        var alcAssemblies = AssemblyLoadContext.Default.Assemblies.ToArray();

        var asm1InDefaultALC = alcAssemblies.Contains(asm1);
        var asm2InDefaultALC = alcAssemblies.Contains(asm2);
        
        Console.WriteLine($"Default ALC contains asm1 = {asm1InDefaultALC}");
        Console.WriteLine($"Default ALC contains asm2 = {asm2InDefaultALC}");
#endif
        Environment.Exit(1);
    }
}



