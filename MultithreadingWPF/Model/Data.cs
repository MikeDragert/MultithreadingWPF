using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingWPF.Model {
  public class Data {
    private string _result = "";
    private List<int> _inputValues = new List<int>();
    private int _percentProgress = 0;
    private int _timeUsed = 0;
    private Object _percenProgressLock = new Object();
    private Object _dataLock = new Object();


    public string Result {
      get { return _result; }
    }

    public int PercentPropgress {
      get { return _percentProgress; }
    }
    public int Delay { get; set; }

    public IList<int> InputValues {
      //get { return _inputValues.AsReadOnly(); }  //doesn't like readonly
      get { return _inputValues; }
    }

    public Data() {
      Reset();
    }

    public void AddInputValue(int value) {
      _inputValues.Add(value);
    }

    public void ClearInputValues() {
      _inputValues.Clear();
    }

    public void ClearResult() {
      _result = "";
    }

    public void Reset() {
      _result = "";
      _inputValues.Clear();
      _percentProgress = 0;
      _timeUsed = 0;
    }

    private void UpdatePercentProgress(int percentProgress) {
      lock (_percenProgressLock) {
        _percentProgress = percentProgress;
      }
    }

    public void ProcessData(Func<int, int, int> callback) {
      ClearResult();
      if (_inputValues.Count == 0) return;
      int accum = _inputValues[0];
      for (int index = 1; index < _inputValues.Count; index++) {
        UpdatePercentProgress((100 * index) / _inputValues.Count);
        Thread.Sleep(Delay);
        _timeUsed += Delay;
        lock (_dataLock) {
          accum = callback(accum, _inputValues[index]);
        }
      }
      _result = $"{accum} in {_timeUsed} time.";
      UpdatePercentProgress(100);
    }
  }
}
