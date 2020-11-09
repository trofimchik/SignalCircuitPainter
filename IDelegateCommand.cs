using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace SignalCircuitPainter
{
    interface IDelegateCommand : ICommand
    {
        void RaiseCanExecuteChanged();
    }
}
