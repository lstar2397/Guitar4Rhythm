using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Guitar4Rhythm.Hooks;
using Guitar4Rhythm.Models;
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

        private string _filePath = "config.json";
        private KeyboardHook _keyboardHook;
        private Dictionary<Key, bool> _isKeyDown = new Dictionary<Key, bool>();

        public MainViewModel() {
            LoadKeys();
            GuitarControllerKeys.PropertyChanged += (s, e) => SaveKeys();
            KeyboardOutputKeys.PropertyChanged += (s, e) => SaveKeys();

            _guitarControllerBackgroundColors = new GuitarController<Brush>();
            _keyboardOutputBackgroundColors = new KeyboardOutput<Brush>();

            _keyboardHook = new KeyboardHook();
            _keyboardHook.KeyBlocking += _keyboardHook_KeyBlocking;
            _keyboardHook.Hook();
        }

        private bool IsKeyDown(Key key) {
            if (_isKeyDown.ContainsKey(key)) {
                return _isKeyDown[key];
            }

            return false;
        }

        private bool _keyboardHook_KeyBlocking(Key key, Hooks.KeyStates keyStates) {
            if (key == GuitarControllerKeys.GreenKey) {
                GuitarControllerBackgroundColors.GreenKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
            } else if (key == GuitarControllerKeys.RedKey) {
                GuitarControllerBackgroundColors.RedKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
            } else if (key == GuitarControllerKeys.YellowKey) {
                GuitarControllerBackgroundColors.YellowKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
            } else if (key == GuitarControllerKeys.BlueKey) {
                GuitarControllerBackgroundColors.BlueKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
            } else if (key == GuitarControllerKeys.OrangeKey) {
                GuitarControllerBackgroundColors.OrangeKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
            } else if (key == GuitarControllerKeys.StrumUpKey) {
                GuitarControllerBackgroundColors.StrumUpKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
            } else if (key == GuitarControllerKeys.StrumDownKey) {
                GuitarControllerBackgroundColors.StrumDownKey = (keyStates == Hooks.KeyStates.Down) ? Brushes.LightGreen : Brushes.Transparent;
            } else {
                return false;
            }

            if (keyStates == Hooks.KeyStates.Down) {
                if ((key == GuitarControllerKeys.StrumUpKey && !IsKeyDown(GuitarControllerKeys.StrumUpKey)) ||
                    (key == GuitarControllerKeys.StrumDownKey && !IsKeyDown(GuitarControllerKeys.StrumDownKey))) {
                    if (IsKeyDown(GuitarControllerKeys.GreenKey)) {
                        KeyboardOutputBackgroundColors.Key1 = Brushes.LightGreen;
                        SendKey(KeyboardOutputKeys.Key1, false);
                        Thread.Sleep(5);
                        SendKey(KeyboardOutputKeys.Key1, true);
                        _isKeyDown[KeyboardOutputKeys.Key1] = true;
                    }
                    if (IsKeyDown(GuitarControllerKeys.RedKey)) {
                        KeyboardOutputBackgroundColors.Key2 = Brushes.LightGreen;
                        SendKey(KeyboardOutputKeys.Key2, false);
                        Thread.Sleep(5);
                        SendKey(KeyboardOutputKeys.Key2, true);
                        _isKeyDown[KeyboardOutputKeys.Key2] = true;
                    }
                    if (IsKeyDown(GuitarControllerKeys.YellowKey)) {
                        KeyboardOutputBackgroundColors.Key3 = Brushes.LightGreen;
                        SendKey(KeyboardOutputKeys.Key3, false);
                        Thread.Sleep(5);
                        SendKey(KeyboardOutputKeys.Key3, true);
                        _isKeyDown[KeyboardOutputKeys.Key3] = true;
                    }
                    if (IsKeyDown(GuitarControllerKeys.BlueKey)) {
                        KeyboardOutputBackgroundColors.Key4 = Brushes.LightGreen;
                        SendKey(KeyboardOutputKeys.Key4, false);
                        Thread.Sleep(5);
                        SendKey(KeyboardOutputKeys.Key4, true);
                        _isKeyDown[KeyboardOutputKeys.Key4] = true;
                    }
                    if (IsKeyDown(GuitarControllerKeys.OrangeKey)) {
                        KeyboardOutputBackgroundColors.Key5 = Brushes.LightGreen;
                        SendKey(KeyboardOutputKeys.Key5, false);
                        Thread.Sleep(5);
                        SendKey(KeyboardOutputKeys.Key5, true);
                        _isKeyDown[KeyboardOutputKeys.Key5] = true;
                    }
                }
            } else {
                if (key == GuitarControllerKeys.GreenKey && IsKeyDown(GuitarControllerKeys.GreenKey)) {
                    KeyboardOutputBackgroundColors.Key1 = Brushes.Transparent;
                    SendKey(KeyboardOutputKeys.Key1, false);
                    _isKeyDown[KeyboardOutputKeys.Key1] = false;
                } else if (key == GuitarControllerKeys.RedKey && IsKeyDown(GuitarControllerKeys.RedKey)) {
                    KeyboardOutputBackgroundColors.Key2 = Brushes.Transparent;
                    SendKey(KeyboardOutputKeys.Key2, false);
                    _isKeyDown[KeyboardOutputKeys.Key2] = false;
                } else if (key == GuitarControllerKeys.YellowKey && IsKeyDown(GuitarControllerKeys.YellowKey)) {
                    KeyboardOutputBackgroundColors.Key3 = Brushes.Transparent;
                    SendKey(KeyboardOutputKeys.Key3, false);
                    _isKeyDown[KeyboardOutputKeys.Key3] = false;
                } else if (key == GuitarControllerKeys.BlueKey && IsKeyDown(GuitarControllerKeys.BlueKey)) {
                    KeyboardOutputBackgroundColors.Key4 = Brushes.Transparent;
                    SendKey(KeyboardOutputKeys.Key4, false);
                    _isKeyDown[KeyboardOutputKeys.Key4] = false;
                } else if (key == GuitarControllerKeys.OrangeKey && IsKeyDown(GuitarControllerKeys.OrangeKey)) {
                    KeyboardOutputBackgroundColors.Key5 = Brushes.Transparent;
                    SendKey(KeyboardOutputKeys.Key5, false);
                    _isKeyDown[KeyboardOutputKeys.Key5] = false;
                }
            }

            _isKeyDown[key] = keyStates == Hooks.KeyStates.Down;

            return true;
        }

        public void SendKey(Key key, bool isKeyDown) {
            var vkCode = (byte)KeyInterop.VirtualKeyFromKey(key);
            keybd_event(vkCode, 0, (uint)(isKeyDown ? 0x00 : 0x02), IntPtr.Zero);
        }

        public void LoadKeys() {
            if (File.Exists(_filePath)) {
                var json = File.ReadAllText(_filePath);
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
            var json = JsonConvert.SerializeObject(new {
                GuitarControllerKeys,
                KeyboardOutputKeys
            }, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnClosing(object sender, CancelEventArgs e) {
            if (_keyboardHook.IsHooked) {
                _keyboardHook.Unhook();
            }
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);
    }
}
