using System.ComponentModel;
using System.Runtime.CompilerServices;
using TimeTracker.Annotations;

namespace TimeTracker.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event handler to be executed when the property of a
        /// child viewmodel is updated.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
