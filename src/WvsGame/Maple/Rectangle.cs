namespace Destiny.Maple
{
    public class Rectangle
    {
        public Point LeftTop { get; }
        public Point RightBottom { get; }

        public Rectangle(Point leftTop, Point rightBottom)
        {
            LeftTop = leftTop;
            RightBottom = rightBottom;
        }

        public Rectangle(int x, int y, int width, int height)
        {
        }
    }
}
