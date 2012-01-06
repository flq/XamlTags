using System;
using System.Windows.Input;

namespace DynamicXaml.MarkupSystem
{
	public class RelayCommand : ICommand
	{
		private readonly Func<object, bool> _canExecute;
		private readonly Action<object> _execute;

		public RelayCommand(Func<object, bool> canExecute, Action<object> execute)
		{
			_canExecute = canExecute;
			_execute = execute;
		}

		public RelayCommand(Action<object> execute) : this(_ => true, execute) { }

		public RelayCommand(Action execute) : this(_ => true, _ => execute()) { }

		public RelayCommand(Func<bool> canExecute, Action execute) : this(_ => canExecute(), _ => execute()) { }

		public void Execute(object parameter)
		{
			_execute(parameter);
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute(parameter);
		}

		public void RaiseCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
	}
}