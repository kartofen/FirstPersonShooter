using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

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
        //public static SafeFileHandle h = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
        public static IntPtr Consoleh = GetStdHandle(STD_OUTPUT_HANDLE);
        public static CharInfo[] buf;
        public static SmallRect rect;
        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }
        public static ConsoleKeyInfo key { get; set; } = new ConsoleKeyInfo();
        public static float deltaTime { get; set; }
        public static int GameTick { get; set; } = 0;
        public static KeyState[] keys = new KeyState[256]; 
        static short[] keyOldState = new short[256];
        static short[] keyNewState = new short[256];
        static string AppName;

        public GameEngine()
        {
            OnGameStart();
        }

        public unsafe static void CreateConsole(int height, int width,int fontWidth, int fontHeight, string appName)
        {
            ScreenHeight = height;
            ScreenWidth = width;
            buf = new CharInfo[ScreenHeight*ScreenWidth];
            rect = new SmallRect() { Left = 0, Top = 0, Right = (short)ScreenWidth, Bottom = (short)ScreenHeight };
            AppName = appName;
            

            /*CONSOLE_FONT_INFO_EX test = new CONSOLE_FONT_INFO_EX();
            GetCurrentConsoleFontEx(Consoleh, false, ref test);*/

            /*string fontName = "Consolas";

            CONSOLE_FONT_INFO_EX newInfo = new CONSOLE_FONT_INFO_EX();
            newInfo.cbSize = (uint) Marshal.SizeOf(newInfo);
            newInfo.nFont = 0;
            newInfo.dwFontSize = new Coord(3, 5);
            //newInfo.dwFontSize = new Coord(test.dwFontSize.X, test.dwFontSize.Y);
            newInfo.FontFamily = 0x00;
            newInfo.FontWeight = 400;
            IntPtr ptr = new IntPtr(newInfo.FaceName);
            Marshal.Copy(fontName.ToCharArray(), 0, ptr, fontName.Length);
            char[] charFontName = fontName.ToCharArray();
            for (int i = 0; i < charFontName.Length; i++)
            {
                newInfo.FaceName[i] = charFontName[i];
            }

            SetCurrentConsoleFontEx(Consoleh, false, newInfo);*/
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

                for (int i = 0; i < 256; i++)
				{
					keyNewState[i] = GetAsyncKeyState(i);
                    keys[i].Pressed = false;
					keys[i].Released = false;

					if (keyNewState[i] != keyOldState[i])
					{
						if ((keyNewState[i] & 0x8000) != 0)
						{
							keys[i].Pressed = !keys[i].Held;
							keys[i].Held = true;
						}
						else
						{
							keys[i].Released = true;
							keys[i].Held = false;
						}
					}

					keyOldState[i] = keyNewState[i];
                }

                OnKeyPressed();
                OnUserUpdate();
                AddInformationToBuffer();

                bool b = WriteConsoleOutput(Consoleh, buf,
                    new Coord() { X = (short)ScreenWidth, Y = (short)ScreenHeight },
                    new Coord() { X = 0, Y = 0 },
                    ref rect
                );
                SetConsoleTitle(AppName+"  Power by ConsoleGameEngine   FPS: "+1/deltaTime);

                time1 = time2;
            }
        }

        public static void DrawToScreenBuffer(string text, int y, int x, COLOUR color)
        {
            Char[] tempArr = text.ToCharArray(0, text.Length);
            int i = 0;
            
            foreach (char c in tempArr)
            {
                buf[y*ScreenWidth + (x + i)].Char.AsciiChar = (byte)(int)c;
                buf[y*ScreenWidth + (x + i)].Attributes = (short)color;
                i++;
            }
        }

        public static void FillA(int x1, int y1, int x2, int y2, PIXEL pixel, COLOUR colour)
        {
            Clip(x1, y1);
		    Clip(x2, y2);
            for (int x = x1; x < x2; x++)
                for (int y = y1; y < y2; y++)
                    DrawA(x, y, pixel, colour);
        }

        public static void FillB(int x1, int y1, int x2, int y2, byte pixel, short colour)
        {
            Clip(x1, y1);
		    Clip(x2, y2);
            for (int x = x1; x < x2; x++)
                for (int y = y1; y < y2; y++)
                    DrawB(x, y, pixel, colour);
            
        }

        public static void DrawA(int x, int y, PIXEL pixel, COLOUR colour)
        {
            buf[y * ScreenWidth + x].Char.AsciiChar = (byte)pixel;
			buf[y * ScreenWidth + x].Attributes = (short)colour;
        }

        public static void DrawB(int x, int y, byte pixel, short colour)
        {
            buf[y * ScreenWidth + x].Char.AsciiChar = pixel;
			buf[y * ScreenWidth + x].Attributes = colour;
        }

        public static void SetPixel(int x, int y, PIXEL pixel)
        {
            buf[y * ScreenWidth + x].Char.AsciiChar = (byte)pixel;
        }

        public static void SetAttribute(int x, int y, COLOUR colour)
        {
			buf[y * ScreenWidth + x].Attributes = (short)colour;
        }

        static void Clip(int x, int y)
        {
            if (x < 0) x = 0;
            if (x >= ScreenWidth) x = ScreenWidth;
            if (y < 0) y = 0;
            if (y >= ScreenHeight) y = ScreenHeight;
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
                    buf[i].Attributes = (short)COLOUR.FG_GREY;
                }
            }
            return;
        }

        
        public enum COLOUR
        {
            FG_BLACK		= 0x0000,
            FG_DARK_BLUE    = 0x0001,	
            FG_DARK_GREEN   = 0x0002,
            FG_DARK_CYAN    = 0x0003,
            FG_DARK_RED     = 0x0004,
            FG_DARK_MAGENTA = 0x0005,
            FG_DARK_YELLOW  = 0x0006,
            FG_GREY			= 0x0007, // Thanks MS :-/
            FG_DARK_GREY    = 0x0008,
            FG_BLUE			= 0x0009,
            FG_GREEN		= 0x000A,
            FG_CYAN			= 0x000B,
            FG_RED			= 0x000C,
            FG_MAGENTA		= 0x000D,
            FG_YELLOW		= 0x000E,
            FG_WHITE		= 0x000F,
            BG_BLACK		= 0x0000,
            BG_DARK_BLUE	= 0x0010,
            BG_DARK_GREEN	= 0x0020,
            BG_DARK_CYAN	= 0x0030,
            BG_DARK_RED		= 0x0040,
            BG_DARK_MAGENTA = 0x0050,
            BG_DARK_YELLOW	= 0x0060,
            BG_GREY			= 0x0070,
            BG_DARK_GREY	= 0x0080,
            BG_BLUE			= 0x0090,
            BG_GREEN		= 0x00A0,
            BG_CYAN			= 0x00B0,
            BG_RED			= 0x00C0,
            BG_MAGENTA		= 0x00D0,
            BG_YELLOW		= 0x00E0,
            BG_WHITE		= 0x00F0,
        };

        public enum PIXEL
        {
            PIXEL_SOLID = 219,
            PIXEL_THREEQUARTERS = 178,
            PIXEL_HALF = 177,
            PIXEL_QUARTER = 176,
            PIXEL_BLANK = 32,
        };
        public const uint STD_INPUT_HANDLE = unchecked((uint)-10),
            STD_OUTPUT_HANDLE = unchecked((uint)-11),
            STD_ERROR_HANDLE = unchecked((uint)-12);

        private const int TMPF_TRUETYPE = 4;
        private const int LF_FACESIZE = 32;
        private static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);


        [StructLayout(LayoutKind.Sequential)]
        internal unsafe struct CONSOLE_FONT_INFO_EX 
        {
            internal uint cbSize;
            internal uint nFont;
            internal Coord dwFontSize;
            internal int FontFamily;
            internal int FontWeight;
            internal fixed char FaceName[LF_FACESIZE];
        }  


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool GetCurrentConsoleFontEx(
            IntPtr consoleOutput, 
            bool maximumWindow,
            ref CONSOLE_FONT_INFO_EX lpConsoleCurrentFontEx);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool SetCurrentConsoleFontEx(
            IntPtr consoleOutput, 
            bool maximumWindow,
            CONSOLE_FONT_INFO_EX consoleCurrentFontEx);

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleTitle(string lpConsoleTitle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(uint nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            IntPtr hConsoleOutput, 
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
        
        public struct KeyState
        {
            public bool Pressed;
            public bool Released;
            public bool Held;
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