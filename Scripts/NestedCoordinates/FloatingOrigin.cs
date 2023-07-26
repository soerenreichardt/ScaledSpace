using UnityEngine;

namespace NestedCoordinates
{
    public class FloatingOrigin : MonoBehaviour
    {
        internal CoordinateSystem coordinateSystem;
        
        // Start is called before the first frame update
        void Start()
        {
            GameObject obj = gameObject;
            this.coordinateSystem = obj.GetComponentInParent<CoordinateSystem>();
        }

        public void UpdatePosition(Vector3 positionDelta, Quaternion transformRotation)
        {
            var transformCache = transform;
            transformCache.position += positionDelta;
            transformCache.rotation = transformRotation;
            coordinateSystem.UpdateParentCoordinateSystemCameraPositions(positionDelta, transformRotation);
        }

        void Update()
        {
            var transformPosition = this.transform.position;
            if (transformPosition.magnitude >= 1000.0f)
            {
                coordinateSystem.Shift(transformPosition);
                this.transform.position = Vector3.zero;
            }
        }
    }
}