using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Raid_AFK_Manager.Tools
{
    public static class WindowHandler
    {
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        private static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private static void PositionWindow(int processId, int x, int y, int cx, int cy)
        {
            //https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowpos
            const short SWP_NOZORDER = 0X4;
            const int SWP_SHOWWINDOW = 0x0040;
            Process p = Process.GetProcessById(processId);
            IntPtr handle = p.MainWindowHandle;
            if (handle != IntPtr.Zero)
            {
                SetWindowPos(handle, 0, x, y, cx, cy, SWP_NOZORDER | SWP_SHOWWINDOW);
                SetForegroundWindow(handle);
            }
            Thread.Sleep(1000);
        }

        internal static void RepositionRaidWindow(int processId)
        {
            PositionWindow(processId, 700, 0, 1235, 615);
        }

        internal static void RepositionMainWindow(int processId)
        {
            PositionWindow(processId, 0, 0, 720, 800);
        }
    }
}
