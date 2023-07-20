using UnityEngine;

namespace NestedCoordinates
{
    public class Player : MonoBehaviour
    {

        public FloatingOrigin floatingOrigin;
        public float speed = 0.1f;
        public float mouseSensitivity = 0.1f;
        private Vector2 currentRotation;
        private CoordinateSystem coordinateSystem;

        void Start()
        {
            this.coordinateSystem = floatingOrigin.coordinateSystem;
        }

        // Update is called once per frame
        void Update()
        {
            var transformRotation = MouseLook();
            var positionDelta = Move(transformRotation);
            transform.rotation = transformRotation;

            // Update the floating origin position and set the player position to that computed value
            floatingOrigin.UpdatePosition(positionDelta, transformRotation);
            transform.position = floatingOrigin.transform.position;

            if (coordinateSystem.IsEmpty())
            {
                ShiftToParentCoordinateSystem();
            }
        }

        private Vector3 Move(Quaternion transformRotation)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var jump = Input.GetAxisRaw("Jump");
            speed += speed * Input.GetAxis("Mouse ScrollWheel");
            return transformRotation * new Vector3(horizontal, jump, vertical) * speed * Time.deltaTime;
        }

        private Quaternion MouseLook()
        {
            currentRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -80f, 80f);
            return Quaternion.Euler(currentRotation.y,currentRotation.x,0);
        }

        private void ShiftToParentCoordinateSystem()
        {
            if (coordinateSystem.hasParent)
            {
                coordinateSystem.OnObjectShiftToLowerCoordinateSystem += ShiftToChildCoordinateSystem;

                var parentCoordinateSystem = coordinateSystem.parentCoordinateSystem;
                coordinateSystem = parentCoordinateSystem;
                speed *= parentCoordinateSystem.inverseScaleFactor;
                transform.parent = parentCoordinateSystem.transform;
                floatingOrigin = parentCoordinateSystem.cameraFloatingOrigin;
            }
        }

        private void ShiftToChildCoordinateSystem()
        {
            if (coordinateSystem.hasChild)
            {
                coordinateSystem.OnObjectShiftToLowerCoordinateSystem -= ShiftToChildCoordinateSystem;
                
                var childCoordinateSystem = coordinateSystem.childCoordinateSystem;
                coordinateSystem = childCoordinateSystem;
                speed *= childCoordinateSystem.scaleFactor;
                transform.parent = childCoordinateSystem.transform;
                floatingOrigin = childCoordinateSystem.cameraFloatingOrigin;
            }
        }
    }
}
