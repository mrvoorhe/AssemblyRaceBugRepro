// See https://aka.ms/new-console-template for more information

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;


// The issue would normally happen within a few hundred runs, sometimes less than 100
// Raise if it doesn't happen
const int quitAfter = 1000;

// I was able to repro on a 32 core / 64 thread windows machine machine and an M1 mac pro
// I tried a number of thread values.  I was able to repro with values ranging from 10-256.
const int threadsToUseInChildProcess = 64;

var ps = new ProcessStartInfo(GetPathToOtherProcess(), threadsToUseInChildProcess.ToString());
ps.UseShellExecute = false;
ps.WindowStyle = ProcessWindowStyle.Hidden;
ps.RedirectStandardOutput = true;
ps.RedirectStandardError = true;

var counter = 0;
while (true)
{
    using var p = new Process();
    p.StartInfo = ps;
    p.OutputDataReceived += (sender, eventArgs) =>
    {
        if (string.IsNullOrWhiteSpace(eventArgs.Data))
            return;
        Console.WriteLine(eventArgs.Data);
    };
    p.ErrorDataReceived += (sender, eventArgs) =>
    {
        if (string.IsNullOrWhiteSpace(eventArgs.Data))
            return;
        Console.WriteLine(eventArgs.Data);
    };
    p.Start();
    p.BeginErrorReadLine();
    p.BeginOutputReadLine();
    p.WaitForExit();
    counter++;
    
    Console.WriteLine($"Run {counter} complete");

    if (p.ExitCode != 0)
    {
        Console.WriteLine($"Quiting because behavior was reproduced");
        return;
    }

    if (counter > quitAfter)
    {
        Console.WriteLine($"Quiting.  The issue never happened");
        return;
    }
}

static string GetPathToOtherProcess()
{
    var location = Assembly.GetExecutingAssembly().Location;
    location = location.Replace(Assembly.GetExecutingAssembly().GetName().Name!, "ConsoleApp16");
    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
        return Path.ChangeExtension(location, "exe");
    return location.Replace(".dll", string.Empty);
}
