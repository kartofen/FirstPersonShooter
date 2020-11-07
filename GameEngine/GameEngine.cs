using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace ConsoleGameEngine
{
    public class GameEngine
    {
    /*  //todo: 
            add mouse:
                https://github.com/OneLoneCoder/videos/blob/master/olcConsoleGameEngine.h#L419
                https://github.com/OneLoneCoder/videos/blob/master/olcConsoleGameEngine.h#L875
                https://stackoverflow.com/a/29971246
                important: lpdword > int*
        */

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

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

        public struct CoordF
        {
            public float X;
            public float Y;

            public CoordF(float X, float Y)
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
        public static int GameTick { get; set; } = 0;

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
            Thread thread = new Thread(GameThread);
            thread.Start();
        }

        public void GameThread()
        {    
            DateTime time1 = DateTime.Now;
            DateTime time2 = DateTime.Now;

            OnUserCreate();
        
            while(true)
            {
                GameTick++;

                time2 = DateTime.Now;
                deltaTime = (time2.Ticks - time1.Ticks) / 10000000f;

                OnKeyPressed();
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
        protected virtual void OnKeyPressed() { return; }
        protected virtual void OnUserCreate() { return; }
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

        public struct Sprite
        {
            public char[,] sprite_chars;
            public short[,] sprite_attributes;
            public byte[,] sprite_bytes;
            public int height;
            public int width;
            public Sprite(char[,] sprite_chars, short[,] attributes, int height, int width)
            {
                this.height = height;
                this.width = width;           
                this.sprite_chars = sprite_chars;
                this.sprite_attributes= attributes;
                this.sprite_bytes = new byte[height, width];
                this.sprite_bytes = CharSpriteToByteSprite(sprite_chars);
            }

            public byte[,] CharSpriteToByteSprite(char[,] sprite_chars_)
            {
                byte[,] bytes = new byte[height, width];
                for(int i = 0; i < height; i++)
                    for(int j = 0; j < width; j++)
                    {
                        if(sprite_chars_[i, j] == '█')
                            bytes[i, j] = 219;
                        else if(sprite_chars_[i, j] == '▓')
                            bytes[i, j] = 178;
                        else if(sprite_chars_[i, j] == '▒')
                            bytes[i, j] = 177;
                        else if(sprite_chars_[i, j] == '░')
                            bytes[i, j] = 176;
                        else if(sprite_chars_[i, j] == ' ')
                            bytes[i, j] = 32;
                        else
                            bytes[i, j] = (byte)(int)sprite_chars[i, j];
                    }
                return bytes;
            }

            //todo fix
            public void Animate(List<Sprite> animation, int interval_animation, int interval_frames)
            {
                int old_GameTick = 0;
                int i = 0;
                if(GameTick - old_GameTick == interval_animation)
                {
                    if(i == animation.Count)
                    {
                        i = 0;
                    }

                    sprite_chars = animation[i].sprite_chars;
                    sprite_attributes = animation[i].sprite_attributes;
                    sprite_bytes = CharSpriteToByteSprite(sprite_chars);
                    i++;

                    old_GameTick = GameTick;
                }
            }

            public char SampleGlyphCharSprite(float x, float y)
            {
                int sx = (int)(x * (float)this.width);
                int sy = (int)(y * (float)this.height - 1.0f);
                if (sx < 0 || sx >= this.width || sy < 0 || sy >= this.height)
                    return ' ';
                else
                    return this.sprite_chars[sy, sx];
            }
            public byte SampleGlyphByteSprite(float x, float y)
            {
                int sx = (int)(x * (float)this.width);
                int sy = (int)(y * (float)this.height - 1.0f);
                if (sx < 0 || sx >= this.width || sy < 0 || sy >= this.height)
                    return 32;
                else
                    return this.sprite_bytes[sy, sx];
            }
            public short SampleGlyphAttribute(float x, float y)
            {
                int sx = (int)(x * (float)this.width);
                int sy = (int)(y * (float)this.height - 1.0f);
                if (sx < 0 || sx >=this. width || sy < 0 || sy >= this.height)
                    return 0;
                else
                    return this.sprite_attributes[sy, sx];
            }
        }
    }
}