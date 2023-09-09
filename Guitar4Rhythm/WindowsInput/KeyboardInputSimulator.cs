using System.Runtime.InteropServices;
using System.Windows.Input;
using Guitar4Rhythm.WindowsInput.Native;

namespace Guitar4Rhythm.WindowsInput {
    internal class KeyboardInputSimulator {

        public void KeyDown(KeyboardInputType inputType, params Key[] keys) {
            InputBuilder builder = new InputBuilder();
            foreach (Key key in keys) {
                builder.AddKeyDown(key, inputType);
            }

            SendInput(builder.ToArray());
        }

        public void KeyUp(KeyboardInputType inputType, params Key[] keys) {
            InputBuilder builder = new InputBuilder();
            foreach (Key key in keys) {
                builder.AddKeyUp(key, inputType);
            }

            SendInput(builder.ToArray());
        }

        public void KeyPress(KeyboardInputType inputType, params Key[] keys) {
            InputBuilder builder = new InputBuilder();
            foreach (Key key in keys) {
                builder.AddKeyPress(key, inputType);
            }

            SendInput(builder.ToArray());
        }

        private void SendInput(INPUT[] inputs) {
            NativeMethods.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
        }
    }
}
