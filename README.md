# ImpulsiveDLLHijack

C# based tool which automates the process of discovering and exploiting DLL Hijacks in target binaries. The Hijacked paths discovered can later be weaponized during RedTeam Operations to evade EDR's.

# 1. Methodological Approach :

The tool basically acts on automating following stages performed for DLL Hijacking:

- **Discovery** - Finding Potentially Vulnerable DLL Hijack paths
- **Exploitation** - Confirming whether the Confirmatory DLL was been loaded from the Hijacked path leading to a confirmation of 100% exploitable DLL Hijack!

**Discovery Methodology :**

- Provide Target binary path to ImpulsiveDLLHijack.exe
- Automation of ProcMon along with the execution of Target binary to find Potentially Vulnerable DLL Hijackable paths.

**Exploitation Methodology :**

- Parse Potentially Vulnerable DLL Hijack paths from CSV generated automatically via ProcMon.
- Copy the Confirmatory DLL (as per the PE architecture) to the hijack paths one by one and execute the Target Binary for predefined time period simultaneously.
- As the DLL hijacking process is in progress following are the outputs which can be gathered from the Hijack Scenario:
	* The Confirmatory DLL present on the potentially vulnerable Hijackable Path is loaded by the Target Binary we get following output on the console stating that the DLL Hijack was successful - **DLL Hijack Successful -> DLLName: <DLLname> | <Target_binary_name>**
	* The Confirmatory DLL present on the potentially vulnerable Hijackable Path is not loaded by the Target Binary we get following output on the console stating that the DLL Hijack was unsuccessful - **DLL Hijack Unsuccessful -> <DLL_Path>**

	**Entry Point Not Found Scenarios:**

	-  The Confirmatory DLL present on the potentially vulnerable Hijackable Path is not loaded by the Target Binary as the Entry Point of the DLL is 				   different from our default entry point "DllMain" throwing an error - "Entry Point Not Found", we get following output on the console stating that the                              DLL Hijack was hijackable if the entry point was correct -> **DLL Hijack Successful -> [Entry Point Not Found - Manual Analysis Required!]: <Hijack_path>**
	- The Confirmatory DLL present on the potentially vulnerable Hijackable Path is executed by the Target Binary even after the Entry Point of the DLL is 		    different from our default entry point "DllMain" throwing an error "Entry Point Not Found", we get following output on the console stating that the DLL Hijack was success even after the entry point was not correct -> **DLL Hijack Successful -> [Entry Point Not Found]: <Hijack_path>**

**Note: The "Entry Point not found" Error is been handled by the code programmatically no need to close the MsgBox manually :) # Rather this would crash the code further******

- Once the DLL Hijacking process is completed for every Potentially Vulnerable DLL Hijack path we get the final output on the console as well as in a text file (C:\DLLLogs\output_logs.txt) in the following format:

	- <DLLHijack_path> --> DLL Hijack Successful (**if the Hijack was successful**)
	- <DLLHijack_path> --> DLL Hijack Unuccessful (**if the Hijack was unsuccessful**)
	- <DLLHijack_path> --> DLL Hijack Successful [Entry Point Not Found - Manual Analysis Required] (**if the Entry point was not found but can be successful after manual analysis**)
	- <DLLHijack_path> --> DLL Hijack Successful [Entry Point Not Found] (**if the hijack was successful even after the entry point was not found**)
	- <DLLHijack_path> --> Copy: Access to Path is Denied (**Access denied**)

**These Confirmed DLL Hijackable paths can later be weaponized during a Red Team Engagement to load a Malicious DLL Implant via a legitimate executable (such as OneDrive,Firefox,MSEdge,"Bring your own LOLBINs" etc.) and bypass State of the art EDR's as most of them fail to detect DLL Hijacking as assessed by George Karantzas and Constantinos Patsakis as mentioned in there research paper: https://arxiv.org/abs/2108.10422


		
# 2. Prerequisites:

- **Procmon.exe**  -> https://docs.microsoft.com/en-us/sysinternals/downloads/procmon
- **Custom Confirmatory DLL's** :
	- These are DLL files which assist the tool to get the confirmation whether the DLL's are been successfully loaded from the identified hijack path 
	- Compiled from the MalDLL project provided above (or use the precompiled binaries if you trust me!)
	- 32Bit dll name should be: maldll32.dll
	- 64Bit dll name should be: maldll64.dll
	- Install NuGet Package:** PeNet** -> https://www.nuget.org/packages/PeNet/ (Prereq while compiling the ImpulsiveDLLHijack project)

**Note: i & ii prerequisites should be placed in the ImpulsiveDLLHijacks.exe's directory itself.**

- **Build and Setup Information:**

	- **ImpulsiveDLLHijack**

		- Clone the repository in Visual Studio
		- Once project is loaded in Visual Studio go to "Project" --> "Manage NuGet packages"  --> Browse for packages and install "PeNet" -> https://www.nuget.org/packages/PeNet/
		- Build the project!
		- The ImpulsiveDLLHijack.exe will be inside the bin directory.

	- **And for Confirmatory DLL's:**

		- Clone the repository in Visual Studio
		- Build the project with x86 and x64
		- Rename x86 release as maldll32.dll and x64 release as maldll64.dll

	- **Setup:** Copy the Confirmatory DLL's (maldll32 & maldll64) in the ImpulsiveDLLHijack.exe directory & then execute ImpulsiveDLLHijack.exe :))

# 3. Usage:

![usage](https://user-images.githubusercontent.com/60843949/132341238-c6e0cad4-dfc1-4d8e-a011-73df17b652d6.PNG)

# 4. Examples:

- Target Executable: OneDrive.exe

- Stage: Discovery

![first](https://user-images.githubusercontent.com/60843949/132492019-6dbb30aa-658f-4642-b9bd-69036d2d081a.PNG)

- Stage: Exploitation

	- Successful DLL Hijacks:

	![success_one](https://user-images.githubusercontent.com/60843949/132493144-78072724-c2c0-4390-b761-7bfb9abfcb5b.PNG)

	- Unsuccessful DLL Hijacks:

	![unsuccessful](https://user-images.githubusercontent.com/60843949/132493860-d9df5fff-6cbc-4785-88a2-92d27cf128e2.PNG)

	- DLL is not loaded as the entry point is not identical! Manual Analysis might make it a successful DLL Hijack :)

	![entrypoint_not_found](https://user-images.githubusercontent.com/60843949/132494965-9d3b302b-360c-48b1-b2a4-ec950fddd893.PNG)

	- DLL Hijack successful even after unidentical entry point!

	![entrypoint_not_found](https://user-images.githubusercontent.com/60843949/132494965-9d3b302b-360c-48b1-b2a4-ec950fddd893.PNG)

- Stage: Final Results and Logs

	- C:\DLLLogs\output_logs.txt:

	![output_logs](https://user-images.githubusercontent.com/60843949/132496859-808bb809-9230-4aee-afef-fe71ef03e8b5.PNG)


**Thankyou, Feedback would be greatly appreciated!** - knight!







	


