using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ClipboardWatcher
{
  internal static class NativeHelpers
  {
    [DllImport("User32.dll")]
    internal static extern int SetClipboardViewer(int hWndNewViewer);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    internal static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
  }
}
