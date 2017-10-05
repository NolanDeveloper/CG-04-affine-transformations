using System;
using System.Drawing;

namespace affine_transformations.Primitives
{
    class Edge : Primitive
    {
        private Point2D a;
        private Point2D b;

        public Point2D A { get { return a; } set { a = value; } }
        public Point2D B { get { return b; } set { b = value; } }

        public Edge(Point2D a, Point2D b)
        {
            this.a = a;
            this.b = b;
        }

        public void Draw(Graphics g, bool selected)
        {
            Pen pen = new Pen(selected ? Color.Red : Color.Black);
            pen.Width = 2;
            g.DrawLine(pen, A.X, A.Y, B.X, B.Y);
        }

        public void Apply(Transformation t)
        {
            A.Apply(t);
            B.Apply(t);
        }
    }
}
