using Plugins.Fog;

namespace Fog
{
    public class AttachFogTransitionOnTrigger : FogTransition
    {
        private Trigger _zoneVolume;
        private void Start()
        {
            _zoneVolume = GetComponent<TriggerBox>();
            _zoneVolume.EventOnTriggerEnter.AddListener(ApplyFog);
        }
    }
}