using System;
using System.Linq;

namespace affine_transformations.Primitives
{
    class Transformation
    {
        private float[] matrix = new float[9];

        public Transformation() { }

        public Transformation(float a, float b, float c,
                              float d, float e, float f,
                              float g, float h, float i)
        {
            matrix = new float[9] { a, b, c, d, e, f, g, h, i };
        }

        public Transformation(float[] matrix)
        {
            if (9 != matrix.Count()) throw new Exception("Bad number of elements in matrix.");
            this.matrix = matrix;
        }

        public float Get(int row, int col)
        {
            return matrix[row * 3 + col];
        }

        public void Set(int row, int col, float value)
        {
            matrix[row * 3 + col] = value;
        }
    }
}
