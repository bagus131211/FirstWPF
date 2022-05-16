namespace UX.Test
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    public class Test : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string _someProperty;

        public string SomeProperty
        {
            get => _someProperty;
            set
            {
                if (value == _someProperty)
                    return;
                _someProperty = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
