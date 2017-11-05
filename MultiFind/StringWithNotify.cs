using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFind {
  class StringWithNotify : INotifyPropertyChanged {
    private string _str;
      public string str {
      get { return _str; }
      set {
        if (value != _str) {
          _str = value;
          NotifyPropertyChanged("str");
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
