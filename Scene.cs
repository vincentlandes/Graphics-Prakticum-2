using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using static Template.Raytracer;

namespace template
{
    public class Scene
    {
        public static Sphere sphere1, sphere2, sphere3;
        public Plane plane;
        public static List<Primitive> Objects;





        public Scene()
        {
            Objects = new List<Primitive>();

            plane = new Plane(new Vector3(0, 1, 0), -1f);
            sphere1 = new Sphere(new Vector3(0, 0, 7), 1f, 0x00ff00);
            sphere2 = new Sphere(new Vector3(-5, 0, 7), 1f, 0xff0000);
            sphere3 = new Sphere(new Vector3(5, 0, 7), 1f, 0xadd8e6);

            Objects.Add(plane);
            Objects.Add(sphere1);
            Objects.Add(sphere2);
            Objects.Add(sphere3); //(╯°□°）╯︵ ┻━┻  ヽ(●ﾟ´Д｀ﾟ●)ﾉﾟ (✖╭╮✖)
        }

        public static Sphere GetSphere1 => sphere1;
        public static Sphere GetSphere2 => sphere2;
        public static Sphere GetSphere3 => sphere3;
        public static List<Primitive> GetObjects => Objects;
    }

    public abstract class Primitive
    {

        public virtual void Intersection(Ray ray)
        {
            //neen
        }


    }


    public class Sphere : Primitive
    {
        public Vector3 pos;
        public float rad;
        public int Color;

        public Sphere(Vector3 _pos, float _rad, int _color)
        {
            pos = _pos;
            rad = _rad;
            Color = _color;
            
        }

        public override void Intersection(Ray ray) 
        {
            Sphere sphere1 = Scene.GetSphere1;

            Vector3 CSphere = pos - ray.O;
            float t = Vector3.Dot(CSphere, ray.D);
            Vector3 q = CSphere - t * ray.D;
            float Psq = Vector3.Dot(q, q);
            if (Psq > sphere1.rad) return;
            t -= (float)Math.Sqrt(sphere1.rad - Psq);
            if ((t < ray.t) && (t > 0))
                ray.t = t;
        }


    }

    public class Plane : Primitive
    {
        public Vector3 norm;
        public float dist;

        public Plane(Vector3 _norm, float _dist)
        {
            norm = _norm;
            dist = _dist;
        }
    }
}
