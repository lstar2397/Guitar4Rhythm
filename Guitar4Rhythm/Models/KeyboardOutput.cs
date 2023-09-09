using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Guitar4Rhythm.Models {
    public class KeyboardOutput<T> : INotifyPropertyChanged {
        private T? _key1;
        public T? Key1 {
            get => _key1;
            set {
                _key1 = value;
                OnPropertyChanged();
            }
        }

        private T? _key2;
        public T? Key2 {
            get => _key2;
            set {
                _key2 = value;
                OnPropertyChanged();
            }
        }

        private T? _key3;
        public T? Key3 {
            get => _key3;
            set {
                _key3 = value;
                OnPropertyChanged();
            }
        }

        private T? _key4;
        public T? Key4 {
            get => _key4;
            set {
                _key4 = value;
                OnPropertyChanged();
            }
        }

        private T? _key5;
        public T? Key5 {
            get => _key5;
            set {
                _key5 = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
