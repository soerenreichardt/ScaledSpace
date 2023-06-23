using System.Collections.Generic;
using UnityEngine;

namespace NestedCoordinates
{
    public class CoordinateSystem : MonoBehaviour
    {

        public List<GameObject> gameObjects;
        public CoordinateSystem parentCoordinateSystem;
        public new Camera camera;
        public float scaleFactor = 100.0f;
        private bool hasParent;
        private float inverseScaleFactor;
        
        // Start is called before the first frame update
        void Start()
        {
            this.hasParent = parentCoordinateSystem != null;
            this.inverseScaleFactor = 1.0f / scaleFactor;
        }

        public void UpdateParentCoordinateSystemCameraPositions(Vector3 positionDelta, Quaternion transformRotation)
        {
            if (hasParent)
            {
                var scaledPositionDelta = positionDelta * inverseScaleFactor;
                var cameraTransform = parentCoordinateSystem.camera.transform;
                cameraTransform.position += scaledPositionDelta;
                cameraTransform.rotation = transformRotation;

                parentCoordinateSystem.UpdateParentCoordinateSystemCameraPositions(scaledPositionDelta, transformRotation);
            }
        }
        
        public void Shift(Vector3 offset)
        {
            var remove = new List<GameObject>();
            foreach (var obj in gameObjects)
            {
                obj.transform.position -= offset;
                if (hasParent && obj.transform.position.magnitude >= scaleFactor)
                {
                    parentCoordinateSystem.TransformFromLowerCoordinateSystem(obj);
                    remove.Add(obj);
                }
            }

            foreach (var toRemove in remove)
            {
                gameObjects.Remove(toRemove);
            }
        }

        private void TransformFromLowerCoordinateSystem(GameObject spaceGameObject)
        {
            gameObjects.Add(spaceGameObject);
            spaceGameObject.transform.localScale *= inverseScaleFactor;
            spaceGameObject.transform.position = spaceGameObject.transform.position * inverseScaleFactor + camera.transform.position;
            
            var obj = gameObject;
            spaceGameObject.transform.parent = obj.transform;
            spaceGameObject.layer = LayerMask.NameToLayer(obj.name);
        }
    }
}
