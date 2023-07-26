using UnityEngine;

namespace NestedCoordinates
{
    public abstract class SpaceObject : MonoBehaviour, ISpaceObjectLOD
    {

        public float lodThreshold;
        
        public abstract float LODThresholdForCoordinateSystem(CoordinateSystem coordinateSystem);
    }
}