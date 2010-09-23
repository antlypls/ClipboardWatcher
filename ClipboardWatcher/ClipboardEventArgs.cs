using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
