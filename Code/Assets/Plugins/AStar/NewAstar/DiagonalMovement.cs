namespace Game.NewAstar
{
    public enum DiagonalMovement
    {
        Always = 1,
        Never,
        IfAtMostOneObstacle,
        OnlyWhenNoObstacles
    }
}