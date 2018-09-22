using PropertyChanged;
using System.ComponentModel;

namespace StreamCipher 
{
    [AddINotifyPropertyChangedInterface]

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}

