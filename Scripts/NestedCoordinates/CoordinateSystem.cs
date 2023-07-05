using System.Collections.Generic;
using UnityEngine;

namespace NestedCoordinates
{
    public class CoordinateSystem : MonoBehaviour
    {

        public delegate void ShiftObjectToLowerCoordinateSystemHandler();
        public event ShiftObjectToLowerCoordinateSystemHandler OnObjectShiftToLowerCoordinateSystem;
        
        public List<SpaceObject> spaceObjects;
        public CoordinateSystem parentCoordinateSystem;
        public CoordinateSystem childCoordinateSystem;
        public new Camera camera;
        public FloatingOrigin cameraFloatingOrigin;
        public float scaleFactor = 100.0f;
        public float inverseScaleFactor;
        public bool hasParent;
        public bool hasChild;

        // Start is called before the first frame update
        void Start()
        {
            this.hasParent = parentCoordinateSystem != null;
            this.hasChild = childCoordinateSystem != null;
            this.inverseScaleFactor = 1.0f / scaleFactor;
            this.cameraFloatingOrigin = camera.GetComponent<FloatingOrigin>();
        }

        public void UpdateParentCoordinateSystemCameraPositions(Vector3 positionDelta, Quaternion transformRotation)
        {
            if (hasParent)
            {
                var scaledPositionDelta = positionDelta * inverseScaleFactor;
                parentCoordinateSystem.cameraFloatingOrigin.UpdatePosition(scaledPositionDelta, transformRotation);
                parentCoordinateSystem.UpdateParentCoordinateSystemCameraPositions(scaledPositionDelta, transformRotation);
            }
        }
        
        public void Shift(Vector3 offset)
        {
            var remove = new List<SpaceObject>();
            foreach (var obj in spaceObjects)
            {
                // calculate new position
                var transformCache = obj.transform;
                var position = transformCache.position;
                position -= offset;
                
                transformCache.position = position;
                
                // check if object needs to be transferred to parent or child coordinate system
                var distanceToObject = position.magnitude;
                var threshold = obj.LODThresholdForCoordinateSystem(this);
                if (hasParent && distanceToObject >= threshold)
                {
                    parentCoordinateSystem.TransformFromChildCoordinateSystem(obj);
                    remove.Add(obj);
                }
            }

            foreach (var toRemove in remove)
            {
                spaceObjects.Remove(toRemove);
            }
        }

        public bool IsEmpty()
        {
            return spaceObjects.Count == 0;
        }

        void Update()
        {
            var remove = new List<SpaceObject>();
            foreach (var obj in spaceObjects)
            {
                var distanceToCamera = Vector3.Distance(obj.transform.position, camera.transform.position);
                var threshold = obj.LODThresholdForCoordinateSystem(this);
                if (hasChild && distanceToCamera < threshold)
                {
                    childCoordinateSystem.TransformFromParentCoordinateSystem(obj);
                    remove.Add(obj);
                }
            }
            
            foreach (var toRemove in remove)
            {
                spaceObjects.Remove(toRemove);
            }
        }

        private void TransformFromChildCoordinateSystem(SpaceObject spaceGameObject)
        {
            spaceObjects.Add(spaceGameObject);
            var transformCache = spaceGameObject.transform;
            transformCache.localScale *= inverseScaleFactor;
            spaceGameObject.size *= inverseScaleFactor;
            transformCache.position = transformCache.position * inverseScaleFactor + camera.transform.position;
            
            var obj = gameObject;
            transformCache.parent = obj.transform;
            spaceGameObject.gameObject.layer = LayerMask.NameToLayer(obj.name);
        }

        private void TransformFromParentCoordinateSystem(SpaceObject spaceGameObject)
        {
            spaceObjects.Add(spaceGameObject);
            var transformCache = spaceGameObject.transform;
            transformCache.localScale *= scaleFactor;
            transformCache.position = (transformCache.position - parentCoordinateSystem.camera.transform.position) * scaleFactor + camera.transform.position;
            spaceGameObject.size *= scaleFactor;

            var obj = gameObject;
            transformCache.parent = obj.transform;
            spaceGameObject.gameObject.layer = LayerMask.NameToLayer(obj.name);
            OnObjectShiftToLowerCoordinateSystem?.Invoke();
        }
    }
}
