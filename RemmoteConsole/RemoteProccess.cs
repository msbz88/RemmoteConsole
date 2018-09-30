using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace RemmoteConsole {
    class RemoteProccess {
        public static void RunProcessOnRemoteMachine(string remoteMachine, string strPathToTheExe, string usernameAndDomain, string password) {
            try {
                var connection = new ConnectionOptions();
                connection.Impersonation = ImpersonationLevel.Impersonate;
                connection.Authentication = AuthenticationLevel.Packet;
                connection.EnablePrivileges = true;
                connection.Username = usernameAndDomain;
                connection.Password = password;
                var wmiScope = new ManagementScope(String.Format("\\\\{0}\\root\\cimv2", remoteMachine), connection);
                wmiScope.Connect();
                var wmiProcess = new ManagementClass(wmiScope, new ManagementPath("Win32_Process"), new ObjectGetOptions());

                var inParams = wmiProcess.GetMethodParameters("Create");
                inParams["CommandLine"] = strPathToTheExe;

                var outParams = wmiProcess.InvokeMethod("Create", inParams, null);

                Console.WriteLine("Process ID: " + outParams["processId"]);

            } catch (Exception ex) {
                Console.WriteLine("Error occurred : " + ex.Message.ToString());
            }
        }

        public static void PsExecRun(string remoteMachine, string strPathToTheExe, string usernameAndDomain, string password) {
            Process p = new Process();
            p.StartInfo.FileName = @"c:\PsTools\PsExec.exe";
            p.StartInfo.Arguments = @"\\" + remoteMachine + " -u " + usernameAndDomain + " -p " + password + " -i " + strPathToTheExe;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
        }

        public void ListAllMachinesInLAN(string remoteMachine, string usernameAndDomain, string password) {
            ConnectionOptions options = new ConnectionOptions();
            options.Password = password;
            options.Username = usernameAndDomain;

            ManagementScope scope = new ManagementScope("\\\\"+ remoteMachine + "\\root\\cimv2", options);
            scope.Connect();

            //Query system for Operating System information
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();
            foreach (ManagementObject m in queryCollection) {
                // Display the remote computer information
                Console.WriteLine("Computer Name : {0}", m["csname"]);
                Console.WriteLine("Windows Directory : {0}", m["WindowsDirectory"]);
                Console.WriteLine("Operating System: {0}", m["Caption"]);
            }
        }
        /*
        public static void CreateRemoteService(string machineName, string serviceNmae, string displayServiceName, string pathToService) {
            using (var scmHandle = NativeMethods.OpenSCManager(machineName, null, NativeMethods.SCM_ACCESS.SC_MANAGER_CREATE_SERVICE)) {
                if (scmHandle.IsInvalid) {
                    throw new Win32Exception();
                }
                using (
                    var serviceHandle = NativeMethods.CreateService(
                        scmHandle,
                        serviceNmae,
                        displayServiceName,
                        NativeMethods.SERVICE_ACCESS.SERVICE_ALL_ACCESS,
                        NativeMethods.SERVICE_TYPES.SERVICE_WIN32_OWN_PROCESS,
                        NativeMethods.SERVICE_START_TYPES.SERVICE_AUTO_START,
                        NativeMethods.SERVICE_ERROR_CONTROL.SERVICE_ERROR_NORMAL,
                        pathToService,
                        null,
                        IntPtr.Zero,
                        null,
                        null,
                        null)) {
                    if (serviceHandle.IsInvalid) {
                        throw new Win32Exception();
                    }

                    NativeMethods.StartService(serviceHandle, 0, null);
                }
            }
        }
        */

    }
}
