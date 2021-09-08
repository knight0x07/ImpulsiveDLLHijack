# ImpulsiveDLLHijack
A C# based tool which automates the process of discovering and exploiting DLL Hijacks in target binaries. The DLL Hijacked paths provided can later be weaponized during RedTeamOps to evade EDR's.

# Methodological Approach:

	The tool basically acts on automating following steps performed for DLL Hijacking:

	i. Discovering - Finding Potentially Vulnerable DLL Hijack paths
	ii. Exploiting - Confirming whether the Confirmatory DLL was been loaded from the Hijacked path leading to a confirmation of 100% exploitable DLL Hijack!

	i. Discovering -
		Process:
		[+] Provide Target binary path to ImpulsiveDLLHijack.exe
		[+] Automation of ProcMon along with the execution of Target binary to find Potentially Vulnerable DLL Hijackable paths.

	ii. Exploiting -
		Process:
		[+] Parse Potentially Vulnerable DLL Hijack paths from CSV generated automatically via ProcMon
		[+] Copy the Confirmatory DLL (as per the PE architecture) to the hijack paths one by one and execute the Target Binary for predefined time period simultaneously.
		[+] As the DLL hijacking process is in progress following are the outputs which can be gathered from the Hijack Scenario,
			-> The Confirmatory DLL present on the potentially vulnerable Hijackable Path is loaded by the Target Binary we get following output on the console stating 			       that the DLL Hijack was successful - **DLL Hijack Successful -> DLLName: <DLLname> | <Target_binary_name>**
			-> The Confirmatory DLL present on the potentially vulnerable Hijackable Path is not loaded by the Target Binary we get following output on the console 		           stating that the DLL Hijack was unsuccessful - **DLL Hijack Unsuccessful -> <DLLPath>**
				
			**Entry Point Not Found Scenarios:**

			-> The Confirmatory DLL present on the potentially vulnerable Hijackable Path is not loaded by the Target Binary as the Entry Point of the DLL is 				   different from our default entry point "DllMain" throwing an error - "Entry Point Not Found", we get following output on the console stating that the                              DLL Hijack was hijackable if the entry point was correct -** DLL Hijack Successful -> [Entry Point Not Found - Manual Analysis Required!]: 				   <Hijack_path>**

			-> The Confirmatory DLL present on the potentially vulnerable Hijackable Path is loaded by the Target Binary even after the Entry Point of the DLL is 				   different from our default entry point "DllMain" throwing an error - "Entry Point Not Found", we get following output on the console stating that the                              DLL Hijack was success even after the entry point was not correct -** DLL Hijack Successful -> [Entry Point Not Found]: <Hijack_path>**			   
			   


# 1. Prerequisites:

	[i] Procmon.exe  -> https://docs.microsoft.com/en-us/sysinternals/downloads/procmon
	[ii] Custom Confirmatory DLL's :
			-> These are DLL files which assist the tool to get the confirmation whether the DLL's are been successfully loaded from the identified hijack path 
			-> Compiled from the MalDLL project provided above (or use the precompiled binaries if you trust me!)
			-> 32Bit dll name should be: maldll32.dll
			-> 64Bit dll name should be: maldll64.dll
	[iii] Install NuGet Package: PeNet -> https://www.nuget.org/packages/PeNet/ (Prereq while compiling the ImpulsiveDLLHijack project)
	
	P.s [i] & [ii] prerequisites should be placed in the ImpulsiveDLLHijacks.exe's directory itself.

# 2. Usage:

![usage](https://user-images.githubusercontent.com/60843949/132341238-c6e0cad4-dfc1-4d8e-a011-73df17b652d6.PNG)

# 3. Examples
