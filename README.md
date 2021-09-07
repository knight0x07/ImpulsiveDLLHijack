# ImpulsiveDLLHijack
A C# based tool which automates the process of discovering and exploiting DLL Hijacks in target binaries. The DLL Hijacked paths provided can later be weaponized during RedTeamOps to evade EDR's.

# Methodological Approach:

The tool basically acts on automating following procedures required for DLL Hijacks:

i. Discovering - Finding Potentially Vulnerable DLL Hijackable paths
ii. Exploiting - Confirming whether the Malicious DLL was been loaded from the Hijacked path leading to a confirmation of 100% exploitable DLL Hijack!

i. Discovering -
	Process:
	[+] Provide Target binary path to ImpulsiveDLLHiack
	[+] Automation of ProcMon along with the execution of Target binary to find Potentially Vulnerable DLL Hijackable paths.

ii. Exploiting -
	Process:
	[+] Parse Potentially Vulnerable DLL Hijackable paths
	[+] Loop through all the potentially vulnerable paths by performing


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
