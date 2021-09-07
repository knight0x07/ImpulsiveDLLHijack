# ImpulsiveDLLHijack
A C# based tool which completely automates the process of discovering and exploiting DLL Hijacks in target binaries. The DLL Hijacked paths provided can later be weaponized during RedTeamOps to evade EDR's.

# 1. Prerequisites:

	[i] Procmon.exe  -> https://docs.microsoft.com/en-us/sysinternals/downloads/procmon
	[ii] Custom Malicious DLL's :
			-> Compiled from the MalDLL project provided above (or use the precompiled binaries if you trust me)
			-> 32Bit malicious dll name: maldll32.dll
			-> 64Bit malicious dll name: maldll64.dll
	[iii] Install NuGet Package: PeNet -> https://www.nuget.org/packages/PeNet/ (Prereq while compiling the ImpulsiveDLLHijack project)
	
	P.s [i] & [ii] prerequisites should be placed in the ImpulsiveDLLHijacks.exe's directory itself.

# 2. Usage:

![usage](https://user-images.githubusercontent.com/60843949/132341238-c6e0cad4-dfc1-4d8e-a011-73df17b652d6.PNG)

# 3. Examples
