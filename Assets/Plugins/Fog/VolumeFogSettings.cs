using UnityEngine;

namespace Plugins.Fog
{
    [CreateAssetMenu(menuName = "Fog/Volume Setting Fog" , fileName = "VolumeFogSettings_", order = 0)]
    public class VolumeFogSettings : ScriptableObject
    {
        public bool fog = true;
        public Color fogColor = Color.white;
        public FogMode fogMode = FogMode.Exponential;
        public float fogDensity = 0.005f;
        public float fogEndDistance = 150f;
        public float fogStartDistance = 30f;
        public float timeTransition = 2;
    
    }
}