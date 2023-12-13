using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlantGenius.GUI.Commands
{
    /// <summary>
    /// When a comman is invoked the two methods 1. CanExecute and 2. Execute will be executed. 
    /// The class will help us to point us to the method to invoke.
    /// There is no further logic in this class.
    /// </summary>
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        // Contructor which initalizes the Action and Predicate.
        public RelayCommand(Action<object> ExecuteMethod, Predicate<object> CanExecuteMethod) 
        { 
            _Execute = ExecuteMethod;
            _CanExecute = CanExecuteMethod;
        }

        // method which returns void.
        private Action<object> _Execute { get; set; }

        //method which returns a boolean.
        private Predicate<object> _CanExecute { get; set; }

        /// <summary>
        /// When we invoke a command we can pass an object.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {
            return _CanExecute(parameter);
        }

        /// <summary>
        /// Executes whatever the user provide.
        /// </summary>
        /// <param name="parameter"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Execute(object? parameter)
        {
            _Execute(parameter);
        }
    }
}
