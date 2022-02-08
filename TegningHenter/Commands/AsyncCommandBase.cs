using System.Threading.Tasks;

namespace Commands
{
    /// <summary>
    /// Lavet af Simon
    /// </summary>
    public abstract class AsyncCommandBase : CommandBase
    {
        private bool _isExecuting;

        public bool IsExecuting
        {
            get { return _isExecuting; }
            set
            {
                _isExecuting = value;
                OnCanExecuteChanged();
            }
        }
        public override bool CanExecute(object parameter)
        {
            return !IsExecuting && base.CanExecute(parameter);
        }
        public override async void Execute(object parameter)
        {
            IsExecuting = true;
            try
            {
                await ExecuteAsync(parameter);
            }
            finally
            {
                IsExecuting = false;
            }
        }
        public abstract Task ExecuteAsync(object parameter);
    }
}
