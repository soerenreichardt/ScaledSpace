namespace NestedCoordinates
{
    public interface ISpaceObjectLOD
    {
        // TODO: replace with static dispatch methods for each coordinate system
        float LODThresholdForCoordinateSystem(CoordinateSystem coordinateSystem);
    }
}