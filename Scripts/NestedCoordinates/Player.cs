using UnityEngine;

namespace NestedCoordinates
{
    public class Player : MonoBehaviour
    {

        public FloatingOrigin floatingOrigin;

        // Update is called once per frame
        void Update()
        {
            var positionDelta = Vector3.back * 0.4f;
            floatingOrigin.UpdatePosition(positionDelta);
        }
    }
}
