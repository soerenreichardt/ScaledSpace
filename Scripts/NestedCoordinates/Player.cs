using UnityEngine;

namespace NestedCoordinates
{
    public class Player : MonoBehaviour
    {

        public FloatingOrigin floatingOrigin;
        public float speed = 0.1f;
        public float mouseSensitivity = 0.1f;
        private Vector2 currentRotation;

        // Update is called once per frame
        void Update()
        {
            var transformRotation = MouseLook();
            var positionDelta = Move(transformRotation);
            transform.rotation = transformRotation;

            // Update the floating origin position and set the player position to that computed value
            floatingOrigin.UpdatePosition(positionDelta, transformRotation);
            transform.position = floatingOrigin.transform.position;
        }

        private Vector3 Move(Quaternion transformRotation)
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            var jump = Input.GetAxisRaw("Jump");
            speed += Input.GetAxis("Mouse ScrollWheel");
            return transformRotation * new Vector3(horizontal, jump, vertical) * speed;
        }

        // implement mouse look that is always up right
        Quaternion MouseLook()
        {
            currentRotation.x += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -80f, 80f);
            return Quaternion.Euler(currentRotation.y,currentRotation.x,0);
        }
    }
}
