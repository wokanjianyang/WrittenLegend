namespace Game
{
    public static class MathHelper
    {
        public static float RadToDeg(float radians)
        {
            return (float)(radians * 180 / System.Math.PI);
        }
        
        public static float DegToRad(float degrees)
        {
            return (float)(degrees * System.Math.PI / 180);
        }

        public static long GetSequence1(long level)
        {
            return level * (level + 1) / 2;
        }
    }
}