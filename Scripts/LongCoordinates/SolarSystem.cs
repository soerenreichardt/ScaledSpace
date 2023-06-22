using UnityEngine;

namespace LongCoordinates
{
    public class SolarSystem : SpaceObject
    {
        void Start()
        {
            var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.parent = transform;
        }

        private void Update()
        {
            transform.position += new Vector3(0, 0, Time.deltaTime);
        }
    }
}