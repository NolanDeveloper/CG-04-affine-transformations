using affine_transformations.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace affine_transformations
{
    public partial class Form1 : Form
    {
        private Graphics graphics;

        private List<Point2D> points = new List<Point2D>();
        private List<Edge> edges = new List<Edge>();
        private List<Polygon> polygons = new List<Polygon>();

        private bool shouldStartNewPolygon = true;
        private bool shouldStartNewEdge = true;
        private Point2D edgeFirstPoint;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(2048, 2048);
            graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs args = (MouseEventArgs)e;
            Point2D p = Point2D.FromPoint(args.Location);
            if (rbPoint.Checked)
                points.Add(p);
            else if (rbEdge.Checked)
            {
                if (shouldStartNewEdge)
                {
                    edgeFirstPoint = p;
                    shouldStartNewEdge = false;
                }
                else
                {
                    edges.Add(new Edge(edgeFirstPoint, p));
                    shouldStartNewEdge = true;
                }
            }
            else if (rbPolygon.Checked)
            {
                if (shouldStartNewPolygon)
                {
                    polygons.Add(new Polygon());
                    shouldStartNewPolygon = false;
                }
                polygons[polygons.Count - 1].Points.Add(p);
            }
            Redraw();
        }

        private void Redraw()
        {
            graphics.Clear(Color.White);
            if (!shouldStartNewEdge) edgeFirstPoint.Draw(graphics);
            points.ForEach((p) => p.Draw(graphics));
            edges.ForEach((e) => e.Draw(graphics));
            polygons.ForEach((p) => p.Draw(graphics));
            pictureBox1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            shouldStartNewPolygon = true;
        }

        private void rbPolygon_CheckedChanged(object sender, EventArgs e)
        {
            shouldStartNewPolygon = true;
        }

        private void TransformAll(Transformation t)
        {
            points.ForEach((p) => p.Apply(t));
            edges.ForEach((e) => e.Apply(t));
            polygons.ForEach((p) => p.Apply(t));
            Redraw();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TransformAll(AffineTransformations.Rotate(0.1f));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TransformAll(AffineTransformations.Scale(1.1f, 1.1f));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TransformAll(AffineTransformations.Translate(20.0f, 15.0f));
        }
    }
}
