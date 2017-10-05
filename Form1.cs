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

        private Primitive selectedPrimitive;

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
            {
                TreeNode node = treeView1.Nodes.Add("Точка");
                node.Tag = p;
                points.Add(p);
            }
            else if (rbEdge.Checked)
            {
                if (shouldStartNewEdge)
                {
                    edgeFirstPoint = p;
                    shouldStartNewEdge = false;
                }
                else
                {
                    Edge edge = new Edge(edgeFirstPoint, p);
                    TreeNode node = treeView1.Nodes.Add("Отрезок");
                    node.Tag = edge;
                    edges.Add(edge);
                    shouldStartNewEdge = true;
                }
            }
            else if (rbPolygon.Checked)
            {
                if (shouldStartNewPolygon)
                {
                    Polygon polygon = new Polygon();
                    TreeNode node = treeView1.Nodes.Add("Многоугольник");
                    node.Tag = polygon;
                    polygons.Add(polygon);
                    shouldStartNewPolygon = false;
                }
                polygons[polygons.Count - 1].Points.Add(p);
            }
            Redraw();
        }

        private void Redraw()
        {
            graphics.Clear(Color.White);
            if (!shouldStartNewEdge) edgeFirstPoint.Draw(graphics, false);
            points.ForEach((p) => p.Draw(graphics, p == selectedPrimitive));
            edges.ForEach((e) => e.Draw(graphics, e == selectedPrimitive));
            polygons.ForEach((p) => p.Draw(graphics, p == selectedPrimitive));
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
            if (null == selectedPrimitive) return;
            selectedPrimitive.Apply(t);
            Redraw();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TransformAll(Transformation.Rotate(0.1f));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TransformAll(Transformation.Scale(1.1f, 1.1f));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TransformAll(Transformation.Translate(20.0f, 15.0f));
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selectedPrimitive = (Primitive)e.Node.Tag;
            Redraw();
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Delete != e.KeyCode) return;
            if (null == selectedPrimitive) return;
            if (selectedPrimitive is Point2D) points.Remove((Point2D)selectedPrimitive);
            if (selectedPrimitive is Edge) edges.Remove((Edge)selectedPrimitive);
            if (selectedPrimitive is Polygon) polygons.Remove((Polygon)selectedPrimitive);
            treeView1.SelectedNode.Remove();
            if (null != treeView1.SelectedNode)
                selectedPrimitive = (Primitive)treeView1.SelectedNode.Tag;
            else
                selectedPrimitive = null;
            Redraw();
        }
    }
}
