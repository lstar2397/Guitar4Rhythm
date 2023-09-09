using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Guitar4Rhythm.Controls {
    public class KeyBindingBox : TextBox {
        public static readonly DependencyProperty BoundKeyProperty =
            DependencyProperty.Register(
                nameof(BoundKey),
                typeof(Key),
                typeof(KeyBindingBox),
                new FrameworkPropertyMetadata(Key.None, new PropertyChangedCallback(BoundKeyProperty_Changed)));

        public Key BoundKey {
            get => (Key)GetValue(BoundKeyProperty);
            set => SetValue(BoundKeyProperty, value);
        }

        private static void BoundKeyProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            if (d is KeyBindingBox box) {
                box.Text = box.BoundKey.ToString();
            }
        }

        public string _listenerText = "...";
        public string ListenerText {
            get => _listenerText;
            set => _listenerText = value;
        }

        private bool _isListening = false;
        public bool IsListening {
            get => _isListening;
            set {
                _isListening = value;
                if (_isListening) {
                    Text = ListenerText;
                } else {
                    Text = BoundKey.ToString();
                }
            }
        }

        public KeyBindingBox() {
            IsReadOnly = true;
            PreviewMouseLeftButtonDown += (s, e) => {
                IsListening = !IsListening;
            };
            PreviewKeyDown += KeyBindingBox_PreviewKeyDown;
            LostFocus += (s, e) => IsListening = false;
        }

        private void KeyBindingBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (!IsListening) return;

            Key key = e.Key;
            BoundKey = (key == Key.Escape) ? Key.None : key;
            IsListening = false;
            e.Handled = true;
        }
    }
}
