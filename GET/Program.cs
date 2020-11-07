using System;
using System.Collections.Generic;
using ConsoleGameEngine;

namespace GET
{
    class Game : GameEngine
    {
        public Game() { }

        protected override void OnUserUpdate() 
        {   
            Start();
            return;
        }

        protected override void OnUserCreate()
        {
            ObjList.Add(Obj1);
            ObjList.Add(Obj2);
        }

        protected override void OnKeyPressed()//ConsoleKeyInfo key)
        {
            short W = GetAsyncKeyState('W'); 
            short A = GetAsyncKeyState('A'); 
            short S = GetAsyncKeyState('S'); 
            short D = GetAsyncKeyState('D'); 
            short Q = GetAsyncKeyState('Q'); 
            short E = GetAsyncKeyState('E'); 
            short F = GetAsyncKeyState('F'); 
            if(W != 0)
            {
                PlayerX += MathF.Sin(PlayerA) * Speed * deltaTime;
                PlayerY += MathF.Cos(PlayerA) * Speed * deltaTime;
                if(map[(int)PlayerX, (int)PlayerY] != '.')
                {
                    PlayerX -= MathF.Sin(PlayerA) * Speed * deltaTime;
                    PlayerY -= MathF.Cos(PlayerA) * Speed * deltaTime;
                }
            }
            if(A != 0)
            {
                PlayerA -= (1f) * deltaTime;
            }
            if(S != 0)
            {
                PlayerX -= MathF.Sin(PlayerA) * Speed * deltaTime;
                PlayerY -= MathF.Cos(PlayerA) * Speed * deltaTime;
                if(map[(int)PlayerX, (int)PlayerY] != '.')
                {
                    PlayerX += MathF.Sin(PlayerA) * Speed * deltaTime;
                    PlayerY += MathF.Cos(PlayerA) * Speed * deltaTime;
                }
            }
            if(D != 0)
            {
                PlayerA += (1f) * deltaTime;
            }
            if(Q != 0)
            {
                PlayerX -= MathF.Cos(PlayerA) * Speed * deltaTime;
                PlayerY += MathF.Sin(PlayerA) * Speed * deltaTime;
                if(map[(int)PlayerX, (int)PlayerY] != '.')
                {
                    PlayerX += MathF.Cos(PlayerA) * Speed * deltaTime;
                    PlayerY -= MathF.Sin(PlayerA) * Speed * deltaTime;
                    
                }
            }
            if(E != 0)
            {
                PlayerX += MathF.Cos(PlayerA) * Speed * deltaTime;
                PlayerY -= MathF.Sin(PlayerA) * Speed * deltaTime;
                if(map[(int)PlayerX, (int)PlayerY] != '.')
                {
                    PlayerX -= MathF.Cos(PlayerA) * Speed * deltaTime;
                    PlayerY += MathF.Sin(PlayerA) * Speed * deltaTime;
                }
            }
            if(F != 0)
            {
                if(map[(int)PlayerX + 1, (int)PlayerY] == 'D') map[(int)PlayerX + 1, (int)PlayerY] = '.';
                else if(map[(int)PlayerX - 1, (int)PlayerY] == 'D') map[(int)PlayerX - 1, (int)PlayerY] = '.';
                else if(map[(int)PlayerX, (int)PlayerY + 1] == 'D') map[(int)PlayerX , (int)PlayerY + 1] = '.';
                else if(map[(int)PlayerX, (int)PlayerY - 1] == 'D') map[(int)PlayerX, (int)PlayerY - 1] = '.';
            }
            return;
        }

        protected override void AddInformationToBuffer()
        {
            WriteObjects();

            for (int nx = 0; nx < MapHeight; nx++)
                for (int ny = 0; ny < MapWidth; ny++)
                {
                    buf[(ny) * ScreenWidth + nx].Char.AsciiChar = (byte)(int)map[nx, ny];
                    buf[(ny) * ScreenWidth + nx].Attributes = 0;
                }

            buf[((int)PlayerY) * ScreenWidth + (int)PlayerX].Char.AsciiChar = (byte)(int)'P';

            DrawToScreenBuffer("FPS: " + 1f / deltaTime, 0, 33, buf);
            DrawToScreenBuffer("Angle: " + PlayerA, 1, 33, buf);
            DrawToScreenBuffer("GameTick: " + GameTick, 2, 33, buf);

            for (int i = 0; i < buf.Length; i++)
            {
                if (buf[i].Attributes == 0)
                {
                    buf[i].Attributes = 7;
                }
            }
            return;
        }



