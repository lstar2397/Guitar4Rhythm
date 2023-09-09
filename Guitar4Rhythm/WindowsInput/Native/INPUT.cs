using System.Runtime.InteropServices;

namespace Guitar4Rhythm.WindowsInput.Native {
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct INPUT {
        public uint Type;
        public MOUSEKEYBDHARDWAREINPUT Data;
    }
}
