To reproduce

1) `dotnet build`

2) `dotnet run --project Runner/Runner.csproj --framework net6.0`

    Note : There are also targets for `net5.0` and `net48`

3) Wait for the program to end.

If the issue happened you'll see something like

```
Assemblies were not equal!!!
asm1.GetHashCode() = 4032828
asm2.GetHashCode() = 37489757
Default ALC contains asm1 = False
Default ALC contains asm2 = True
Run 76 complete
Quiting because behavior was reproduced
```

If the issue doesn't happen, you'll see
```
Quiting.  The issue never happened
```

If the issue doesn't happen, there are some parameters you can tweak at the top of `Runner/Program.cs` that may make it easier to reproduce