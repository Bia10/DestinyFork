namespace Destiny.Maple
{
    public sealed class Line
    {
        public Point Point1 { get; }
        public Point Point2 { get; }

        public Line(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
        }
    }
}
