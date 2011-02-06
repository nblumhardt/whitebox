// This file is adapted from the Magellan Framework
// Copyright (c) Paul Stovell
// Used under the MIT license
// http://magellan-framework.googlecode.com

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Whitebox.Profiler.Util
{
    /// <summary>
    /// An <see cref="ICommand"/> that invokes a delegate on execution.
    /// </summary>
    public class RelayCommand : RelayCommand<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action execute)
            : base(x => execute())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
            : base(x => execute(), x => canExecute())
        {
        }
    }

    /// <summary>
    /// A generic <see cref="ICommand"/> that invokes a delegate on execution.
    /// </summary>
    public class RelayCommand<TArgument> : ICommand
    {
        private readonly Action<TArgument> execute;
        private readonly Func<TArgument, bool> canExecute;
        private EventHandler canExecuteChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;TArgument&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action<TArgument> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand&lt;TArgument&gt;"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        /// <param name="canExecute">The can execute.</param>
        public RelayCommand(Action<TArgument> execute, Func<TArgument, bool> canExecute)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (parameter == null && typeof(TArgument).IsValueType)
                return false;

            return canExecute == null ? true : canExecute((TArgument)parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                canExecuteChanged += value;
#if !SILVERLIGHT
                CommandManager.RequerySuggested += value;
#endif
            }
            remove
            {
                canExecuteChanged -= value;
#if !SILVERLIGHT
                CommandManager.RequerySuggested -= value;
#endif
            }
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            var handler = canExecuteChanged;
            if (handler != null) handler(this, new EventArgs());
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        [DebuggerStepThrough]
        public void Execute(object parameter)
        {
            if (parameter == null && typeof(TArgument).IsValueType)
                return;

            execute((TArgument)parameter);
        }
    }
}