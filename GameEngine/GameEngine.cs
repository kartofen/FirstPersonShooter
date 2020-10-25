using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace ConsoleGameEngine
{
    public class GameEngine
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            SafeFileHandle hConsoleOutput, 
            CharInfo[] lpBuffer, 
            Coord dwBufferSize, 
            Coord dwBufferCoord, 
            ref SmallRect lpWriteRegion);

        [StructLayout(LayoutKind.Sequential)]
        public struct Coord
        {
            public short X;
            public short Y;

            public Coord(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Explicit)]
        public struct CharUnion
        {
            [FieldOffset(0)] public char UnicodeChar;
            [FieldOffset(0)] public byte AsciiChar;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct CharInfo
        {
            [FieldOffset(0)] public CharUnion Char;
            [FieldOffset(2)] public short Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SmallRect
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        public static SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        public static CharInfo[] buf;
        public static SmallRect rect;
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
        public static ConsoleKeyInfo key { get; set; } = new ConsoleKeyInfo();
        public static float deltaTime { get; set; }

        public GameEngine()
        {
            OnGameStart();
        }

        public static void CreateConsole(int height, int width)
        {
            if(!h.IsInvalid)
            {
                ScreenHeight = height;
                ScreenWidth = width;
                buf = new CharInfo[ScreenHeight*ScreenWidth];
                rect = new SmallRect() { Left = 0, Top = 0, Right = (short)ScreenWidth, Bottom = (short)ScreenHeight };
            }
        }

        public void OnGameStart()
        {    
            DateTime time1 = DateTime.Now;
            DateTime time2 = DateTime.Now;

            while(true)
            {
                time2 = DateTime.Now;
                deltaTime = (time2.Ticks - time1.Ticks) / 10000000f;
                if(Console.KeyAvailable == true)
                {
                    key = Console.ReadKey();
                    OnKeyPressed(key);
                }
                
                OnAIUpdate();
                OnUserUpdate();
                AddInformationToBuffer();

                bool b = WriteConsoleOutput(h, buf,
                    new Coord() { X = (short)ScreenWidth, Y = (short)ScreenHeight },
                    new Coord() { X = 0, Y = 0 },
                    ref rect
                );

                time1 = time2;
            }
        }

        public static void DrawToScreenBuffer(string text, int x, int y, CharInfo[] buf)
        {
            Char[] tempArr = text.ToCharArray(0, text.Length);
            int i = 0;
            
            foreach (char c in tempArr)
            {
                buf[x*ScreenWidth + (y + i)].Char.AsciiChar = (byte)(int)c;
                buf[x*ScreenWidth + (y + i)].Attributes = 4;
                i++;
            }
        }
        protected virtual void OnUserUpdate() { return; }
        protected virtual void OnKeyPressed(ConsoleKeyInfo key) { return; }
        protected virtual void OnAIUpdate() { return; }
        protected virtual void AddInformationToBuffer() 
        {
            for (int i = 0; i < buf.Length; i++)
            {
                if (buf[i].Attributes == 0)
                {
                    buf[i].Attributes = 7;
                }
            }
            return;
        }

        public struct AI
        {
            public Coord coords;
            public AI(int x, int y)
            {
                coords = new Coord((short)x, (short)y);
            }
        }
    }
}