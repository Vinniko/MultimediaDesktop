using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace DesktopBd.MVVM
{
        [Serializable]
        public class BaseVM : INotifyPropertyChanged
        {
            [field: NonSerialized]
            public event PropertyChangedEventHandler PropertyChanged;

            public void NotifyPropertyChanged([CallerMemberName] string prop = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }
}
