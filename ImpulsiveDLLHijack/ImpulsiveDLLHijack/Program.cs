using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace ImpulsiveDLLHijack
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendMessage(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        static uint WM_CLOSE = 0x10;


        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static void GenPMC(string hexString2,string filename)
        {
            // Generating Custom PMC File as per the process name and filters
           
            string currentworkingdirectory = Directory.GetCurrentDirectory();
            string hexString1 = "A000000010000000200000008000000043006F006C0075006D006E007300000032006400280064007902640064000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002C00000010000000280000000400000043006F006C0075006D006E0043006F0075006E0074000000070000002401000010000000240000000001000043006F006C0075006D006E004D006100700000008E9C0000759C0000769C0000779C0000879C0000789C0000799C0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A400000010000000280000007C000000440062006700480065006C0070005000610074006800000043003A005C00500072006F006700720061006D002000460069006C00650073005C0044006500620075006700670069006E006700200054006F006F006C007300200066006F0072002000570069006E0064006F00770073002000280078003600340029005C00640062006700680065006C0070002E0064006C006C00200000001000000020000000000000004C006F006700660069006C00650000002C00000010000000280000000400000048006900670068006C006900670068007400460047000000000000002C00000010000000280000000400000048006900670068006C00690067006800740042004700000080FFFF001C000000100000001C000000000000005400680065006D00650000007C00000010000000200000005C0000004C006F00670046006F006E0074000000080000000000000000000000000000009001000000000000000000004D00530020005300680065006C006C00200044006C0067000000000000000000000000000000000000000000000000000000000000000000000000000000000088000000100000002C0000005C00000042006F006F006F006B006D00610072006B0046006F006E007400000008000000000000000000000000000000BC02000000000000000000004D00530020005300680065006C006C00200044006C006700000000000000000000000000000000000000000000000000000000000000000000000000000000002E000000100000002A0000000400000041006400760061006E006300650064004D006F00640065000000010000002A0000001000000026000000040000004100750074006F007300630072006F006C006C000000000000002E000000100000002A0000000400000048006900730074006F00720079004400650070007400680000001200000028000000100000002400000004000000500072006F00660069006C0069006E0067000000000000003800000010000000340000000400000044006500730074007200750063007400690076006500460069006C007400650072000000000000002C00000010000000280000000400000041006C0077006100790073004F006E0054006F007000000000000000360000001000000032000000040000005200650073006F006C00760065004100640064007200650073007300650073000000010000002600000010000000260000000000000053006F007500720063006500500061007400680000008400000010000000260000005E000000530079006D0062006F006C00500061007400680000007300720076002A00680074007400700073003A002F002F006D00730064006C002E006D006900630072006F0073006F00660074002E0063006F006D002F0064006F0077006E006C006F00610064002F00730079006D0062006F006C007300B8000000100000002800000090000000460069006C00740065007200520075006C006500730000000103000000759C000000000000012E000000";  // 1st Blob of PMC
            string hexString3 = "789C00000600000001140000004E004F005400200046004F0055004E00440000000000000000000000879C000005000000010A0000002E0064006C006C000000000000000000000033000000100000002E0000000500000048006900670068006C006900670068007400520075006C00650073000000010000000032000000100000002E0000000400000046006C0069006700680074005200650063006F00720064006500720000000000000032000000100000002E00000004000000520069006E006700420075006600660065007200530069007A00650000000000000030000000100000002C00000004000000520069006E0067004200750066006600650072004D0069006E00000000000000"; // 3rd Blob of PMC
            string finalstring = hexString1 + hexString2 + hexString3; // Concat the 3 Blobs of PMC File structure
            File.WriteAllBytes("config.pmc", StringToByteArray(finalstring)); // Write Custom PMC File
            Console.WriteLine("\n[+] Generated Custom PMC File : " + currentworkingdirectory + "\\config.pmc");

        }

        public static List<string> FindVulnHijacksPaths(string currentdirectory, string processpath, string filename, string inputarguments)
        {
            // Start Process-Monitor 
            Console.WriteLine("[+] Starting Process-Monitor ");

            Process procmon1 = new Process();
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.CreateNoWindow = true;
            proc.FileName = "Procmon.exe";
            proc.Arguments = "/Minimized /AcceptEula /quiet /LoadConfig " + "\"" + currentdirectory + "\"" + "\\config.pmc /backingfile " + "\"" + currentdirectory + "\\logs.pml\"";
            procmon1.StartInfo = proc;
            procmon1.Start();

            // Run the Target Process
            Thread.Sleep(10000);
            string filenamewithext = Path.GetFileName(processpath);
            Console.WriteLine("[+] Executing " + filenamewithext + " " + inputarguments + "!");


            Process targetprocess = new Process();
            ProcessStartInfo mainprocess = new ProcessStartInfo();
            mainprocess.CreateNoWindow = true;
            mainprocess.FileName = processpath;
            mainprocess.Arguments = inputarguments;
            targetprocess.StartInfo = mainprocess;
            targetprocess.Start();
            targetprocess.WaitForExit(10000); // Run target process for 20seconds!
            try
            {
                targetprocess.Kill();
                Console.WriteLine("[+] Exiting " + filenamewithext);
            }
            catch (System.InvalidOperationException)
            {
                Console.WriteLine("[-] Process exited automatically");
            }

            
            if (targetprocess.HasExited == true)
            {

                //Terminate Proc-Mon Process

                
                Process closeprocmon = new Process();
                ProcessStartInfo closeie = new ProcessStartInfo();
                closeie.CreateNoWindow = true;
                closeie.FileName = "Procmon.exe";
                closeie.Arguments = "/terminate ";
                closeprocmon.StartInfo = closeie;
                closeprocmon.Start();
                Console.WriteLine("[+] Exiting Process-Monitor");
                procmon1.WaitForExit();
                if (procmon1.HasExited == true)
                {
                    // Save the Output of Procmon to CSV.
                    Console.WriteLine("[+] Generating CSV ProcMon Log File: " + "\\vulnpaths.csv");
                    Process savecsv = new Process();
                    ProcessStartInfo csv = new ProcessStartInfo();
                    csv.CreateNoWindow = true;
                    csv.FileName = "Procmon.exe";
                    csv.Arguments = "/Minimized /AcceptEula /quiet /SaveApplyFilter /saveas " + "\"" +  currentdirectory + "\"" + "\\vulnpaths.csv /OpenLog " + "\"" +  currentdirectory + "\\logs.pml\"";
                    savecsv.StartInfo = csv;
                    savecsv.Start();
                    savecsv.WaitForExit();

                    //Parse the CSV to Get the Potentially Vulnerable Paths :-

                    Console.WriteLine("[+] Parsing ProcMon Log-File..");
                    var column5 = new List<string>();
                    using (var rd = new StreamReader(currentdirectory + "\\vulnpaths.csv"))
                    {
                        while (!rd.EndOfStream)
                        {
                            var splits = rd.ReadLine().Split(',');
                            column5.Add(splits[4]);

                        }
                    }

                    int numofpaths = column5.Count;

                    List<string> finalpaths = new List<string>(); // List of Final Paths

                    Console.WriteLine("[+] List of Unique Potentially Vulnerable DLL Paths : " + filenamewithext + "\n");

                    foreach (var element in column5.Skip(1))
                    {
                        string pattern = "\"";
                        var regexpath = Regex.Replace(element, pattern, string.Empty);

                        // Check if the directory of the following file paths exists

                        string directoryName = Path.GetDirectoryName(regexpath);

                        if (Directory.Exists(directoryName))
                        {


                            finalpaths.Add(regexpath); // Write Paths to the Final Paths List

                        }



                    }
                    return finalpaths;
                }


            }

            return new List<string>();

        }

        public static bool CheckMsgBox(string processpath)
        {
            // Check MsgBox -> Entry Point not found! 

            string ordinal_windowname = Path.GetFileName(processpath) + " - Ordinal Not Found";
            string entrypoint_windowname = Path.GetFileName(processpath) + " - Entry Point Not Found";
            string applicationerror_windowname = Path.GetFileName(processpath) + " - Application Error";
            bool msgpop = false;

            var ordinalhandle = FindWindowA(null, ordinal_windowname);
            var entrypointhandle = FindWindowA(null, entrypoint_windowname);
            var applicationerrorhandle = FindWindowA(null, applicationerror_windowname);


            while (ordinalhandle != IntPtr.Zero)
            {

                SendMessage(ordinalhandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                msgpop = true;
                Thread.Sleep(5000);
                ordinalhandle = FindWindowA(null, ordinal_windowname);

            }
            while (entrypointhandle != IntPtr.Zero)
            {

                SendMessage(entrypointhandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                msgpop = true;
                Thread.Sleep(5000);
                entrypointhandle = FindWindowA(null, entrypoint_windowname);

            }

            while (applicationerrorhandle != IntPtr.Zero)
            {

                SendMessage(applicationerrorhandle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                msgpop = true;
                Thread.Sleep(5000);
                applicationerrorhandle = FindWindowA(null, applicationerror_windowname);

            }

            return msgpop;




        }


        public static void WriteDLLLog(string dllpath,string message)
        {
            // write DLL hijack logs

            using (StreamWriter logger = File.AppendText("C:\\DLLLogs\\output_logs.txt"))
            {
                
                logger.WriteLine("[+] " + dllpath + " --> " + message);


            }



        }

        public static void ExecuteDLLHijack(List<string> uniquepaths, string processpath, string currentpwd, string inputarguments)
        {
            // Execute DLL Hijack on the Vulnerable DLL Paths


            // Find whether the target process is x86 or x64 Architecture -> Depending on which the DLL will be acquired further
            string processname = Path.GetFileName(processpath);
            string processnamewithoutext = Path.GetFileNameWithoutExtension(processpath);
            var peparsing = new PeNet.PeFile(processpath);
            string PEMachineCode = peparsing.ImageNtHeaders.FileHeader.Machine.ToString();
            
            ///Console.WriteLine("The machine code is: " + PEMachineCode);
            Console.WriteLine("\n");
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            Console.WriteLine("----------------------------------PERFORMING DLL HIJACK------------------------------------");
            Console.WriteLine("-------------------------------------------------------------------------------------------");
            Console.WriteLine("\n PE Architecture: " + processnamewithoutext + "= " + PEMachineCode);
            int i = 1;
            foreach (var paths in uniquepaths)
            {

                Console.WriteLine("\n-------------------------------------------------------------------------------------------\n");


                string mal32 = "maldll32.dll";

                string mal64 = "maldll64.dll";

                string dllfinalname = null;
                string maldllfile = currentpwd + "\\" + mal32;

                if (PEMachineCode == "I386")
                {
                    // The Target process is 32 Bit --> Therefore DLL should be 32 bit!

                    maldllfile = currentpwd + "\\" + mal32;
                    dllfinalname = mal32;
                    if (!File.Exists("maldll32.dll"))
                    {
                        Console.Write("\n[+] PreReq Check: maldll32.dll does not exist! \n");
                        System.Environment.Exit(1);
                    }
                }
                else if (PEMachineCode == "Amd64")
                {
                    // The Target process is 64 Bit --> Therefore DLL should be 64 bit!

                    maldllfile = currentpwd + "\\" + mal64;
                    dllfinalname = mal64;
                    if (!File.Exists("maldll64.dll"))
                    {
                        Console.Write("\n[+] PreReq Check: maldll64.dll does not exist! \n");
                        System.Environment.Exit(1);
                    }
                }
                else
                {
                    Console.WriteLine("[+] Improper Architecture of " + processname);
                    System.Environment.Exit(1);
                }
                Console.WriteLine("[+] " + i + ". DLL Hijacking: " + Path.GetFileName(paths));
                string destinationpath = paths;

                try
                {

                    // Copying malicious dll to Vulnerable DLL Paths

                    File.Copy(maldllfile, destinationpath, true);
                    File.SetAttributes(destinationpath, FileAttributes.Normal);
                    Console.WriteLine("     ->  Copy : " + dllfinalname + "-> " + destinationpath + " completed");
                    Thread.Sleep(5000);

                    // Starting the Target process

                    Process starttargetprocess = new Process();
                    ProcessStartInfo startie = new ProcessStartInfo();
                    startie.CreateNoWindow = true;
                    startie.FileName = processpath;
                    startie.Arguments = inputarguments;
                    starttargetprocess.StartInfo = startie;
                    starttargetprocess.Start();
                    Console.WriteLine("     ->  Starting " + processname + "! (10 seconds)");
                    Thread.Sleep(10000);

                    // Checking if any msgbox is popped up :- Ordinal - Application - Entry Point Not Found Error
                    string msg = "";
                    bool MessagePopup = CheckMsgBox(processpath);
                    if (MessagePopup == true)
                    {

                        //Console.WriteLine("     ->  Note: The DLL " + paths + " was loaded but the export function did not match! Manual Analysis required.. "); // After loading the DLL the export function called by the target process was not present in the malicious dll leading to this issue, if the export function is matched the dll will be loaded successfully :)
                        Console.WriteLine("     ->  " + processname + " killed automatically");
                        Thread.Sleep(5000);
                        string logdirectories = "C:\\DLLLogs\\";
                        if (Directory.Exists(logdirectories))
                        {
                            // The Initial DLL Was executed -> Check 1 Completed
                            string finaldllognames = logdirectories + processnamewithoutext + "_" + Path.GetFileName(paths);
                            if (File.Exists(finaldllognames))
                            {
                                Console.WriteLine("     [+] DLL Hijack Successful [Entry Point Not Found] -> DllName: " + Path.GetFileName(paths) + " | " + processname);
                                Console.WriteLine("     ->  Analyzing next DLL!");
                                msg = "DLL Hijack Successful -  [Entry Point Not Found] ";
                                WriteDLLLog(paths, msg);
                            }
                            else
                            {

                                Console.WriteLine("     [-] DLL Hijack Successful [Entry Point Not Found - Manual Analysis Required!]: " + paths);
                                Console.WriteLine("     ->  Analyzing next DLL!");
                                msg = "DLL Hijack Successful [Entry Point Not Found - Manual Analysis Required]";
                                WriteDLLLog(paths, msg);
                            }


                        }
                        else
                        {

                            Console.WriteLine("     [-] DLL Hijack successful [Entry Point Not Found - Manual Analysis Required] " + paths);
                            Console.WriteLine("     ->  Analyzing next DLL!");
                            msg = "DLL Hijack successful [Entry Point Not Found - Manual Analysis Required]";
                            WriteDLLLog(paths, msg);
                        }
                        
                        // File.SetAttributes(destinationpath, FileAttributes.Normal);
                        Thread.Sleep(4000);
                        File.Delete(destinationpath);
                        Console.WriteLine("     ->  Deleted: " + destinationpath);



                    }
                    else
                    {
                        try
                        {
                            starttargetprocess.WaitForExit(5000);
                            starttargetprocess.Kill();
                            Console.WriteLine("     ->  Killing " + processname + "!");
                            starttargetprocess.WaitForExit(5000);
                        }
                        catch
                        {
                            Process[] processes = Process.GetProcessesByName(processnamewithoutext);
                            if (processes.Length == 0)
                            {
                                
                                Console.WriteLine("     ->  Already Killed " + processname + "!");
                            }
                            else
                            {
                                Process[] runingProcess = Process.GetProcesses();
                                for (i = 0; i < runingProcess.Length; i++)
                                {
                                    // compare equivalent process by their name
                                    if (runingProcess[i].ProcessName == processnamewithoutext)
                                    {
                                        // kill  running process
                                        runingProcess[i].Kill();
                                    }

                                }
                                Console.WriteLine("" + processname + " Killed!!");

                            }

                           

                        }

                        // Checking if the Logs are been generated - C:\DLLLogs 
                        Thread.Sleep(5000);
                        string logdirectory = "C:\\DLLLogs\\";
                        if (Directory.Exists(logdirectory))
                        {
                            // The Initial DLL Was executed -> Check 1 Completed
                            string finaldllogname = logdirectory + processnamewithoutext + "_" + Path.GetFileName(paths);
                            if (File.Exists(finaldllogname))
                            {
                                Console.WriteLine("     [+] DLL Hijack Successful -> DllName: " + Path.GetFileName(paths) + " | " + processname);
                                Console.WriteLine("     ->  Analyzing next DLL!");
                                msg = "DLL Hijack Successful";
                                WriteDLLLog(paths, msg);
                            }
                            else
                            {

                                Console.WriteLine("     [-] DLL Hijack Unsuccessful : " + paths);
                                Console.WriteLine("     ->  Analyzing next DLL!");
                                msg = "DLL Hijack Unsuccessful";
                                WriteDLLLog(paths, msg);
                            }


                        }
                        else
                        {

                            Console.WriteLine("     [-] DLL Hijack Unsuccessful : " + paths);
                            Console.WriteLine("     ->  Analyzing next DLL!");
                            msg = "DLL Hijack Unsuccessful";
                            WriteDLLLog(paths, msg);
                        }

                        File.Delete(destinationpath);
                        Console.WriteLine("     ->  Deleted: " + destinationpath);

                    }





                }
                catch (Exception ex)
                {
                    if (ex is System.UnauthorizedAccessException)
                    {
                        try
                        {
                            File.Delete(destinationpath);
                        }
                        catch
                        {

                        }
                        Console.WriteLine("     [-] Copy: Access to Path is Denied: " + destinationpath);
                        string msge = "Copy: Access to Path is Denied";
                        WriteDLLLog(paths, msge);
                        
                        


                    }

                }

                i += 1;
                Thread.Sleep(3000);
            }




        }

        public static void FinalOutput(string processname)
        {
            Console.WriteLine("\n----------------------------------------------------------------------------");
            Console.WriteLine("-----------------------FINAL DLL HIJACK OUTPUT: " + processname + "-----------------");
            Console.WriteLine("----------------------------------------------------------------------------\n");

            string[] logs = File.ReadAllLines("C:\\DLLLogs\\output_logs.txt");
            foreach (string logline in logs)
            {
                Console.WriteLine(logline);
            }

        }




        static void Main(string[] args)
        {

            Console.Write(@" 

    ____                      __     _            ____  __    __    __  ___   _            __  
   /  _/___ ___  ____  __  __/ /____(_)   _____  / __ \/ /   / /   / / / (_) (_)___ ______/ /__
   / // __ `__ \/ __ \/ / / / / ___/ / | / / _ \/ / / / /   / /   / /_/ / / / / __ `/ ___/ //_/
 _/ // / / / / / /_/ / /_/ / (__  ) /| |/ /  __/ /_/ / /___/ /___/ __  / / / / /_/ / /__/ ,<   
/___/_/ /_/ /_/ .___/\__,_/_/____/_/ |___/\___/_____/_____/_____/_/ /_/_/_/ /\__,_/\___/_/|_| 
             /_/                                                       /___/
                       
                        Author: https://twitter.com/knight0x07
                        Github: https://github.com/knight0x07

            ");

            try
            {
                if (args[0] == "-path")
                {
                    if (File.Exists(args[1]))
                    {
                        // Initiate the process

                        //Provide Process Name for Finding Potential DLL Hijacks

                        string logpathis = "C:\\DLLLogs";
                        if (Directory.Exists(logpathis))
                        {
                            Directory.Delete(logpathis, true);
                        }
                        DirectoryInfo di = Directory.CreateDirectory(logpathis);
                        using (StreamWriter sw = File.CreateText(logpathis + "\\output_logs.txt"));
                        string currentpwd = Directory.GetCurrentDirectory();
                        Console.Write("\n[+] Initiating Impulsive DLL Hijack! ");
                        if (!File.Exists("Procmon.exe"))
                        {
                            Console.Write("\n[+] PreReq Check: Procmon.exe does not exist! \n");
                            System.Environment.Exit(1);
                        }
                        string processpath = args[1];
                        string filename = Path.GetFileNameWithoutExtension(processpath);
                        string processname = Path.GetFileName(args[1]);
                        Console.Write("\n[+] Target Process Name: " + processname);
                        string inputarguments = "";
                        try
                        {
                            inputarguments = args[2];
                        }
                        catch (System.IndexOutOfRangeException)
                        {

                        }

                        // Conversion to Hex String

                        byte[] bytes = Encoding.Default.GetBytes(processname);
                        string hexString = BitConverter.ToString(bytes);
                        hexString = hexString.Replace("-", "00");
                        int lengthstring = hexString.Length;
                        int bufferprocessname = 108;
                        int padzeros = bufferprocessname - lengthstring;
                        string padding = new String('0', padzeros);
                        string hexString2 = hexString + padding; // 2nd Blob of PMC
                        int lengthhexString2 = hexString2.Length;
                        if (lengthhexString2 > bufferprocessname)
                        {
                            Console.WriteLine("\n[-] Error: Process Name Out of Bound..");
                        }
                        else
                        {

                            GenPMC(hexString2, filename); // Generate Custom PMC config as per process name
                            List<string> finalpaths = FindVulnHijacksPaths(currentpwd, processpath, filename, inputarguments); // Find Potentially Vulnerable Hijack Paths
                            List<string> uniquepaths = finalpaths.Distinct().ToList(); // Remove Duplicate Paths
                            foreach (var path in uniquepaths)
                            {
                                Console.WriteLine("     -> " + path);
                            }
                            ExecuteDLLHijack(uniquepaths, processpath, currentpwd, inputarguments);
                            FinalOutput(processname);
                            Console.WriteLine("\n\n[+] Final Log File stored at: " + logpathis + "\\output_logs.txt");

                        }



                    }
                    else
                    {
                      Console.WriteLine("\n[-] Error: Invalid File Path provided! ");
                    }



                }
                else if (args[0] == "-h")
                {

                    Console.WriteLine("\n[+] Command : ImpulsiveDLLHijack.exe -path <binarypath> <args_if_any> ");


                }
                else
                {

                    Console.WriteLine("\n[+] Execution Command: ImpulsiveDLLHijack.exe -path <binarypath> <args_if_any> ");
                    Console.WriteLine("[+] Help Command : ImpulsiveDLLHijack.exe -h ");

                }



            }
            catch (System.IndexOutOfRangeException)
            {

                Console.WriteLine("\n[+] Execution Command: ImpulsiveDLLHijack.exe -path <binarypath> <args_if_any> ");
                Console.WriteLine("[+] Help Command : ImpulsiveDLLHijack.exe -h ");



            }
        }
    }
}
