﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiFind {
  class Hits {
    public string path { get { return _path; } }
    public string filename { get { return _filename; } }
    private string _path;
    private string _filename;

    public Hits(string path, string filename) {
      this._path = path;
      try {
        this._filename = Path.GetFileName(filename);
      }
      catch (Exception e) { this._filename = filename; };
    }

  }
}
