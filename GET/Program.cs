using System;
using System.Threading;
using ConsoleGameEngine;

namespace GET
{
    class Game : GameEngine
    {
        public Game() { }

        protected override void OnUserUpdate() 
        {
            
            /*if(__time == 130)
            {
                map[(int)ai.coords.X, (int)ai.coords.Y] = '.';
                ai.coords.Y = (short)((int)ai.coords.Y + 1);
                __time = 0;
            }
            else __time = __time + 1;*/
            
            Start();
            return;
        }

        protected override void OnAIUpdate()
        {
            //map[(int)ai.coords.X, (int)ai.coords.Y] = 'I';
        }

        protected override void OnKeyPressed(ConsoleKeyInfo key)
        {
            switch(key.Key)
            {
                case ConsoleKey.W:
                    PlayerX += MathF.Sin(PlayerA) * Speed * deltaTime;
                    PlayerY += MathF.Cos(PlayerA) * Speed * deltaTime;
                    if(map[(int)PlayerX, (int)PlayerY] != '.')
                    {
                        PlayerX -= MathF.Sin(PlayerA) * Speed * deltaTime;
                        PlayerY -= MathF.Cos(PlayerA) * Speed * deltaTime;
                    }
                break;
                case ConsoleKey.A:
                    PlayerA -= (1f) * deltaTime;
                break;
                case ConsoleKey.S:
                    PlayerX -= MathF.Sin(PlayerA) * Speed * deltaTime;
                    PlayerY -= MathF.Cos(PlayerA) * Speed * deltaTime;
                    if(map[(int)PlayerX, (int)PlayerY] != '.')
                    {
                        PlayerX += MathF.Sin(PlayerA) * Speed * deltaTime;
                        PlayerY += MathF.Cos(PlayerA) * Speed * deltaTime;
                    }
                break;
                case ConsoleKey.D:
                    PlayerA += (1f) * deltaTime;
                break;
                case ConsoleKey.Q:
                    PlayerX -= MathF.Cos(PlayerA) * Speed * deltaTime;
                    PlayerY += MathF.Sin(PlayerA) * Speed * deltaTime;
                    if(map[(int)PlayerX, (int)PlayerY] != '.')
                    {
                        PlayerX += MathF.Cos(PlayerA) * Speed * deltaTime;
                        PlayerY -= MathF.Sin(PlayerA) * Speed * deltaTime;
                        
                    }
                break;
                case ConsoleKey.E:
                    PlayerX += MathF.Cos(PlayerA) * Speed * deltaTime;
                    PlayerY -= MathF.Sin(PlayerA) * Speed * deltaTime;
                    if(map[(int)PlayerX, (int)PlayerY] != '.')
                    {
                        PlayerX -= MathF.Cos(PlayerA) * Speed * deltaTime;
                        PlayerY += MathF.Sin(PlayerA) * Speed * deltaTime;
                    }
                break;
                case ConsoleKey.F: 
                    if(map[(int)PlayerX + 1, (int)PlayerY] == 'D') map[(int)PlayerX + 1, (int)PlayerY] = '.';
                    else if(map[(int)PlayerX - 1, (int)PlayerY] == 'D') map[(int)PlayerX - 1, (int)PlayerY] = '.';
                    else if(map[(int)PlayerX, (int)PlayerY + 1] == 'D') map[(int)PlayerX , (int)PlayerY + 1] = '.';
                    else if(map[(int)PlayerX, (int)PlayerY - 1] == 'D') map[(int)PlayerX, (int)PlayerY - 1] = '.';
                break;
            }
            return;
        }

        protected override void AddInformationToBuffer()
        {
            for (int nx = 0; nx < MapHeight; nx++)
                for (int ny = 0; ny < MapWidth; ny++)
                {
                    buf[(ny) * ScreenWidth + nx].Char.AsciiChar = (byte)(int)map[nx, ny];  
                    buf[(ny) * ScreenWidth + nx].Attributes = 0;
                }

            buf[((int)PlayerY) * ScreenWidth + (int)PlayerX].Char.AsciiChar = (byte)(int)'P';

            DrawToScreenBuffer("FPS: " + 1f / deltaTime, 0, 33, buf);
            DrawToScreenBuffer("Angle: " + PlayerA, 1, 33, buf);

            for (int i = 0; i < buf.Length; i++)
            {
                if (buf[i].Attributes == 0)
                {
                    buf[i].Attributes = 7;
                }
            }
            return;
        }


        //static int __time;

        static int MapWidth = 16;			
        static int MapHeight = 32;

        static float PlayerX = 8.0f;		
        static float PlayerY = 8.0f;

        //todo use coord struct
        static Coord PlayerCoords = new Coord(8, 8);
        static AI ai = new AI(8, 10);
        static float PlayerA = 0.0f;		
        static float FOV = MathF.PI / 4.0f;
        static float Depth = 16.0f;		
        static float Speed = 10.0f;		
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

        private static void Start()
        {
            for (int x = 0; x < ScreenWidth; x++)
            {
                float RayAngle = (PlayerA - FOV / 2.0f) + ((float)x / (float)ScreenWidth) * FOV;

                float StepSize = 0.1f;
                float DistanceToWall = 0.0f;

                bool Boundary = false;
                bool HitWall = false;
                bool HitAI = false;
                bool Door = false;

                float EyeX = MathF.Sin(RayAngle);
                float EyeY = MathF.Cos(RayAngle);

                RayCasting(StepSize, ref DistanceToWall, ref Boundary, ref HitWall, ref Door, ref HitAI, EyeX, EyeY);

                Shading(buf, x, DistanceToWall, Boundary, Door, HitAI);
            }
        }

        private static void Shading(CharInfo[] buf, int x, float DistanceToWall, bool Boundary, bool Door, bool HitAI)
        {
            int Ceiling = (int)((float)(ScreenHeight / 2.0) - ScreenHeight / ((float)DistanceToWall));
            int Floor = ScreenHeight - Ceiling;

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
                    if (HitAI == true)
                        buf[y * ScreenWidth + x].Attributes = 9;
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

        private static void RayCasting(float StepSize, ref float DistanceToWall, ref bool Boundary, ref bool HitWall, ref bool Door, ref bool HitAI, float EyeX, float EyeY)
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

                        if (map[TestX, TestY] == 'I')
                        {
                            HitAI = true;
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

        private static float[] SortFVector(float[,] f)
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
            Console.CursorVisible = false;
            Console.Clear();
            GameEngine.CreateConsole(70, 220);
            new Game();
        }
    }
}
