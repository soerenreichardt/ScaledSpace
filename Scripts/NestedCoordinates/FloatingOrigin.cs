using UnityEngine;

namespace NestedCoordinates
{
    public class FloatingOrigin : MonoBehaviour
    {
        private CoordinateSystem coordinateSystem;
        
        // Start is called before the first frame update
        void Start()
        {
            GameObject obj = gameObject;
            this.coordinateSystem = obj.GetComponentInParent<CoordinateSystem>();
            coordinateSystem.camera.gameObject.transform.parent = obj.transform;
        }

        public void UpdatePosition(Vector3 positionDelta)
        {
            transform.position += positionDelta;
            coordinateSystem.UpdateCameraPosition(false, positionDelta);
        }

        void Update()
        {
            var transformPosition = this.transform.position;
            if (transformPosition.magnitude >= 10.0f)
            {
                coordinateSystem.Shift(transformPosition);
                this.transform.position = Vector3.zero;
            }
        }
    }
}