        public struct Object
        {
            public CoordF coords;
            public CoordF velocity;
            public Sprite sprite;
            public Object(CoordF coords, CoordF velocity, Sprite sprite)
            {
                this.coords = coords;
                this.sprite = sprite;
                this.velocity = velocity;
            }
        }

        static Sprite LampSprite = new Sprite(
            new char[,] {
                {' ', ' ', ' ', ' ', ' ',' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', '█', '█','█', '█', ' ', ' ', ' '},
                {' ', ' ', '█', '█', '▓','▓', '█', '█', ' ', ' '},
                {' ', ' ', '█', '▓', '▓','▓', '▓', '█', ' ', ' '},
                {' ', ' ', '█', '▓', '▓','▓', '▓', '█', ' ', ' '},
                {' ', ' ', '█', '█', '▓','▓', '█', '█', ' ', ' '},
                {' ', ' ', ' ', '█', '▓','▓', '█', ' ', ' ', ' '},
                {' ', ' ', ' ', '█', '█','█', '█', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '█','█', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '█','█', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '█','█', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '█','█', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '█','█', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '█','█', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', '█','█', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', '█', '█','█', '█', ' ', ' ', ' '},
                {' ', ' ', ' ', '█', '█','█', '█', ' ', ' ', ' '},
                {' ', ' ', '█', '█', '█','█', '█', '█', ' ', ' '},
                {' ', ' ', '█', '█', '█','█', '█', '█', ' ', ' '},
                {' ', '█', '█', '█', '█','█', '█', '█', '█', ' '},
            }, 
            new short[,] {
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 4, 4, 9, 9, 9, 9 },
                { 9, 9, 9, 4, 4, 4, 4, 9, 9, 9 },
                { 9, 9, 9, 4, 4, 4, 4, 9, 9, 9 },
                { 9, 9, 9, 9, 4, 4, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 4, 4, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 },                                
            },
            20, 10
        );

        static Sprite Obj3A1 = new Sprite(
            new char[,] {
                {' ', ' ', '█', ' ', ' '},
                {' ', ' ', '█', ' ', ' '},
                {'█', '█', '█', '█', '█'},
                {' ', ' ', '█', ' ', ' '},
                {' ', ' ', '█', ' ', ' '},
            },
            new short[,] { 
                {3, 3, 3, 3, 3}, 
                {3, 3, 3, 3, 3}, 
                {3, 3, 3, 3, 3}, 
                {3, 3, 3, 3, 3}, 
                {3, 3, 3, 3, 3} 
            },
            5, 5
        );
        static Sprite Obj3A2 = new Sprite(
            new char[,] {
                {' ', ' ', '█', ' ', ' '},
                {' ', ' ', ' ', ' ', ' '},
                {'█', '█', ' ', '█', '█'},
                {' ', ' ', ' ', ' ', ' '},
                {' ', ' ', '█', ' ', ' '},
            },
            new short[,] { 
                {3, 3, 3, 3, 3}, 
                {3, 3, 3, 3, 3}, 
                {3, 3, 3, 3, 3}, 
                {3, 3, 3, 3, 3}, 
                {3, 3, 3, 3, 3} 
            },
            5, 5
        );
        
        static Object Obj1 = new Object(new CoordF(8.5f, 10.5f), new CoordF(0, 0), LampSprite);
        static Object Obj2 = new Object(new CoordF(25.5f, 8.5f), new CoordF(0, 0), LampSprite);
        //static Object Animated_Obj3 = new Object(new CoordF(9.5f, 14.5f), new CoordF(0, 0), Obj3A1);
        static List<Object> ObjList = new List<Object>();
        static List<Object> Enemies = new List<Object>();
        static List<Sprite> AnimationObj3 = new List<Sprite>();

