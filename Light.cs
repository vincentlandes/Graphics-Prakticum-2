using OpenTK;

namespace template
{
    public class Light
    {
        public Vector3 pos;
        public Vector3 color;
        public float brightness;

        public Light(Vector3 _pos, Vector3 _color, float _brightness)
        {
            pos = _pos;
            color = _color;
            brightness = _brightness;
        }
    }
}
