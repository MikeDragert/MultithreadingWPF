using MultithreadingWPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadingWPF.Helpers {
  public class Adder : IExecutor {
    public void Execute(object data) {
      if (data == null) return;
      if (data is Data) {
        Data checkedData = (Data)data;

        checkedData.ProcessData((x, y) => x + y);
      }
    }
  }
}
