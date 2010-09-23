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
  public partial class MainForm : Form
  {
    private IntPtr _nextClipboardViewer;

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
        SetupMonitor(val);
      });

      var clipboard = from start in startClicks
                      from cb in cbChanged.TakeUntil(stopClicks)
                      select cb;

      clipboard.Select(e => e.EventArgs.Data).DistinctUntilChanged().Subscribe(str => lbxWords.Items.Add(str));
    }

    private void SetupMonitor(bool isStart)
    {
      if (isStart)
      {
        Clipboard.Clear();
        _nextClipboardViewer = (IntPtr)NativeHelpers.SetClipboardViewer((int)this.Handle);
      }
      else
      {
        NativeHelpers.ChangeClipboardChain(this.Handle, _nextClipboardViewer);
      }
    }

    private string ProcessWord(string word)
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

    protected override void WndProc(ref System.Windows.Forms.Message message)
    {
      const int WM_DRAWCLIPBOARD = 0x308;
      const int WM_CHANGECBCHAIN = 0x030D;

      switch (message.Msg)
      {
        case WM_DRAWCLIPBOARD:
          OnClipboardChanged();
          NativeHelpers.SendMessage(_nextClipboardViewer, message.Msg, message.WParam, message.LParam);
          break;

        case WM_CHANGECBCHAIN:
          if (message.WParam == _nextClipboardViewer)
          {
            _nextClipboardViewer = message.LParam;
          }
          else
          {
            NativeHelpers.SendMessage(_nextClipboardViewer, message.Msg, message.WParam, message.LParam);
          }
          break;

        default:
          base.WndProc(ref message);
          break;
      }
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
