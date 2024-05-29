using MultithreadingWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultithreadingWPF.Helpers {
  public class ThreadHelper {

    private List<Thread> _threads = new List<Thread>();

    public ThreadHelper() {
    }

    public void StartThread(IExecutor executor, Data _data) {
      _threads.Add(new Thread(executor.Execute));
      _threads[_threads.Count - 1].Start(_data);
    }

  }
}
