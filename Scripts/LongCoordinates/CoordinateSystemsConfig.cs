using System;
using UnityEngine;

namespace LongCoordinates
{
    [CreateAssetMenu(fileName = "CoordinateSystemsConfig", menuName = "ScriptableObjects/CoordinateSystemsConfig", order = 1)]
    public class CoordinateSystemsConfig : ScriptableObject
    {
        [Serializable]
        public struct CoordinateSystemData
        {
            public string name;
            public long scaleFactor;
        }

        public CoordinateSystemData[] coordinateSystems;
    }
}