using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace RemmoteConsole {
    class RemoteProccess {
        public static void RunProcessOnRemoteMachine(string remoteMachine, string strPathToTheExe, string usernameAndDomain, string password) {
            try {
                var processToRun = new[] { strPathToTheExe };
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

                //InvokeMethodOptions methodOptions = new InvokeMethodOptions(null, TimeSpan.MaxValue);
                var outParams = wmiProcess.InvokeMethod("Create", inParams, null);

                Console.WriteLine("Process ID: " + outParams["processId"]);

            } catch (Exception ex) {
                Console.WriteLine("Error occurred : " + ex.Message.ToString());
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
