using UnityEngine;

namespace NestedCoordinates
{
    public abstract class SpaceObject : MonoBehaviour, ISpaceObjectLOD
    {

        public float size;
        
        public abstract float LODThresholdForCoordinateSystem(CoordinateSystem coordinateSystem);
    }
}