using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Guitar4Rhythm.Hooks {
    public class KeyboardHook {
        #region [ Windows API Code ]
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion

        #region [ Windows API Constants ]
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;
        #endregion

        #region [ Keyboard Hook Structs ]
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelKeyboardProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        #endregion

        #region [ Keyboard Hook Events ]
        public delegate bool KeyBlockingDelegate(Key key, KeyStates keyStates);
        public event KeyBlockingDelegate? KeyBlocking;
        #endregion

        public bool IsHooked => _hookID != IntPtr.Zero;

        public KeyboardHook() => _proc = HookCallback;

        public void Hook() {
            if (!IsHooked) {
                _hookID = SetHook(_proc);
            }
        }

        public void Unhook() {
            if (IsHooked) {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }
        }

        private IntPtr SetHook(LowLevelKeyboardProc proc) {
            using (Process process = Process.GetCurrentProcess())
            using (ProcessModule? module = process.MainModule) {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(module.ModuleName), 0);
            }
        }

        private bool IsKeyDown(IntPtr wParam) => wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN;

        private bool IsKeyUp(IntPtr wParam) => wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP;

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0) {
                int vkCode = Marshal.ReadInt32(lParam);
                Key key = KeyInterop.KeyFromVirtualKey(vkCode);

                if (IsKeyDown(wParam)) {
                    if (KeyBlocking?.Invoke(key, KeyStates.Down) ?? false) {
                        return (IntPtr)1;
                    }
                } else if (IsKeyUp(wParam)) {
                    if (KeyBlocking?.Invoke(key, KeyStates.Up) ?? false) {
                        return (IntPtr)1;
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }
    }

    public enum KeyStates {
        Up = 0,
        Down = 1
    }
}
