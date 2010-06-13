using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ClipboardWatcher
{
  public partial class Form1 : Form
  {
    private IntPtr nextClipboardViewer;
    private bool _isStart = false;

    public Form1()
    {
      InitializeComponent();
      lbxWords.Items.Clear();
    }

    void StartMonitor()
    {
      if (_isStart)
      {
        return;
      }

      Clipboard.Clear();
      nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
      _isStart = true;
    }

    void StopMonitor()
    {
      if (!_isStart)
      {
        return;
      }

      ChangeClipboardChain(this.Handle, nextClipboardViewer);
      _isStart = false;
    }

    string ProcessWord(string word)
    {
      var trimmed = word.Trim(new char[] {' ', '.', ','} );
      return trimmed.ToLower();
    }

    private void DisplayClipboardData()
    {
      IDataObject data = Clipboard.GetDataObject();
      if (data.GetDataPresent(DataFormats.Text))
      {
        string cpdata = data.GetData(DataFormats.Text).ToString();

        if (lbxWords.Items.Count == 0 || (string) lbxWords.Items[lbxWords.Items.Count - 1] != cpdata)
        {
          lbxWords.Items.Add(cpdata);
        }
      }
    }

    [DllImport("User32.dll")]
    protected static extern int SetClipboardViewer(int hWndNewViewer);

    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

    protected override void WndProc(ref System.Windows.Forms.Message message)
    {
      const int WM_DRAWCLIPBOARD = 0x308;
      const int WM_CHANGECBCHAIN = 0x030D;

      switch (message.Msg)
      {
        case WM_DRAWCLIPBOARD:
          DisplayClipboardData();
          SendMessage(nextClipboardViewer, message.Msg, message.WParam, message.LParam);
          break;

        case WM_CHANGECBCHAIN:
          if (message.WParam == nextClipboardViewer)
          {
            nextClipboardViewer = message.LParam;
          }
          else
          {
            SendMessage(nextClipboardViewer, message.Msg, message.WParam, message.LParam);
          }
          break;

        default:
          base.WndProc(ref message);
          break;
      }
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      StartMonitor();
      btnStart.Enabled = false;
      btnStop.Enabled = true;
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
      StopMonitor();
      btnStart.Enabled = true;
      btnStop.Enabled = false;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
      var builder = new StringBuilder();
      foreach (string str in lbxWords.Items)
      {
        builder.AppendLine(ProcessWord(str));
      }

      Clipboard.SetDataObject(builder.ToString(), true);
    }

  }
}
