using System.Diagnostics;
using System.Runtime.InteropServices;

class Program
{
    const double INTERVAL = 1.017;
    const int VK_SPACE = 0x20;
    const int VK_ESCAPE = 0x1B;

    static void Main()
    {
        Console.WriteLine("Press SPACE to start, ESC to stop\n");

        while ((GetAsyncKeyState(VK_SPACE) & 0x8000) == 0)
            Thread.Sleep(10);

        Console.WriteLine(">>> STARTED <<<\n");

        Stopwatch sw = Stopwatch.StartNew();
        double next = INTERVAL;

        while (true)
        {
            if ((GetAsyncKeyState(VK_ESCAPE) & 0x8000) != 0)
                break;

            if (sw.Elapsed.TotalSeconds >= next)
            {
                INPUT[] inp = new INPUT[2];
                inp[0].type = inp[1].type = 1;
                inp[0].U.ki.wVk = inp[1].U.ki.wVk = VK_SPACE;
                inp[1].U.ki.dwFlags = 2;
                SendInput(2, inp, Marshal.SizeOf(typeof(INPUT)));
                Console.Write("! ");
                next += INTERVAL;
            }
            Thread.Sleep(1);
        }
    }

    [DllImport("user32.dll")]
    static extern short GetAsyncKeyState(int vKey);

    [DllImport("user32.dll")]
    static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    struct INPUT
    {
        public int type;
        public InputUnion U;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct InputUnion
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] public HARDWAREINPUT hi;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct MOUSEINPUT
    {
        public int dx, dy;
        public uint mouseData, dwFlags, time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL, wParamH;
    }
}