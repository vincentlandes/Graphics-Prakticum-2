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

        }
        // tick: renders one frame
        public void Render()
        {
            screen.Clear(0);

            Objects = GetObjects;

            //
            for (int y = 0; y < screen.height; y++)
            {
                for (int x = 0; x < screen.width; x++)
                {
                    ray[x, y].t = 30;
                    ray[x, y].O = camera.CamPos;
                    ray[x, y].D = screen.pos0 + (x * ((screen.pos1 - screen.pos0) / 512)) + (y * ((screen.pos2 - screen.pos0) / 512));
                    //ray direction
                    ray[x, y].D = (ray[x, y].D - camera.CamPos).Normalized();

                    if (x % 50 == 0 && y == 0)
                        screenDebug.Line(CordxTrans(camera.CamPos.X), CordzTrans(camera.CamPos.Z), CordxTrans(ray[x, y].D.X), CordzTrans(ray[x, y].D.Z), 0xffff00);

                    for (int i = 0; i < Objects.Count; i++) //щ（ﾟДﾟщ）<3 kjoet, munro. tenk joe *x* ^.^ :^)
                    {
                        Objects[i].Intersection(ray[x, y]);
                    }

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
    }
} // namespace Template

/*
 *
             //Draw ray tracer
            for (int y = 0; y < screen.height; y++)
            {
                for (int x = 0; x < screen.width; x++)
                {
                    Ray ray = new Ray();
                    ray.O = camera.CamPos;
                    ray.ray.Z = 15;
                    ray.ray.X = screen.pos1.X + InvertxTrans(x);
                    ray.ray.Y = screen.pos1.Y + InvertyTrans(y);
                    ray.D = (ray.ray - camera.CamPos).Normalized();
                    ray.ray = ray.D * ray.ray.Z;
                    
                    screenDebug.Line(CordxTrans(ray.O.X), CordzTrans(ray.O.Z), CordxTrans(ray.ray.X), CordzTrans(ray.ray.Z) ,0xffff00);
                    //wil een ray schieten van camera naar dit punt in het scherm
                }
            }
 * 
 */
