using MultithreadingWPF.Model;
using MultithreadingWPF.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace MultithreadingWPF.ViewModel {
  public class DataViewModel : INotifyPropertyChanged {
    private const int WATCHINTERVAL = 100;
    private ThreadHelper _threadHelper = new ThreadHelper();
    private List<Data> _dataSet = new List<Data>();
    private string _resultBoxText = "";
    private Task _watchTask;

    private delegate void SetResultsBoxTextDelegate(string resultsBoxText);
    private SetResultsBoxTextDelegate _setResultsBoxTextDelegate;

    public string ResultBoxText {
      get { return _resultBoxText; }
      set {
        _resultBoxText = value;
        OnPropertyChanged("ResultBoxText");
      }
    }

    private void SetResultsBoxText(string resultsBoxText) {
      ResultBoxText = resultsBoxText;
    }



    public DataViewModel() {
      //_dispatcher = dispatcher;
      //_setResultsBoxTextDelegate = new SetResultsBoxTextDelegate(this.SetResultsBoxText);
      Thread watchResultThread = new Thread(this.WatchResults);
      watchResultThread.Start();

      _watchTask = Task.Factory.StartNew(WatchResults);



    }


    private ICommand _executeCommand;
    public ICommand ExecuteCommand {
      get {
        return _executeCommand ?? (_executeCommand = new CommandHandler(() => Execute(), true));
      }
    }

    public void WatchResults() {
      //todo: do we want an exit condition?


      while (true) {
        Thread.Sleep(WATCHINTERVAL);
        string newResultBoxText = "";
        foreach (Data data in _dataSet) {
          if (data.PercentPropgress != 100) {
            newResultBoxText += $"Progressing task.  {data.PercentPropgress} percent complete/n";
          }
          else {
            newResultBoxText += $"Task complete.  {data.Result}/n";
          }
        }

        //dispatch
        //_dispatcher.RunAsync(1, _setResultsBoxTextDelegate);
        ResultBoxText = newResultBoxText;
      }

    }


    public void Execute() {
      Data addData = new Data();
      addData.Delay = 1000;
      addData.AddInputValue(10);
      addData.AddInputValue(100);
      addData.AddInputValue(2);
      addData.AddInputValue(15);
      addData.AddInputValue(222);



      _dataSet.Add(addData);


      //set up one to make sure working
      //_threadHelper.StartThread(new Adder(), _dataSet[0]);
      Adder adder = new Adder();

      Task addTask = Task.Factory.StartNew(() => adder.Execute(_dataSet[0]));


      //todo:  let's give maybe 10 thread choices on the app
      //        eg none/option 1/ option 2/ option 3 etc
      //  and for each one chosen as not none, we execute and show results as they happen (progress report, and then final value when done)
      //on each execute, recreate and run threads.
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propertyName) {
      if (PropertyChanged != null) {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }

  public class CommandHandler : ICommand {
    private Action _commandAction;
    private bool _commandCanExecute;
    public CommandHandler(Action commandAction, bool commandCanExecute) {
      _commandAction = commandAction;
      _commandCanExecute = commandCanExecute;
    }

    public bool CanExecute(object parameter) {
      return _commandCanExecute;
    }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter) {
      _commandAction();
    }
  }
}
