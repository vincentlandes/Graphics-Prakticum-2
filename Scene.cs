using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Template;
using static Template.Raytracer;

namespace template
{
    public class Scene
    {
        public static Sphere sphere1, sphere2, sphere3;
        public Plane plane;
        public static List<Primitive> prims = new List<Primitive>();

        public Scene()
        {
            plane = new Plane(new Vector3(0, 1, 0), -1f);
            sphere1 = new Sphere(new Vector3(0, 0, 7), 2f, new Vector3(255, 0, 0));
            sphere2 = new Sphere(new Vector3(-5, 0, 7), 2f,new Vector3(0, 255, 0));
            sphere3 = new Sphere(new Vector3(5, 0, 7), 2f, new Vector3(173, 216, 230));

            prims.Add(plane);
            prims.Add(sphere1);
            prims.Add(sphere2);
            prims.Add(sphere3);
            //(╯°□°）╯︵ ┻━┻  ヽ(●ﾟ´Д｀ﾟ●)ﾉﾟ (✖╭╮✖)
        }


        public static Sphere GetSphere1 => sphere1;
        public static Sphere GetSphere2 => sphere2;
        public static Sphere GetSphere3 => sphere3;
    }

    public abstract class Primitive
    {
        public Vector3 Color;
        public abstract Intersection Intersection(ref Ray ray);
    }


    public class Sphere : Primitive
    {
        public Vector3 pos;
        public float rad;
        //public Vector3 Color;

        public Sphere(Vector3 _pos, float _rad, Vector3 _color)
        {
            pos = _pos;
            rad = _rad;
            Color = _color;           
        }

        public override Intersection Intersection(ref Ray ray) 
        {
            Vector3 CSphere = pos;
            float t = Vector3.Dot(CSphere, ray.D);
            Vector3 q = CSphere - t * ray.D;
            float Psq = Vector3.Dot(q, q);
            if (Psq > this.rad) return null;
            t -= (float)Math.Sqrt(this.rad - Psq);
            if ((t < ray.t) && (t > 0))
                ray.t = t;
            else return null;

            Vector3 I = ray.O + (ray.t * ray.D);
            Vector3 N = (I - this.pos).Normalized();
            return new Intersection(t, N, this);
            
            
            /*Vector3 I = ray.O + (ray.t * ray.D);
            Vector3 N = (I - this.pos).Normalized();
            Vector3 Color = this.Color;
            return new Intersection(I,N,Color);*/
        }
    }

    public class Intersection
    {
        public float t;
        public Vector3 N;
        public Primitive p;

        public Intersection(float _t, Vector3 _N, Primitive _p)
        {
            t = _t;
            N = _N;
            p = _p;
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

        public override Intersection Intersection(ref Ray ray)
        {
            return null;
        }
    }
}
