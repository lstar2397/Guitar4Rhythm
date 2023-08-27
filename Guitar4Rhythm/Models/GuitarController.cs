using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Guitar4Rhythm.Models {
    public class GuitarController<T> : INotifyPropertyChanged {
        private T _greenKey;
        public T GreenKey {
            get => _greenKey;
            set {
                _greenKey = value;
                OnPropertyChanged();
            }
        }

        private T _redKey;
        public T RedKey {
            get => _redKey;
            set {
                _redKey = value;
                OnPropertyChanged();
            }
        }

        private T _yellowKey;
        public T YellowKey {
            get => _yellowKey;
            set {
                _yellowKey = value;
                OnPropertyChanged();
            }
        }

        private T _blueKey;
        public T BlueKey {
            get => _blueKey;
            set {
                _blueKey = value;
                OnPropertyChanged();
            }
        }

        private T _orangeKey;
        public T OrangeKey {
            get => _orangeKey;
            set {
                _orangeKey = value;
                OnPropertyChanged();
            }
        }

        private T _strumUpKey;
        public T StrumUpKey {
            get => _strumUpKey;
            set {
                _strumUpKey = value;
                OnPropertyChanged();
            }
        }

        private T _strumDownKey;
        public T StrumDownKey {
            get => _strumDownKey;
            set {
                _strumDownKey = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
