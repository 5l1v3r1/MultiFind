using MultiFind;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiFindWindowsForms {
  public partial class Form1 : Form {
    //ConcurrentQueue<Hits> hits = new ConcurrentQueue<Hits>();
    ObservableCollection<Hits> hits = new ObservableCollection<Hits>();
    bool run = false;
    ConcurrentDictionary<int, string> runningTasks = new ConcurrentDictionary<int, string>();
    List<string> searchPaths = new List<string>();
    private System.Threading.Timer timer;
    private Stopwatch stopwatch;
    private int workerThreads;
    private int competionPortThreads;
    private int workerThreadsMax;
    private int competionPortThreadsMax;
    private int workerThreadsAvailable;
    private int completionThreadsAvailable;
    static long fileCount;
    static long dirCount;

    public Form1() {
      InitializeComponent();
      searchPaths.AddRange(GetLogicalDrives());
      /*foreach (var s in searchPaths)
        runningTasks.Add(s);*/
    }



    string[] GetLogicalDrives() {
      string[] drives = System.IO.Directory.GetLogicalDrives();
      return drives;
    }


    void RecursiveFind(string startPath, string keyword, int driveIndex) {

      string[] files = null;
      string[] directories = null;

      if (!run)
        return;

      var thread = Thread.CurrentThread.ManagedThreadId;

      //Dispatcher.Invoke(() => {
      //StatusLabel.Content = startPath;
      //runningTasks[driveIndex] = startPath;
      //if (!runningTasks.ContainsKey(thread))
      //runningTasks.Add(thread, new StringWithNotify());
      runningTasks[thread] = new string(startPath.Take(70).ToArray());
      //CollectionViewSource.GetDefaultView(PathList)?.Refresh();        
      //});

      try {
        files = Directory.GetFileSystemEntries(startPath, keyword);
        directories = Directory.GetDirectories(startPath, "*.*");
      }
      catch (Exception e) {
        //files = new string[1];
        //files[0] = e.Message;
      }

      if (files != null)
        foreach (var file in files) {
          hits.Add(new Hits(startPath, file));
          fileCount++;
        }
 
      if (directories != null)
        foreach (var dir in directories)
          ThreadPool.QueueUserWorkItem(o => {
            RecursiveFind(dir, keyword, driveIndex);
            dirCount++;
          });

      if (directories == null || directories.Length == 0)
        //Dispatcher.Invoke(() => {
        //runningTasks[driveIndex] = searchPaths[driveIndex] + " done";
        runningTasks[thread] = "";
      //if (runningTasks.ContainsKey(thread))
      //runningTasks.Remove(thread);
      //});

    }

    private void button1_Click(object sender, EventArgs e) {
      if (run)
        return;
      hits.Clear();
      run = true;

      runningTasks.Clear();
      /*foreach (var s in searchPaths)
        runningTasks.Add(s);*/
      //PathList.ItemsSource = null;
      PathList.Clear();
      //PathList.Items = runningTasks.Values;
      PathList.View = View.List;

      string filter;
      if (!KeywordTextBox.Text.Contains("*") ||
          !KeywordTextBox.Text.Contains("?"))
        filter = "*" + KeywordTextBox.Text + "*";
      else
        filter = KeywordTextBox.Text;
      int i = 0;
      foreach (var drive in searchPaths)
        ThreadPool.QueueUserWorkItem(o => RecursiveFind(drive, filter, i++));

      fileCount = dirCount = 0;
      timer = new System.Threading.Timer(timerCallback, null, 50, 50);
      stopwatch = new Stopwatch();
      stopwatch.Start();
    }

    private void timerCallback(object state) {
      ThreadPool.GetMaxThreads(out workerThreadsMax, out competionPortThreadsMax);
      ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionThreadsAvailable);
      /*Dispatcher.Invoke(() => {
        Title = stopwatch.Elapsed.ToString(@"hh\:mm\:ss")
        + " Threads: " + (workerThreadsMax - workerThreadsAvailable)
        + " Hits: " + fileCount
        + " Dirs: " + dirCount
        + " DPS: " + (int)(dirCount / stopwatch.Elapsed.TotalSeconds);
        //NotifyCollectionChanged()
        PathList.ItemsSource = runningTasks.Values;
        NotifyPropertyChanged("runningTasks");
        //PathList.Items.Refresh();
      });*/

      PathList.Invoke((MethodInvoker)delegate {
        Text = stopwatch.Elapsed.ToString(@"hh\:mm\:ss")
        + " Threads: " + (workerThreadsMax - workerThreadsAvailable)
        + " Hits: " + fileCount
        + " Dirs: " + dirCount
        + " DPS: " + (int)(dirCount / stopwatch.Elapsed.TotalSeconds);

        //PathList.Clear();
        for(int i=0; i < runningTasks.Count; i++)
          if (PathList.Items.Count <= i)
            PathList.Items.Add(runningTasks.ElementAt(i).Value);
          else
            PathList.Items[i].Text= runningTasks.ElementAt(i).Value;
      });

      if (workerThreadsMax - workerThreadsAvailable == 1) {
        timer.Dispose();
        run = false;
      }
    }
    

    private void button2_Click(object sender, EventArgs e) {
      run = false;
    }


    /*private void HitsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
      try {
        Hits dataRow = (Hits)HitsDataGrid.SelectedItem;
        int index = HitsDataGrid.CurrentCell.Column.DisplayIndex;
        Console.WriteLine(index);
        Console.WriteLine(dataRow);
        if (index == 0)
          System.Diagnostics.Process.Start(dataRow.path);
        else
          System.Diagnostics.Process.Start(dataRow.path + '\\' + dataRow.filename);
      }
      catch (Exception ex) { MessageBox.Show(ex.Message); }
    }*/
  }
}
