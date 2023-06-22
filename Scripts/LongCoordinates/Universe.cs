using UnityEngine;

namespace LongCoordinates
{
    public class Universe : MonoBehaviour
    {
        public CoordinateSystemsConfig coordinateSystemsConfig;
        public Player player;

        private CoordinateSystemManager coordinateSystemManager;

        void Start()
        {
            this.coordinateSystemManager = CoordinateSystemManager.FromConfig(coordinateSystemsConfig);
            this.coordinateSystemManager.WithPivotObject(player);

            var solarSystem = new GameObject("SolarSystem");
            solarSystem.transform.parent = transform;
            var solarSystemSpaceObject = solarSystem.AddComponent<SolarSystem>();
            var spaceTransform = new SpaceTransform { position = new Vector3L(0, 0, 1) };
            solarSystemSpaceObject.spaceTransform = spaceTransform;
            solarSystemSpaceObject.coordinateSystemLevel = 1;
            this.coordinateSystemManager.AddSpaceObject(solarSystemSpaceObject);
        }

        void Update()
        {
            this.coordinateSystemManager.Update();
        }
    }
}