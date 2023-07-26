namespace NestedCoordinates
{
    public class Planet : SpaceObject
    {

        public override float LODThresholdForCoordinateSystem(CoordinateSystem coordinateSystem)
        {
            return lodThreshold;
        }
    }
}