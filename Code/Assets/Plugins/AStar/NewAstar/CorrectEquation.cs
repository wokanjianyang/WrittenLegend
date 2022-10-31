using Game.NewAstar;

namespace Astar
{
    public class CorrectEquation
    {
        public float b;
        public float k;

        public CorrectEquation(Position start, Position end)
        {
            var v1 = start.X         - end.X;
            var v2 = start.Y         - end.Y;
            var v3 = start.X * end.Y - end.X * start.Y;
            if (v1 != 0)
            {
                k = 1f * v2 / v1;
                b = 1f * v3 / v1;
            }
            else
            {
                k = 0;
                b = start.Y;
            }
        }

        public int GetX(int y)
        {
            return (int) ((y - b) / k);
        }

        public int GetY(int x)
        {
            return (int) (k * x + b);
        }
    }
}