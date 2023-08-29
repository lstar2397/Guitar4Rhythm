using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Guitar4Rhythm.Models.Enums;

namespace Guitar4Rhythm.Models {
    public class KeyboardSimulator {
        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT {
            public uint Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/f0e82d6e-4999-4d22-b3d3-32b25f61fb2a
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        private struct MOUSEKEYBDHARDWAREINPUT {
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct HARDWAREINPUT {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/2abc6be8-c593-4686-93d2-89785232dacd
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        public static void SendKey(Key key, bool isKeyDown, KeyInputType inputType = KeyInputType.VirtualKeyCode) {
            INPUT input = new INPUT { Type = 1 };
            input.Data.Keyboard = new KEYBDINPUT();

            var vkCode = KeyInterop.VirtualKeyFromKey(key);
            if (inputType == KeyInputType.VirtualKeyCode) {
                input.Data.Keyboard.Vk = (ushort)vkCode;
                input.Data.Keyboard.Scan = 0;
            } else {
                input.Data.Keyboard.Vk = 0;
                input.Data.Keyboard.Scan = (ushort)MapVirtualKey((uint)vkCode, 0);
            }

            input.Data.Keyboard.Flags = (uint)(isKeyDown ? 0 : 2);
            input.Data.Keyboard.Time = 0;
            input.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            INPUT[] inputs = new INPUT[] { input };
            if (SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT))) == 0) {
                throw new Exception();
            }
        }
    }
}
