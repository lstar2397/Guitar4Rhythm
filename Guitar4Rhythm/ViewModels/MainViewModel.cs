using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using Guitar4Rhythm.Hooks;
using Guitar4Rhythm.Models;
using Guitar4Rhythm.WindowsInput;
using Guitar4Rhythm.WindowsInput.Native;
using Newtonsoft.Json;

namespace Guitar4Rhythm.ViewModels {
    internal class MainViewModel : INotifyPropertyChanged {
        private GuitarController<Key> _guitarControllerKeys;
        public GuitarController<Key> GuitarControllerKeys {
            get => _guitarControllerKeys;
            set {
                _guitarControllerKeys = value;
                OnPropertyChanged();
            }
        }

        private KeyboardOutput<Key> _keyboardOutputKeys;
        public KeyboardOutput<Key> KeyboardOutputKeys {
            get => _keyboardOutputKeys;
            set {
                _keyboardOutputKeys = value;
                OnPropertyChanged();
            }
        }

        public GuitarController<Brush> _guitarControllerBackgroundColors;
        public GuitarController<Brush> GuitarControllerBackgroundColors {
            get => _guitarControllerBackgroundColors;
            set {
                _guitarControllerBackgroundColors = value;
                OnPropertyChanged();
            }
        }

        public KeyboardOutput<Brush> _keyboardOutputBackgroundColors;
        public KeyboardOutput<Brush> KeyboardOutputBackgroundColors {
            get => _keyboardOutputBackgroundColors;
            set {
                _keyboardOutputBackgroundColors = value;
                OnPropertyChanged();
            }
        }

