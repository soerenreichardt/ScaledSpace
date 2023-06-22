using UnityEngine;

namespace LongCoordinates
{
    public class Player : SpaceObject
    {
        void Start()
        {
            // this.rigidbody = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            // rigidbody.MovePosition(rigidbody.position + Vector3.forward * (Time.fixedTime * 0.01f) );
            transform.position += new Vector3(0, 0, Time.deltaTime);
        }
    }
}