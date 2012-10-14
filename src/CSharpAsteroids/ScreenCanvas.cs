using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace Asteroids
{
    /// <summary>
    /// Summary description for ScreenCanvas.
    /// </summary>
    public class ScreenCanvas
    {
        protected ArrayList points;
        protected ArrayList pensLineColor;
        protected ArrayList polygons;
        protected ArrayList pensPolyColor;

        private Point ptLast;
        private Pen penLast;
        private bool ptLastDefined;

        public ScreenCanvas()
        {
            ptLastDefined = false;
            points = new ArrayList();
            pensLineColor = new ArrayList();
            polygons = new ArrayList();
            pensPolyColor = new ArrayList();
        }

        public void Clear()
        {
            points.Clear();
            pensLineColor.Clear();
            polygons.Clear();
            pensPolyColor.Clear();
        }

        public void Draw(PaintEventArgs e)
        {
            Pen penDraw;
            if (points.Count % 2 == 0)
            {
                Point pt1, pt2;
                int iLinePen = 0;
                for (int i = 0; i < points.Count;)
                {
                    pt1 = (Point)points[i++];
                    pt2 = (Point)points[i++];
                    penDraw = (Pen)pensLineColor[iLinePen++];
                    e.Graphics.DrawLine(penDraw, pt1, pt2);
                }

                for (int i = 0; i < polygons.Count; i++)
                {
                    Point[] poly = (Point[])polygons[i];
                    penDraw = (Pen)pensPolyColor[i];
                    e.Graphics.DrawPolygon(penDraw, poly);
                }
            }
        }

        public void AddLine(Point ptStart, Point ptEnd, Pen penColor)
        {
            points.Add(ptStart);
            points.Add(ptEnd);
            pensLineColor.Add(penColor);
            ptLastDefined = true;
            ptLast = ptEnd;
            penLast = penColor;
        }

        public void AddLine(Point ptStart, Point ptEnd)
        {
            AddLine(ptStart, ptEnd, Pens.White);
        }

        public void AddLineTo(Point ptEnd)
        {
            if (ptLastDefined)
            {
                points.Add(ptLast);
                points.Add(ptEnd);
                pensLineColor.Add(penLast);
                ptLast = ptEnd;
            }
        }

        public void AddPolygon(Point[] ptArray, Pen penColor)
        {
            polygons.Add(ptArray);
            pensPolyColor.Add(penColor);
        }

        public void AddPolygon(Point[] ptArray)
        {
            AddPolygon(ptArray, Pens.White);
        }
    }
}
