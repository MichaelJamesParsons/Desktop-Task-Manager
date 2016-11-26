using System.Windows.Input;

namespace TimeTracker.Commands
{
    /// <summary>
    /// Source provided by Jake Ginnivan's article on delegate commands.
    /// http://jake.ginnivan.net/awaitable-delegatecommand/
    /// </summary>
    public interface IRaiseCanExecuteChanged
    {
        void RaiseCanExecuteChanged();
    }

    public static class CommandExtensions
    {
        public static void RaiseCanExecuteChanged(this ICommand command)
        {
            var canExecuteChanged = command as IRaiseCanExecuteChanged;

            if (canExecuteChanged != null)
                canExecuteChanged.RaiseCanExecuteChanged();
        }
    }
}
