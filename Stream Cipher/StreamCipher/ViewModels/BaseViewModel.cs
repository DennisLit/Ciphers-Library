using PropertyChanged;
using System.ComponentModel;

namespace StreamCipher.Core 
{
    [AddINotifyPropertyChangedInterface]

    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}

