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

        private Primitive SelectedPrimitive
        {
            get
            {
                if (null == treeView1.SelectedNode) return null;
                var p = (Primitive)treeView1.SelectedNode.Tag;
                buttonRotate.Enabled = p is Edge;
                return p;
            }
            set
            {
                Redraw();
            }
        }

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
            points.ForEach((p) => p.Draw(graphics, p == SelectedPrimitive));
            edges.ForEach((e) => e.Draw(graphics, e == SelectedPrimitive));
            polygons.ForEach((p) => p.Draw(graphics, p == SelectedPrimitive));
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

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedPrimitive = (Primitive)e.Node.Tag;
            Redraw();
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Delete != e.KeyCode) return;
            if (null == SelectedPrimitive) return;
            if (SelectedPrimitive is Point2D) points.Remove((Point2D)SelectedPrimitive);
            if (SelectedPrimitive is Edge) edges.Remove((Edge)SelectedPrimitive);
            if (SelectedPrimitive is Polygon) polygons.Remove((Polygon)SelectedPrimitive);
            treeView1.SelectedNode.Remove();
            if (null != treeView1.SelectedNode)
                SelectedPrimitive = (Primitive)treeView1.SelectedNode.Tag;
            else
                SelectedPrimitive = null;
            Redraw();
        }

        private void buttonRotate_Click(object sender, EventArgs e)
        {
            var edge = (Edge)SelectedPrimitive;
            var center = edge.Center;
            var moveToCenter = Transformation.Translate(-center.X, -center.Y);
            var rotate = Transformation.Rotate((float) Math.PI / 2);
            var moveBack = Transformation.Translate(center.X, center.Y);
            edge.Apply(moveToCenter * rotate * moveBack);
            Redraw();
        }
    }
}
