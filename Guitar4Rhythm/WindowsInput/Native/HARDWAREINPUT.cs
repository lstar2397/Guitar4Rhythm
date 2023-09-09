using System.Runtime.InteropServices;

namespace Guitar4Rhythm.WindowsInput.Native {
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct HARDWAREINPUT {
        public uint Msg;
        public ushort ParamL;
        public ushort ParamH;
    }
}
