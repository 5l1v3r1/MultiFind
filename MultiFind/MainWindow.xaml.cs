﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace MultiFind {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window , INotifyPropertyChanged {

    //ConcurrentQueue<Hits> hits = new ConcurrentQueue<Hits>();
    ObservableCollection<Hits> hits = new ObservableCollection<Hits>();
    bool run = false;
    private bool eachDirNewThread;
    ConcurrentDictionary<int,string> runningTasks = new ConcurrentDictionary<int,string>();
    List<string> searchPaths=new List<string> ();
    private Timer timer;
    private Stopwatch stopwatch;
    private int workerThreads;
    private int competionPortThreads;
    private int workerThreadsMax;
    private int competionPortThreadsMax;
    private int workerThreadsAvailable;
    private int completionThreadsAvailable;
    static long fileCount;
    static long dirCount;

    public MainWindow() {
      InitializeComponent();
      HitsDataGrid.ItemsSource = hits;
      searchPaths.AddRange(GetLogicalDrives());
      /*foreach (var s in searchPaths)
        runningTasks.Add(s);*/
      PathList.ItemsSource = runningTasks;

    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(String propertyName = "") {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
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
        runningTasks[thread] = new string(startPath.Take(70).ToArray()) ;
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

      NotifyPropertyChanged("Values");

      if (files != null)
        foreach (var file in files)
          Dispatcher.Invoke(() => {
            hits.Add(new Hits(startPath, file));
            fileCount++;
          });

      if (directories != null)
        foreach (var dir in directories)
          if (eachDirNewThread)
            ThreadPool.QueueUserWorkItem(o => {
              RecursiveFind(dir, keyword, driveIndex);
              dirCount++;
            });
          else {
            RecursiveFind(dir, keyword, driveIndex);
            dirCount++;
          }

      if (directories==null || directories.Length==0)
      //Dispatcher.Invoke(() => {
        //runningTasks[driveIndex] = searchPaths[driveIndex] + " done";
        runningTasks[thread]="";
        //if (runningTasks.ContainsKey(thread))
          //runningTasks.Remove(thread);
      //});

    }

    private void Button_Click(object sender, RoutedEventArgs e) {
      if (run)
        return;
      hits.Clear();
      run = true;

      eachDirNewThread = NewThreadCheckBox.IsChecked == true;

      runningTasks.Clear();
      /*foreach (var s in searchPaths)
        runningTasks.Add(s);*/
      //PathList.ItemsSource = null;
      PathList.ItemsSource = runningTasks.Values;

      string filter;
      if (!KeywordTextBox.Text.Contains("*")||
          !KeywordTextBox.Text.Contains("?"))
        filter = "*" + KeywordTextBox.Text + "*";
      else
        filter = KeywordTextBox.Text;
      int i = 0;
      foreach (var drive in searchPaths)
        ThreadPool.QueueUserWorkItem(o => RecursiveFind(drive, filter, i++));

      fileCount = dirCount = 0;
      timer = new Timer(timerCallback, null, 20, 20);
      stopwatch = new Stopwatch();
      stopwatch.Start();
    }

    private void timerCallback(object state) {
      ThreadPool.GetMaxThreads(out workerThreadsMax, out competionPortThreadsMax);
      ThreadPool.GetAvailableThreads(out workerThreadsAvailable, out completionThreadsAvailable);
      Dispatcher.Invoke(() => {
        Title = stopwatch.Elapsed.ToString(@"hh\:mm\:ss")
        + " Threads: " + (workerThreadsMax - workerThreadsAvailable)
        + " Hits: " + fileCount
        + " Dirs: " + dirCount
        + " DPS: " + (int) (dirCount / stopwatch.Elapsed.TotalSeconds);
        //NotifyCollectionChanged()
        PathList.ItemsSource = runningTasks.Values;
        NotifyPropertyChanged("runningTasks");
        //PathList.Items.Refresh();
      });
      if (workerThreadsMax - workerThreadsAvailable == 1) {
        timer.Dispose();
        run = false;
      }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e) {
      run = false;
    }


    private void HitsDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
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
    }

    private void KeywordTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
      if (e.Key == System.Windows.Input.Key.Enter)
        Button_Click(null, null);
    }
  }
}