        private KeyboardInputType _selectedKeyboardInputType = KeyboardInputType.VirtualKeyCode;
        public KeyboardInputType SelectedKeyboardInputType {
            get => _selectedKeyboardInputType;
            set {
                _selectedKeyboardInputType = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand StartCommand { get; set; }

        public RelayCommand StopCommand { get; set; }

        private string _filePath = "config.json";
        private KeyboardHook _keyboardHook;
        private Dictionary<Key, bool> _isKeyDown = new Dictionary<Key, bool>();
        private KeyboardInputSimulator _inputSimulator;

        public MainViewModel() {
            LoadKeys();
            GuitarControllerKeys.PropertyChanged += (s, e) => SaveKeys();
            KeyboardOutputKeys.PropertyChanged += (s, e) => SaveKeys();

            _guitarControllerBackgroundColors = new GuitarController<Brush>();
            _keyboardOutputBackgroundColors = new KeyboardOutput<Brush>();

            _keyboardHook = new KeyboardHook();
            _keyboardHook.KeyBlocking += _keyboardHook_KeyBlocking;

            _inputSimulator = new KeyboardInputSimulator();

            StartCommand = new RelayCommand(o => _keyboardHook.Hook(), o => !_keyboardHook.IsHooked);
            StopCommand = new RelayCommand(o => _keyboardHook.Unhook(), o => _keyboardHook.IsHooked);
        }

        private bool IsKeyDown(Key key) => _isKeyDown.TryGetValue(key, out bool isKeyDown) && isKeyDown;

        private bool _keyboardHook_KeyBlocking(Key key, Hooks.KeyStates keyStates) {
            bool isGuitarControllerKey = false;
            if (key == GuitarControllerKeys.GreenKey) {
                GuitarControllerBackgroundColors.GreenKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
                isGuitarControllerKey = true;
            }
            if (key == GuitarControllerKeys.RedKey) {
                GuitarControllerBackgroundColors.RedKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
                isGuitarControllerKey = true;
            }
            if (key == GuitarControllerKeys.YellowKey) {
                GuitarControllerBackgroundColors.YellowKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
                isGuitarControllerKey = true;
            }
            if (key == GuitarControllerKeys.BlueKey) {
                GuitarControllerBackgroundColors.BlueKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
                isGuitarControllerKey = true;
            }
            if (key == GuitarControllerKeys.OrangeKey) {
                GuitarControllerBackgroundColors.OrangeKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
                isGuitarControllerKey = true;
            }
            if (key == GuitarControllerKeys.StrumUpKey) {
                GuitarControllerBackgroundColors.StrumUpKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
                isGuitarControllerKey = true;
            }
            if (key == GuitarControllerKeys.StrumDownKey) {
                GuitarControllerBackgroundColors.StrumDownKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
                isGuitarControllerKey = true;
            }

            if (!isGuitarControllerKey) return false;


            if (keyStates == Hooks.KeyStates.Down) {
                if ((key == GuitarControllerKeys.StrumUpKey && !IsKeyDown(GuitarControllerKeys.StrumUpKey)) ||
                    (key == GuitarControllerKeys.StrumDownKey && !IsKeyDown(GuitarControllerKeys.StrumDownKey))) {
                    List<Key> keyList = new List<Key>();
                    if (IsKeyDown(GuitarControllerKeys.GreenKey)) {
                        KeyboardOutputBackgroundColors.Key1 = Brushes.LightGreen;
                        keyList.Add(KeyboardOutputKeys.Key1);
                        _isKeyDown[KeyboardOutputKeys.Key1] = true;
                    }
                    if (IsKeyDown(GuitarControllerKeys.RedKey)) {
                        KeyboardOutputBackgroundColors.Key2 = Brushes.LightGreen;
                        keyList.Add(KeyboardOutputKeys.Key2);
                        _isKeyDown[KeyboardOutputKeys.Key2] = true;
                    }
                    if (IsKeyDown(GuitarControllerKeys.YellowKey)) {
                        KeyboardOutputBackgroundColors.Key3 = Brushes.LightGreen;
                        keyList.Add(KeyboardOutputKeys.Key3);
                        _isKeyDown[KeyboardOutputKeys.Key3] = true;
                    }
                    if (IsKeyDown(GuitarControllerKeys.BlueKey)) {
                        KeyboardOutputBackgroundColors.Key4 = Brushes.LightGreen;
                        keyList.Add(KeyboardOutputKeys.Key4);
                        _isKeyDown[KeyboardOutputKeys.Key4] = true;
                    }
                    if (IsKeyDown(GuitarControllerKeys.OrangeKey)) {
                        KeyboardOutputBackgroundColors.Key5 = Brushes.LightGreen;
                        keyList.Add(KeyboardOutputKeys.Key5);
                        _isKeyDown[KeyboardOutputKeys.Key5] = true;
                    }

                    if (keyList.Count > 0) {
                        _inputSimulator.KeyUp(SelectedKeyboardInputType, keyList.ToArray());
                        Thread.Sleep(5);
                        _inputSimulator.KeyDown(SelectedKeyboardInputType, keyList.ToArray());
                    }
                }
            } else {
                List<Key> keyList = new List<Key>();
                if (key == GuitarControllerKeys.GreenKey && IsKeyDown(GuitarControllerKeys.GreenKey)) {
                    KeyboardOutputBackgroundColors.Key1 = Brushes.Transparent;
                    keyList.Add(KeyboardOutputKeys.Key1);
                    _isKeyDown[KeyboardOutputKeys.Key1] = false;
                }
                if (key == GuitarControllerKeys.RedKey && IsKeyDown(GuitarControllerKeys.RedKey)) {
                    KeyboardOutputBackgroundColors.Key2 = Brushes.Transparent;
                    keyList.Add(KeyboardOutputKeys.Key2);
                    _isKeyDown[KeyboardOutputKeys.Key2] = false;
                }
                if (key == GuitarControllerKeys.YellowKey && IsKeyDown(GuitarControllerKeys.YellowKey)) {
                    KeyboardOutputBackgroundColors.Key3 = Brushes.Transparent;
                    keyList.Add(KeyboardOutputKeys.Key3);
                    _isKeyDown[KeyboardOutputKeys.Key3] = false;
                }
                if (key == GuitarControllerKeys.BlueKey && IsKeyDown(GuitarControllerKeys.BlueKey)) {
                    KeyboardOutputBackgroundColors.Key4 = Brushes.Transparent;
                    keyList.Add(KeyboardOutputKeys.Key4);
                    _isKeyDown[KeyboardOutputKeys.Key4] = false;
                }
                if (key == GuitarControllerKeys.OrangeKey && IsKeyDown(GuitarControllerKeys.OrangeKey)) {
                    KeyboardOutputBackgroundColors.Key5 = Brushes.Transparent;
                    keyList.Add(KeyboardOutputKeys.Key5);
                    _isKeyDown[KeyboardOutputKeys.Key5] = false;
                }

                if (keyList.Count > 0) {
                    _inputSimulator.KeyUp(SelectedKeyboardInputType, keyList.ToArray());
                }
            }

            _isKeyDown[key] = keyStates == Hooks.KeyStates.Down;

            return true;
        }

        public void LoadKeys() {
            if (File.Exists(_filePath)) {
                string json = File.ReadAllText(_filePath);
                var keys = JsonConvert.DeserializeAnonymousType(json, new {
                    GuitarControllerKeys,
                    KeyboardOutputKeys
                });

                GuitarControllerKeys = keys.GuitarControllerKeys;
                KeyboardOutputKeys = keys.KeyboardOutputKeys;
            } else {
                GuitarControllerKeys = new GuitarController<Key>();
                KeyboardOutputKeys = new KeyboardOutput<Key>();
            }
        }

        public void SaveKeys() {
            string json = JsonConvert.SerializeObject(new {
                GuitarControllerKeys,
                KeyboardOutputKeys
            }, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnClosing(object? sender, CancelEventArgs e) {
            if (_keyboardHook.IsHooked) {
                _keyboardHook.Unhook();
            }
        }
    }
}
