using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PlygonHandle.Logic
{
    public static class PolygonHelper
    {
        public static double GetPolygonArea(Path path)
        {
            PointCollection points = GetPolygonPoints(path);

            return CalculatePolygonArea(points);
        }

        private static double CalculatePolygonArea(PointCollection points)
        {
            double sum = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                sum += points[i].X * points[i + 1].Y - points[i].Y * points[i + 1].X;
            }

            return Math.Abs(sum * 0.5);
        }

        private static PointCollection GetPolygonPoints(Path path)
        {
            PointCollection points = new();

            if (path is null)
                return points;

            PathGeometry polygon = path.RenderedGeometry.GetFlattenedPathGeometry();

            foreach (PathFigure figure in polygon.Figures)
            {
                points.Add(figure.StartPoint);

                foreach (PathSegment segment in figure.Segments)
                {
                    foreach (Point point in ((PolyLineSegment)segment).Points)
                    {
                        points.Add(point);
                    }
                }
            }

            return points;
        }
    }
}
