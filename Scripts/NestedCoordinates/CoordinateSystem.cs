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

        private float cameraNearClipPlane;
        private float cameraFarClipPlane;

        // Start is called before the first frame update
        void Start()
        {
            this.hasParent = parentCoordinateSystem != null;
            this.hasChild = childCoordinateSystem != null;
            this.inverseScaleFactor = 1.0f / scaleFactor;
            this.cameraFloatingOrigin = camera.GetComponent<FloatingOrigin>();

            this.cameraNearClipPlane = camera.nearClipPlane;
            this.cameraFarClipPlane = camera.farClipPlane;
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
            foreach (var obj in spaceObjects)
            {
                // calculate new position
                var transformCache = obj.transform;
                var position = transformCache.position;
                position -= offset;
                
                transformCache.position = position;
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
                if (hasParent && distanceToCamera >= cameraFarClipPlane - obj.lodThreshold)
                {
                    parentCoordinateSystem.TransformFromChildCoordinateSystem(obj);
                    remove.Add(obj);
                } 
                if (hasChild && distanceToCamera < cameraNearClipPlane + cameraNearClipPlane + (obj.lodThreshold / 2.0f))
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
            transformCache.position = (transformCache.position - childCoordinateSystem.camera.transform.position) * inverseScaleFactor + camera.transform.position;
            spaceGameObject.lodThreshold *= inverseScaleFactor;

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
            spaceGameObject.lodThreshold *= scaleFactor;

            var obj = gameObject;
            transformCache.parent = obj.transform;
            spaceGameObject.gameObject.layer = LayerMask.NameToLayer(obj.name);
            OnObjectShiftToLowerCoordinateSystem?.Invoke();
        }
    }
}
