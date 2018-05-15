using System;
using System.IO;
using template;
using static template.Scene;
using OpenTK;
using System.Collections.Generic;

namespace Template
{
    public class Raytracer
    {
        // member variables
        public Surface screen;
        public Surface screenDebug;
        public Camera camera;
        public Scene scene;

        double angle = (100 * Math.PI) / 180;


        public static Ray[,] ray = new Ray[512, 512];

        public static Ray[,] GetRays => ray;
        public static List<Primitive> Objects;



        public struct Ray
        {
            public Vector3 O; // ray origin
            public Vector3 D; // ray direction (normalized)
            public float t; // distance
        };


        // initialize
        public void Init()
        {
            ChangePOV(angle);
        }

        // tick: renders one frame
        public void Render()
        {
            screen.Clear(0);

            //
            for (int y = 0; y < screen.height; y++)
            {
                for (int x = 0; x < screen.width; x++)
                {
                    Ray ray;
                    ray.t = 300;
                    ray.O = camera.CamPos;
                    ray.D = screen.pos0 + (x * ((screen.pos1 - screen.pos0) / 512.0f)) + (y * ((screen.pos2 - screen.pos0) / 512.0f));
                    //ray normalized direction
                    ray.D = (ray.D - camera.CamPos).Normalized();

                    Intersection nearest = null;

                    foreach (Primitive p in prims)
                    {
                        Intersection overr = p.Intersection(ref ray);

                        if (overr != null)
                            nearest = overr;
                    }
                        if (x % 10 == 0 && y == screen.height / 2)
                            screenDebug.Line(CordxTrans(camera.CamPos.X), CordzTrans(camera.CamPos.Z), CordxTrans(ray.D.X * ray.t), CordzTrans(ray.D.Z * ray.t), 0xffff00);
                        if (nearest !=null)
                            screen.Plot(x, y, nearest.p.Color);
                }
            }



            //Draw Debug screen
            screenDebug.Line(0, 0, 0, 1024, 0xffffff);
            //camera
            screenDebug.Line(CordxTrans(camera.CamPos.X) - 5, CordzTrans(camera.CamPos.Y) - 1, CordxTrans(camera.CamPos.X) + 5, CordzTrans(camera.CamPos.Y) - 1, 0xffa500);
            screenDebug.Line(CordxTrans(camera.CamPos.X) - 5, CordzTrans(camera.CamPos.Y) + 1, CordxTrans(camera.CamPos.X) + 5, CordzTrans(camera.CamPos.Y) + 1, 0xffa500);
            //screen
            screenDebug.Line(CordxTrans(screen.pos1.X), CordzTrans(screen.pos1.Z), CordxTrans(screen.pos2.X), CordzTrans(screen.pos2.Z), 0x00ffff);
            //bollen
            var sphere1 = GetSphere1;
            screenDebug.DrawSphere(sphere1);
            var sphere2 = GetSphere2;
            screenDebug.DrawSphere(sphere2);
            var sphere3 = GetSphere3;
            screenDebug.DrawSphere(sphere3);
        }

        public int CordxTrans(float x)
        {
            float xx;
            xx = x * 51.2f + 512f;
            return (int)xx;
        }

        public int CordzTrans(float y)
        {
            float yy;
            yy = 1024 - (y * 51.2f) - 150;
            return (int)yy;
        }

        public int InvertxTrans(float x)
        {
            float xx;
            xx = (x - 512) / 51.2f;
            return (int)xx;
        }

        public int InvertyTrans(float y)
        {
            float yy;
            yy = (y - 1024 + 150) / 51.2f;
            return (int)yy;
        }

        public void ChangePOV(double _angle)
        {
            double angle = _angle / 2;
            double disToScreen = 2 / Math.Tan(angle);
            Vector3 ScreenC = camera.CamPos + (float)disToScreen * camera.CamDir;
            screen.pos0 = ScreenC + new Vector3(-2, -2, 0);
            screen.pos1 = ScreenC + new Vector3(2, -2, 0);
            screen.pos2 = ScreenC + new Vector3(-2, 2, 0);
        }

        public static int VecToInt(Vector3 c)
        {
            int color;

            color = ((int)c.Z << 0) | ((int)c.Y << 8) | ((int)c.X << 16);

            return color;
        }
    }
} // namespace Template

/*
 *
 */
