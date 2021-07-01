This file describes what folder contains the assemblies you need to use with a particular version of the .NET Framework.



Folder           Description
---------------------------------------------------------------------------------------------------------------------------


bin              Contains assemblies to use with .NET Framework 2.0, 3.0, 3.5, 4.0, 4.5.
                 These are the assemblies that you should normally use.

netstandard2.0   Contains assemblies to use with frameworks that implements .NET Standard 2.0 such as .NET Core 2.0.

Note that netstandard2.0 dll has external references:

Microsoft.Extensions.DependencyModel >= 2.0.4
Mono.Posix.NETStandard >= 1.0.0
Microsoft.Win32.Registry >= 4.7.0
SkiaSharp >= 2.80.1
System.Diagnostics.PerformanceCounter >= 4.5.0
System.Drawing.Common >= 5.0.0
System.Reflection.Emit >= 4.7.0
System.Reflection.Emit.ILGeneration >= 4.7.0
System.Security.Cryptography.Pkcs >= 4.7.0
System.Security.Permissions >= 4.5.0
System.Text.Encoding.CodePages >= 5.0.0