        static int MapWidth = 16;			
        static int MapHeight = 32;
        static float PlayerX = 8.0f;		
        static float PlayerY = 8.0f;
        static float PlayerA = 0.0f;		
        static float FOV = MathF.PI / 4.0f;
        static float Depth = 16.0f;
        static float Speed = 10.0f;
        static float[] DepthBuffer = new float[ScreenWidth];	
        static char[,] map = {
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
            {'#', '.', '.', '#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '#', '.', '.', '.', '.', '.', '.', '.', '.', '#', '.', '.', '#'},
            {'#', '.', '#', '#', '.', '.', '.', '.', '.', '.', '.', '.', '#', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', 'D', '#', '#', '#', '#', '#', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#', '#', '#', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#', '#', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#', '#', '#', '#'},
            {'#', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '.', '#'},
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'}
        };

        static void Start()
        {
            for (int x = 0; x < ScreenWidth; x++)
            {
                float RayAngle = (PlayerA - FOV / 2.0f) + ((float)x / (float)ScreenWidth) * FOV;

                float StepSize = 0.1f;
                float DistanceToWall = 0.0f;

                bool Boundary = false;
                bool HitWall = false;
                bool Door = false;

                float EyeX = MathF.Sin(RayAngle);
                float EyeY = MathF.Cos(RayAngle);

                RayCasting(StepSize, ref DistanceToWall, ref Boundary, ref HitWall, ref Door, EyeX, EyeY);

                Shading(buf, x, DistanceToWall, Boundary, Door);
            }
        }

        static void WriteObjects()
        {       
            foreach (Object obj in ObjList)
            {
                float VecX = obj.coords.X - PlayerX;
                float VecY = obj.coords.Y - PlayerY;
                float DistanceFromPlayer = MathF.Sqrt(VecX * VecX + VecY * VecY);

                float EyeX = MathF.Sin(PlayerA);
                float EyeY = MathF.Cos(PlayerA);

                float ObjectAngle = MathF.Atan2(EyeY, EyeX) - MathF.Atan2(VecY, VecX);

                if (ObjectAngle < -3.14159f)
                    ObjectAngle += 2.0f * 3.14159f;
                if (ObjectAngle > 3.14159f)
                    ObjectAngle -= 2.0f * 3.14159f;

                bool InPlayerFOV = MathF.Abs(ObjectAngle) < FOV / 2.0f;

                if (InPlayerFOV && DistanceFromPlayer >= 0.5f && DistanceFromPlayer < Depth)
                {
                    float ObjectCeiling = (float)(ScreenHeight / 2.0) - ScreenHeight / ((float)DistanceFromPlayer);
                    float ObjectFloor = ScreenHeight - ObjectCeiling;
                    float ObjectHeight = ObjectFloor - ObjectCeiling;
                    float ObjectAspectRatio = (float)obj.sprite.height / (float)obj.sprite.width;
                    float ObjectWidth = ObjectHeight / ObjectAspectRatio;
                    float MiddleOfObject = (0.5f * (ObjectAngle / (FOV / 2.0f)) + 0.5f) * (float)ScreenWidth;

                    for (float lx = 0; lx < ObjectWidth; lx++)
                        for (float ly = 0; ly < ObjectHeight; ly++)
                        {
                            float SampleX = lx / ObjectWidth;
                            float SampleY = ly / ObjectHeight;
                            char s_char = obj.sprite.SampleGlyphCharSprite(SampleX, SampleY);
                            byte s_byte = obj.sprite.SampleGlyphByteSprite(SampleX, SampleY);
                            short s_attribute = obj.sprite.SampleGlyphAttribute(SampleX, SampleY);
                            int ObjectColumn = (int)(MiddleOfObject + lx - (ObjectWidth / 2.0f));
                            if (ObjectColumn >= 0 && ObjectColumn < ScreenWidth)
                                if (s_char != ' ' && DepthBuffer[ObjectColumn] >= DistanceFromPlayer || s_byte != 32 && DepthBuffer[ObjectColumn] >= DistanceFromPlayer)
                                {
                                    //buf[(int)(ObjectCeiling + ly) * ScreenWidth + (int)ObjectColumn].Char.AsciiChar = (byte)(int)s_char;
                                    buf[(int)(ObjectCeiling + ly) * ScreenWidth + (int)ObjectColumn].Char.AsciiChar = s_byte;
                                    buf[(int)(ObjectCeiling + ly) * ScreenWidth + (int)ObjectColumn].Attributes = s_attribute;
                                    DepthBuffer[ObjectColumn] = DistanceFromPlayer;
                                }
                        }
                }
            }
        }

        static void Shading(CharInfo[] buf, int x, float DistanceToWall, bool Boundary, bool Door)
        {
            int Ceiling = (int)((float)(ScreenHeight / 2.0) - ScreenHeight / ((float)DistanceToWall));
            int Floor = ScreenHeight - Ceiling;

            DepthBuffer[x] = DistanceToWall;

            byte Shade;
            for (int y = 0; y < ScreenHeight; y++)
            {

                if (y < Ceiling)
                {
                    buf[y * ScreenWidth + x].Char.AsciiChar = 32;
                }
                else if (y > Ceiling && y <= Floor)
                {

                    if (DistanceToWall <= Depth / 4.0f) Shade = 219;
                    else if (DistanceToWall < Depth / 3.0f) Shade = 178;
                    else if (DistanceToWall < Depth / 2.0f) Shade = 177;
                    else if (DistanceToWall < Depth) Shade = 176;
                    else Shade = 32;

                    if (Boundary == true) Shade = 32;

                    buf[y * ScreenWidth + x].Attributes = 1;
                    if (Door == true)
                        buf[y * ScreenWidth + x].Attributes = 2;
                    buf[y * ScreenWidth + x].Char.AsciiChar = Shade;
                }

                else
                {
                    float shading = 1.0f - (((float)y - ScreenHeight / 2.0f) / ((float)ScreenHeight / 2.0f));
                    if (shading < 0.25) Shade = 219;
                    else if (shading < 0.5) Shade = 178;
                    else if (shading < 0.75) Shade = 177;
                    else if (shading < 0.9) Shade = 176;
                    else Shade = 32;
                    buf[y * ScreenWidth + x].Char.AsciiChar = Shade;
                    buf[y * ScreenWidth + x].Attributes = 8;
                }
            }
        }

        static void RayCasting(float StepSize, ref float DistanceToWall, ref bool Boundary, ref bool HitWall, ref bool Door, float EyeX, float EyeY)
        {
            while (!HitWall && DistanceToWall < Depth)
            {
                DistanceToWall += StepSize;
                int TestX = (int)(PlayerX + EyeX * DistanceToWall);
                int TestY = (int)(PlayerY + EyeY * DistanceToWall);

                if (TestX < 0 || TestY >= MapWidth || TestY < 0 || TestX >= MapHeight)
                {
                    HitWall = true;
                    DistanceToWall = Depth;
                }
                else
                {
                    if (map[TestX, TestY] == '#' || map[TestX, TestY] == 'D' || map[TestX, TestY] == 'I')
                    {
                        if (map[TestX, TestY] == 'D')
                        {
                            Door = true;
                        }

                        HitWall = true;

                        float[,] f = new float[2, 4];

                        for (int tx = 0; tx < 2; tx++)
                            for (int ty = 0; ty < 2; ty++)
                            {
                                float vy = (float)TestY + ty - PlayerY;
                                float vx = (float)TestX + tx - PlayerX;
                                float d = MathF.Sqrt(vx * vx + vy * vy);
                                float dot = (EyeX * vx / d) + (EyeY * vy / d);
                                if (tx == 0)
                                {
                                    f[0, ty] = d;
                                    f[1, ty] = dot;
                                }
                                else
                                {
                                    f[0, ty + 2] = d;
                                    f[1, ty + 2] = dot;
                                }
                            }

                        float[] sorted = SortFVector(f);

                        float Bound = 0.01f;
                        if (MathF.Acos(sorted[0]) < Bound) Boundary = true;
                        if (MathF.Acos(sorted[1]) < Bound) Boundary = true;
                    }
                }
            }
        }

        static float[] SortFVector(float[,] f)
        {
            //contains ints representing the order from smallest to farthest
            float[] sorted = new float[4];
            for(int i = 0; i < 4; i++)
            {
                sorted[i] = f[0, i];
            }
            Array.Sort(sorted);
            for (int j = 0; j < 4; j++)
            {
                if(sorted[j] == f[0, 0])
                    sorted[j] = f[1, 0];
                if(sorted[j] == f[0, 1])
                    sorted[j] = f[1, 1];
                if(sorted[j] == f[0, 2])
                    sorted[j] = f[1, 2];
                if(sorted[j] == f[0, 3])
                    sorted[j] = f[1, 3];
                    
            }
            return sorted;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            GameEngine.CreateConsole(70, 220);
            Console.CursorVisible = false;
            Console.Clear();
            new Game();
        }
    }
}
