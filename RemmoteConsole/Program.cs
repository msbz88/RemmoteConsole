using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemmoteConsole {
    class Program {
        static void Main(string[] args) {
            RemoteProccess.RunProcessOnRemoteMachine("", @"C:\Windows\system32\notepad.exe", @"", "");
            Console.WriteLine("Task ended.");
        }
    }
}
