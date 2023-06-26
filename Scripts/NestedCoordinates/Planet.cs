using UnityEngine;

namespace NestedCoordinates
{
    public class Planet : SpaceObject
    {
        void Start()
        {
            transform.localScale = new Vector3(size, size, size);
        }

        public override float LODThresholdForCoordinateSystem(CoordinateSystem coordinateSystem)
        {
            return size;
        }
    }
}