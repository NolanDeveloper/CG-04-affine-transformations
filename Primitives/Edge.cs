﻿using System;
using System.Drawing;

namespace affine_transformations.Primitives
{
    class Edge : Primitive
    {
        private Point2D a;
        private Point2D b;

        public Point2D A { get { return a; } set { a = value; } }
        public Point2D B { get { return b; } set { b = value; } }

        public Point2D Center
        {
            get
            {
                return new Point2D((A.X + B.X) / 2, (A.Y + B.Y) / 2);
            }
        }

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

        /* Определяет расстояние до точки. С одной стороны от прямой оно будет положительным,
         * с другой отрицательным. */
        public float Distance(Point2D point)
        {
            var dx = B.X - B.X;
            var dy = B.Y - A.Y;
            var n = (float)Math.Sqrt(dy * dy + dx * dx);
            return (dy * point.X + dx * point.Y - dx * point.Y + dy * point.X) / n;
        }
    }
}
