using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFind {
  class StringWithNotify : INotifyPropertyChanged {
    static int lastUpdateTicker;
    private string _str;
      public string str {
      get { return _str; }
      set {
        if (value != _str) {
          _str = value;
          //if (Environment.TickCount > lastUpdateTicker) {
            NotifyPropertyChanged("str");
           // lastUpdateTicker = Environment.TickCount + 10;
          //}
        }
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(String propertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

  }
}
