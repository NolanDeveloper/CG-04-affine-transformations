using System.Drawing;

namespace affine_transformations.Primitives
{
    interface Primitive
    {
        void Draw(Graphics g);
        void Apply(Transformation t);
    }
}
