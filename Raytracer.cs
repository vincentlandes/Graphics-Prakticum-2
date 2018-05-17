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
        public static List<Primitive> Objects;
        public Surface screen;
        public Surface screenDebug;
        public Camera camera;
        public Scene scene;

        //Angle for the PoV
        double angle = (100 * Math.PI) / 180;

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

        // Render: renders one frame
        public void Render()
        {
            screen.Clear(0);
            light1 = GetLight;
            Vector3 Color = new Vector3(1,1,1);

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

                    if (nearest != null)
                    {

                        Ray shadowRay = new Ray();
                        shadowRay.O = nearest.I + nearest.N * Single.Epsilon;
                        shadowRay.D = (light1.pos - shadowRay.O).Normalized();
                       

                        foreach (Primitive p in prims)
                        {
                            if (Color == Vector3.Zero)
                            {
                                if (p.Intersection(ref shadowRay) != null)
                                    Color = nearest.C * Vector3.Zero;
                                else
                                {
                                    //klopt
                                    var ndotl = Vector3.Dot(nearest.N, shadowRay.D);
                                    var dist = (light1.pos - shadowRay.O).Length;
                                    Color = ndotl / (dist * dist) * light1.color * light1.brightness * nearest.C;

                                }

                            }
                        }
                    }
                    /* In de scene intersection function maken die een ray neemt en alle primitives langs loopt, vervolgens kunnen we loop weglaten en hem gebruiken voor Primary&shadow ray.
                     */

                    //Draw 1 in 10 rays on the debugscreen
                    if (x % 10 == 0 && y == screen.height / 2)
                        screenDebug.Line(CordxTrans(camera.CamPos.X), CordzTrans(camera.CamPos.Z), CordxTrans(ray.D.X * ray.t), CordzTrans(ray.D.Z * ray.t), 0xffff00);

                    //Draw the ray on the screen
                    if (nearest !=null)
                        screen.Plot(x, y, nearest.C);
                    Color = new Vector3(1, 1, 1);

                }
            }



            //Draw Debug screen
            //Draw line between screen and debug screen
            screenDebug.Line(0, 0, 0, 1024, 0xffffff);
            //Draw camera as 2 orange lines
            screenDebug.Line(CordxTrans(camera.CamPos.X) - 5, CordzTrans(camera.CamPos.Y) - 1, CordxTrans(camera.CamPos.X) + 5, CordzTrans(camera.CamPos.Y) - 1, 0xffa500);
            screenDebug.Line(CordxTrans(camera.CamPos.X) - 5, CordzTrans(camera.CamPos.Y) + 1, CordxTrans(camera.CamPos.X) + 5, CordzTrans(camera.CamPos.Y) + 1, 0xffa500);
            //Draw screen as a blue line
            screenDebug.Line(CordxTrans(screen.pos1.X), CordzTrans(screen.pos1.Z), CordxTrans(screen.pos2.X), CordzTrans(screen.pos2.Z), 0x00ffff);
            //Draw spheres
            var sphere1 = GetSphere1;
            screenDebug.DrawSphere(sphere1);
            var sphere2 = GetSphere2;
            screenDebug.DrawSphere(sphere2);
            var sphere3 = GetSphere3;
            screenDebug.DrawSphere(sphere3);
        }


        //change coordinate to pixel location
        public int CordxTrans(float x)
        {
            float xx;
            xx = x * 51.2f + 512f;
            return (int)xx;
        }

        //change coordinate to pixel location
        public int CordzTrans(float y)
        {
            float yy;
            yy = 1024 - (y * 51.2f) - 150;
            return (int)yy;
        }

        //change pixel location to coordinate
        public int InvertxTrans(float x)
        {
            float xx;
            xx = (x - 512) / 51.2f;
            return (int)xx;
        }

        //change pixel location to coordinate
        public int InvertyTrans(float y)
        {
            float yy;
            yy = (y - 1024 + 150) / 51.2f;
            return (int)yy;
        }

        //calculate the screen with the given angle
        public void ChangePOV(double _angle)
        {
            double angle = _angle / 2;
            double disToScreen = 2 / Math.Tan(angle);
            Vector3 ScreenC = camera.CamPos + (float)disToScreen * camera.CamDir;
            screen.pos0 = ScreenC + new Vector3(-2, -2, 0);
            screen.pos1 = ScreenC + new Vector3(2, -2, 0);
            screen.pos2 = ScreenC + new Vector3(-2, 2, 0);
        }

        //change the Color vector to an integer
        public static int VecToInt(Vector3 c)
        {
            int color;
            color = ((int)c.Z << 0) | ((int)c.Y << 8) | ((int)c.X << 16);
            return color;
        }
    }
}
