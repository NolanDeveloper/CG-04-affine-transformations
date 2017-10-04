using System;

namespace affine_transformations.Primitives
{
    static class AffineTransformations
    {
        public static Transformation Scale(float fx, float fy)
        {
            return new Transformation(
                fx,  0,  0,
                 0, fy,  0,
                 0,  0,  1
            );
        }

        public static Transformation Rotate(float angle)
        {
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);
            return new Transformation(
                 cos,  sin,   0,
                -sin,  cos,   0,
                    0,   0,   1
            );
        }

        public static Transformation Translate(float dx, float dy)
        {
            return new Transformation(
                 1,  0,  0,
                 0,  1,  0,
                dx, dy,  1
            );
        }

        public static Transformation Compose(Transformation t1, Transformation t2)
        {
            Transformation result = new Transformation();
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                {
                    float value = 0;
                    for (int k = 0; k < 3; ++k)
                        value += t1.Get(i, k) * t2.Get(k, j);
                    result.Set(i, j, value);
                }
            return result;
        }
    }
}
