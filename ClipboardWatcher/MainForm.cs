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
  public class ClipboardEventArgs : EventArgs
  {
    public ClipboardEventArgs(string data)
    {
      Data = data;
    }

    public string Data { get; set; }
  }

  public partial class MainForm : Form
  {
    private IntPtr nextClipboardViewer;

    public event EventHandler<ClipboardEventArgs> ClipboardChanged;

    public MainForm()
    {
      InitializeComponent();
      lbxWords.Items.Clear();

      var startClicks = Observable.FromEvent<EventArgs>(btnStart, "Click");
      var stopClicks = Observable.FromEvent<EventArgs>(btnStop, "Click");
      var cbChanged = Observable.FromEvent<ClipboardEventArgs>(this, "ClipboardChanged");

      Observable.Merge(startClicks.Select(_ => true), 
                       stopClicks.Select(_ => false))
                .DistinctUntilChanged()
                .Subscribe(val =>
      {
        btnStart.Enabled = !val;
        btnStop.Enabled = val;
        DoMonitor(val);
      });

      var clipboard = from start in startClicks
                      from cb in cbChanged.TakeUntil(stopClicks)
                      select cb;

      clipboard.Select(e => e.EventArgs.Data).DistinctUntilChanged().Subscribe(str => lbxWords.Items.Add(str));
    }

    void MainForm_ClipboardChanged(object sender, ClipboardEventArgs e)
    {
      var cpdata = e.Data;
      if (lbxWords.Items.Count == 0 || (string)lbxWords.Items[lbxWords.Items.Count - 1] != cpdata)
      {
        lbxWords.Items.Add(cpdata);
      }
    }

    void DoMonitor(bool isStart)
    {
      if (isStart)
      {
        Clipboard.Clear();
        nextClipboardViewer = (IntPtr)SetClipboardViewer((int)this.Handle);
      }
      else
      {
        ChangeClipboardChain(this.Handle, nextClipboardViewer);
      }
    }

    string ProcessWord(string word)
    {
      var trimmed = word.Trim(new char[] {' ', '.', ','} );
      return trimmed.ToLower();
    }

    private void OnClipboardChanged()
    {
      var data = Clipboard.GetDataObject();
      if (data.GetDataPresent(DataFormats.Text))
      {
        string cpdata = data.GetData(DataFormats.Text).ToString();

        var handler = ClipboardChanged;
        if (handler != null)
        {
          handler(this, new ClipboardEventArgs(cpdata));
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
          OnClipboardChanged();
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
      btnStart.Enabled = false;
      btnStop.Enabled = true;
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
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
