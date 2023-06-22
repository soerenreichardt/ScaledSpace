using System.Collections.Generic;
using UnityEngine;

namespace LongCoordinates
{
    public class CoordinateSystem
    {
        private readonly string name;
        private readonly long scaleFactor;
        private readonly List<SpaceObject> spaceObjects;

        public CoordinateSystem(string name, long scaleFactor)
        {
            this.name = name;
            this.scaleFactor = scaleFactor;
            this.spaceObjects = new List<SpaceObject>();
        }

        public void AddSpaceObject(SpaceObject spaceObject)
        {
            var positionCache = spaceObject.transform.position;
            var transformPosition = positionCache;
            spaceObject.spaceTransform.position.Add(
                (long) transformPosition.x,
                (long) transformPosition.y,
                (long) transformPosition.z
            );

            positionCache -= new Vector3(
                (long) transformPosition.x,
                (long) transformPosition.y,
                (long) transformPosition.z
            );
            spaceObject.transform.position = positionCache;
            this.spaceObjects.Add(spaceObject);
        }

        public void Update(SpaceObject pivotObject)
        {
            foreach (var spaceObject in spaceObjects)
            {
                var spaceObjectTransformPosition = spaceObject.transform.position;
                var spaceObjectSpaceTransformPosition = spaceObject.spaceTransform.position;

                var longRelativePosition = spaceObjectSpaceTransformPosition - pivotObject.spaceTransform.position;
                var relativePosition = longRelativePosition.ToFloat();

                relativePosition.x += spaceObjectTransformPosition.x % 1;
                relativePosition.y += spaceObjectTransformPosition.y % 1;
                relativePosition.z += spaceObjectTransformPosition.z % 1;

                spaceObjectSpaceTransformPosition.Add(spaceObjectTransformPosition - relativePosition);

                spaceObject.transform.position = relativePosition;
            }
        }
    }
}