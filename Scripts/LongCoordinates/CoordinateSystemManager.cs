using UnityEngine;

namespace LongCoordinates
{
    public class CoordinateSystemManager
    {

        private readonly CoordinateSystem[] coordinateSystems;
        private SpaceObject pivotObject;

        public static CoordinateSystemManager FromConfig(CoordinateSystemsConfig coordinateSystemsConfig)
        {
            var coordinateSystems = new CoordinateSystem[coordinateSystemsConfig.coordinateSystems.Length];
            for (var i = 0; i < coordinateSystemsConfig.coordinateSystems.Length; i++)
            {
                var coordinateSystemData = coordinateSystemsConfig.coordinateSystems[i];
                coordinateSystems[i] = new CoordinateSystem(coordinateSystemData.name, coordinateSystemData.scaleFactor);
            }

            return new CoordinateSystemManager(coordinateSystems);
        }
    
        private CoordinateSystemManager(CoordinateSystem[] coordinateSystems)
        {
            this.coordinateSystems = coordinateSystems;
        }

        public void AddSpaceObject(SpaceObject spaceObject)
        {
            coordinateSystems[spaceObject.coordinateSystemLevel].AddSpaceObject(spaceObject);
        }

        public void WithPivotObject(SpaceObject pivotObject)
        {
            this.pivotObject = pivotObject;
        }
    
        public void Update()
        {
            UpdatePivotObjectPosition();
            for (var i = pivotObject.coordinateSystemLevel; i < coordinateSystems.Length; i++)
            {
                coordinateSystems[i].Update(pivotObject);
            }
        }

        private void UpdatePivotObjectPosition()
        {
            var positionCache = pivotObject.transform.position;
            var pivotTransformPosition = positionCache;
            var pivotSpaceTransformPosition = pivotObject.spaceTransform.position;

            pivotSpaceTransformPosition.Add(new Vector3L(
                (long) pivotTransformPosition.x,
                (long) pivotTransformPosition.y,
                (long) pivotTransformPosition.z
            ));

            positionCache -= new Vector3((long) pivotTransformPosition.x, (long) pivotTransformPosition.y,
                (long) pivotTransformPosition.z);
            pivotObject.transform.position = positionCache;
        }
    }
}