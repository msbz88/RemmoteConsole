using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemmoteConsole {
    class Program {
        static void Main(string[] args) {
           RemoteProccess.RunProcessOnRemoteMachine("", @"C:\Windows\system32\calc.exe", "", "");
           // RemoteProccess.PsExecRun("DK01WR5188", @"C:\Windows\system32\calc.exe", @"SCDOM\MSBZ", "Simcorp100");

            Console.WriteLine("Task ended.");
            Console.ReadKey();
        }
    }
}
