using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FlightgearSimulator.Commands
{
    public class ButtonClickCommand : ICommand
    {
        // the members
        private Action action;

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        /// <param name="action"></param>
        public ButtonClickCommand(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// this function return if can execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// this function execute.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            action();
        }
    }
}
