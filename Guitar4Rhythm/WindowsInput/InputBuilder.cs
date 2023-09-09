using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Guitar4Rhythm.WindowsInput.Native;
using InputType = Guitar4Rhythm.WindowsInput.Native.InputType;

namespace Guitar4Rhythm.WindowsInput {
    internal class InputBuilder {
        private readonly IList<INPUT> _inputs;

        public InputBuilder() {
            _inputs = new List<INPUT>();
        }

        public INPUT[] ToArray() {
            return _inputs.ToArray();
        }

        public InputBuilder AddKeyDown(Key key, KeyboardInputType inputType) {
            INPUT down = CreateKeyInput(key, inputType, isKeyDown: true);
            _inputs.Add(down);

            return this;
        }

        public InputBuilder AddKeyUp(Key key, KeyboardInputType inputType) {
            INPUT up = CreateKeyInput(key, inputType, isKeyDown: false);
            _inputs.Add(up);

            return this;
        }

        public InputBuilder AddKeyPress(Key key, KeyboardInputType inputType) {
            AddKeyDown(key, inputType);
            AddKeyUp(key, inputType);

            return this;
        }

        private INPUT CreateKeyInput(Key key, KeyboardInputType inputType, bool isKeyDown) {
            INPUT input = new INPUT { Type = (uint)InputType.Keyboard };
            input.Data.Keyboard = new KEYBDINPUT();

            int vkCode = KeyInterop.VirtualKeyFromKey(key);
            if (inputType == KeyboardInputType.VirtualKeyCode) {
                input.Data.Keyboard.Vk = (ushort)vkCode;
                input.Data.Keyboard.Scan = 0;
            } else {
                input.Data.Keyboard.Vk = 0;
                input.Data.Keyboard.Scan = (ushort)NativeMethods.MapVirtualKey((uint)vkCode, 0);
            }

            input.Data.Keyboard.Flags = (uint)(isKeyDown ? 0 : 2);
            input.Data.Keyboard.Time = 0;
            input.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            return input;
        }
    }
}
