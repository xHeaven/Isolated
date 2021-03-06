﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Isolated.Protection.Other
{
    internal static class AntiDebugSafe
    {
        [DllImport("ntdll.dll", CharSet = CharSet.Auto)]
        public static extern int NtQueryInformationProcess(IntPtr test, int test2, int[] test3, int test4, ref int test5);

        private static void Initialize()
        {
            bool flag = Debugger.IsLogging();
            if (flag)
            {
                Environment.Exit(0);
            }
            bool isAttached = Debugger.IsAttached;
            if (isAttached)
            {
                Environment.Exit(0);
            }
            bool flag2 = Environment.GetEnvironmentVariable("complus_profapi_profilercompatibilitysetting") != null;
            if (flag2)
            {
                Environment.Exit(0);
            }
            bool flag3 = Environment.GetEnvironmentVariable("COR_ENABLE_PROFILING") == "1";
            if (flag3)
            {
                Environment.Exit(0);
            }
            bool flag4 = Environment.OSVersion.Platform == PlatformID.Win32NT;
            if (flag4)
            {
                int[] array = new int[6];
                int num = 0;
                IntPtr intPtr = Process.GetCurrentProcess().Handle;
                bool flag5 = NtQueryInformationProcess(intPtr, 31, array, 4, ref num) == 0 && array[0] != 1;
                if (flag5)
                {
                    Environment.Exit(0);
                }
                bool flag6 = NtQueryInformationProcess(intPtr, 30, array, 4, ref num) == 0 && array[0] != 0;
                if (flag6)
                {
                    Environment.Exit(0);
                }
                bool flag7 = NtQueryInformationProcess(intPtr, 0, array, 24, ref num) == 0;
                if (flag7)
                {
                    intPtr = Marshal.ReadIntPtr(Marshal.ReadIntPtr((IntPtr)array[1], 12), 12);
                    Marshal.WriteInt32(intPtr, 32, 0);
                    IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, 0);
                    IntPtr ptr = intPtr2;
                    do
                    {
                        ptr = Marshal.ReadIntPtr(ptr, 0);
                        bool flag8 = Marshal.ReadInt32(ptr, 44) == 1572886 && Marshal.ReadInt32(Marshal.ReadIntPtr(ptr, 48), 0) == 7536749;
                        if (flag8)
                        {
                            IntPtr intPtr3 = Marshal.ReadIntPtr(ptr, 8);
                            IntPtr intPtr4 = Marshal.ReadIntPtr(ptr, 12);
                            Marshal.WriteInt32(intPtr4, 0, (int)intPtr3);
                            Marshal.WriteInt32(intPtr3, 4, (int)intPtr4);
                        }
                    }
                    while (!ptr.Equals(intPtr2));
                }
            }
        }
    }
}