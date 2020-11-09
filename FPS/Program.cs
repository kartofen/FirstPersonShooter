using System;
using System.Collections.Generic;
using ConsoleGameEngine;

namespace FPS
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

        protected override void OnKeyPressed()
        {
            if(keys['W'].Held)
            {
                PlayerX += MathF.Sin(PlayerA) * Speed * deltaTime;
                PlayerY += MathF.Cos(PlayerA) * Speed * deltaTime;
                if(map[(int)PlayerX, (int)PlayerY] != '.')
                {
                    PlayerX -= MathF.Sin(PlayerA) * Speed * deltaTime;
                    PlayerY -= MathF.Cos(PlayerA) * Speed * deltaTime;
                }
            }
            if(keys['A'].Held)
            {
                PlayerA -= (1f) * deltaTime;
            }
            if(keys['S'].Held)
            {
                PlayerX -= MathF.Sin(PlayerA) * Speed * deltaTime;
                PlayerY -= MathF.Cos(PlayerA) * Speed * deltaTime;
                if(map[(int)PlayerX, (int)PlayerY] != '.')
                {
                    PlayerX += MathF.Sin(PlayerA) * Speed * deltaTime;
                    PlayerY += MathF.Cos(PlayerA) * Speed * deltaTime;
                }
            }
            if(keys['D'].Held)
            {
                PlayerA += (1f) * deltaTime;
            }
            if(keys['Q'].Held)
            {
                PlayerX -= MathF.Cos(PlayerA) * Speed * deltaTime;
                PlayerY += MathF.Sin(PlayerA) * Speed * deltaTime;
                if(map[(int)PlayerX, (int)PlayerY] != '.')
                {
                    PlayerX += MathF.Cos(PlayerA) * Speed * deltaTime;
                    PlayerY -= MathF.Sin(PlayerA) * Speed * deltaTime;
                    
                }
            }
            if(keys['E'].Held)
            {
                PlayerX += MathF.Cos(PlayerA) * Speed * deltaTime;
                PlayerY -= MathF.Sin(PlayerA) * Speed * deltaTime;
                if(map[(int)PlayerX, (int)PlayerY] != '.')
                {
                    PlayerX -= MathF.Cos(PlayerA) * Speed * deltaTime;
                    PlayerY += MathF.Sin(PlayerA) * Speed * deltaTime;
                }
            }
            if(keys['F'].Released)
            {
                if(map[(int)PlayerX + 1, (int)PlayerY] == 'D') map[(int)PlayerX + 1, (int)PlayerY] = '.';
                else if(map[(int)PlayerX - 1, (int)PlayerY] == 'D') map[(int)PlayerX - 1, (int)PlayerY] = '.';
                else if(map[(int)PlayerX, (int)PlayerY + 1] == 'D') map[(int)PlayerX , (int)PlayerY + 1] = '.';
                else if(map[(int)PlayerX, (int)PlayerY - 1] == 'D') map[(int)PlayerX, (int)PlayerY - 1] = '.';
            }
            if(keys[0x20].Released) //spacebar
            {
                Object o;
                o.coords.X = PlayerX + MathF.Sin(PlayerA) * 3.0f;
                o.coords.Y = PlayerY + MathF.Cos(PlayerA) * 3.0f;
                o.velocity.X = MathF.Sin(PlayerA) * 8.0f;
                o.velocity.Y = MathF.Cos(PlayerA) * 8.0f;
                o.sprite = FireBall;
                o.remove = false;
                ObjList.Add(o);
            }
            return;
        }

        protected override void AddInformationToBuffer()
        {
            WriteObjects();

            for (int nx = 0; nx < MapHeight; nx++)
                for (int ny = 0; ny < MapWidth; ny++)
                {
                    DrawB(nx, ny, (byte)(int)map[nx, ny], (short)COLOUR.FG_BLACK);
                }

            DrawB((int)PlayerX, (int)PlayerY, (byte)(int)'P', (short)COLOUR.FG_BLACK);
            DrawToScreenBuffer("FPS: " + 1f / deltaTime, 0, 33, COLOUR.FG_DARK_RED);
            DrawToScreenBuffer("Angle: " + PlayerA, 1, 33, COLOUR.FG_DARK_RED);
            DrawToScreenBuffer("GameTick: " + GameTick, 2, 33, COLOUR.FG_DARK_RED);

            for (int i = 0; i < buf.Length; i++)
            {
                if (buf[i].Attributes == 0)
                {
                    SetAttribute(i, 0, COLOUR.FG_GREY);
                }
            }
            return;
        }



        public struct Object
        {
            public CoordF coords;
            public CoordF velocity;
            public Sprite sprite;
            public bool remove;
            public Object(CoordF coords, CoordF velocity, Sprite sprite, bool remove)
            {
                this.coords = coords;
                this.sprite = sprite;
                this.velocity = velocity;
                this.remove = remove;
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

        static Sprite FireBall = new Sprite(
            new char[,] {
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', '█', '█', ' ', ' ', ' '},
                {' ', ' ', '█', '█', '█', '█', ' ', ' '},
                {' ', ' ', '█', '█', '█', '█', ' ', ' '},
                {' ', ' ', ' ', '█', '█', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},
                {' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '},

            },
            new short[,] { 
                {8, 8, 8, 8, 8, 8, 8, 8},
                {8, 8, 8, 8, 8, 8, 8, 8},
                {8, 8, 8, 8, 8, 8, 8, 8},
                {8, 8, 8, 4, 4, 8, 8, 8},
                {8, 8, 8, 4, 4, 8, 8, 8},
                {8, 8, 8, 8, 8, 8, 8, 8},
                {8, 8, 8, 8, 8, 8, 8, 8},
                {8, 8, 8, 8, 8, 8, 8, 8},
                {8, 8, 8, 8, 8, 8, 8, 8},
            },
            10, 8
        );
        
        static Object Obj1 = new Object(new CoordF(8.5f, 11.5f), new CoordF(0, 0), LampSprite, false);
        static Object Obj2 = new Object(new CoordF(25.5f, 8.5f), new CoordF(0, 0), LampSprite, false);
        static List<Object> ObjList = new List<Object>();
        static List<Object> Enemies = new List<Object>();

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
            for(int i = 0; i < ObjList.Count; i++)
            {
                Object tempObj;
                tempObj = ObjList[i];
                tempObj.coords.X += tempObj.velocity.X * deltaTime;
                tempObj.coords.Y += tempObj.velocity.Y * deltaTime;
                ObjList[i] = tempObj;

                if(map[(int)ObjList[i].coords.X, (int)ObjList[i].coords.Y] == '#')
                {
                    Object tempObj_;
                    tempObj_ = ObjList[i];
                    tempObj_.remove = true;
                    ObjList[i] = tempObj_;
                }

                float VecX = ObjList[i].coords.X - PlayerX;
                float VecY = ObjList[i].coords.Y - PlayerY;
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
                    float ObjectAspectRatio = (float)ObjList[i].sprite.height / (float)ObjList[i].sprite.width;
                    float ObjectWidth = ObjectHeight / ObjectAspectRatio;
                    float MiddleOfObject = (0.5f * (ObjectAngle / (FOV / 2.0f)) + 0.5f) * (float)ScreenWidth;

                    for (float lx = 0; lx < ObjectWidth; lx++)
                        for (float ly = 0; ly < ObjectHeight; ly++)
                        {
                            float SampleX = lx / ObjectWidth;
                            float SampleY = ly / ObjectHeight;
                            char s_char = ObjList[i].sprite.SampleGlyphCharSprite(SampleX, SampleY);
                            byte s_byte = ObjList[i].sprite.SampleGlyphByteSprite(SampleX, SampleY);
                            short s_attribute = ObjList[i].sprite.SampleGlyphAttribute(SampleX, SampleY);
                            int ObjectColumn = (int)(MiddleOfObject + lx - (ObjectWidth / 2.0f));
                            if (ObjectColumn >= 0 && ObjectColumn < ScreenWidth)
                                if (s_char != ' ' && DepthBuffer[ObjectColumn] >= DistanceFromPlayer || s_byte != 32 && DepthBuffer[ObjectColumn] >= DistanceFromPlayer)
                                {
                                    DrawB((int)ObjectColumn, (int)(ObjectCeiling + ly), s_byte, s_attribute);
                                    DepthBuffer[ObjectColumn] = DistanceFromPlayer;
                                }
                        }
                }
                if(ObjList[i].remove == true)
                {
                    ObjList.Remove(ObjList[i]);
                }
            }
        }

        static void Shading(CharInfo[] buf, int x, float DistanceToWall, bool Boundary, bool Door)
        {
            int Ceiling = (int)((float)(ScreenHeight / 2.0) - ScreenHeight / ((float)DistanceToWall));
            int Floor = ScreenHeight - Ceiling;

            DepthBuffer[x] = DistanceToWall;

            PIXEL Shade;
            COLOUR attribute;
            for (int y = 0; y < ScreenHeight; y++)
            {

                if (y < Ceiling)
                {
                    SetPixel(x, y, PIXEL.PIXEL_BLANK);
                }
                else if (y > Ceiling && y <= Floor)
                {

                    if (DistanceToWall <= Depth / 4.0f) Shade = PIXEL.PIXEL_SOLID;
                    else if (DistanceToWall < Depth / 3.0f) Shade = PIXEL.PIXEL_THREEQUARTERS;
                    else if (DistanceToWall < Depth / 2.0f) Shade = PIXEL.PIXEL_HALF;
                    else if (DistanceToWall < Depth) Shade = PIXEL.PIXEL_QUARTER;
                    else Shade = PIXEL.PIXEL_BLANK;

                    if (Boundary == true)
                    {
                        Shade = PIXEL.PIXEL_BLANK;
                        attribute = COLOUR.FG_BLACK;
                    }
                    else
                    {
                        if (Door == true)
                            attribute = COLOUR.FG_DARK_GREEN;
                        else
                            attribute = COLOUR.FG_DARK_BLUE;
                    }
                    
                    DrawA(x, y, Shade, attribute);
                }

                else
                {
                    float shading = 1.0f - (((float)y - ScreenHeight / 2.0f) / ((float)ScreenHeight / 2.0f));
                    if (shading < 0.25) Shade = PIXEL.PIXEL_SOLID;
                    else if (shading < 0.5) Shade = PIXEL.PIXEL_THREEQUARTERS;
                    else if (shading < 0.75) Shade = PIXEL.PIXEL_HALF;
                    else if (shading < 0.9) Shade = PIXEL.PIXEL_QUARTER;
                    else Shade = PIXEL.PIXEL_BLANK;

                    DrawA(x, y, Shade, COLOUR.FG_DARK_GREY);
                    //buf[y * ScreenWidth + x].Char.AsciiChar = Shade;
                    //buf[y * ScreenWidth + x].Attributes = (short)COLOUR.FG_DARK_GREY;
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
            GameEngine.CreateConsole(70, 220, 10, 10, "FirstPersonShooter"); //70, 220 normal; 120 320 very good
            Console.CursorVisible = false;
            Console.Clear();
            new Game();
        }
    }
}
