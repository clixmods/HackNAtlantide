using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scriptables.Value.Listener
{
    public class ScriptableValueListGameObjectListener : ScriptableValueListener<List<GameObject>>
    {
        public UnityEvent ListBecameEmpty;
        protected override void LaunchScriptableValueEvent(List<GameObject> value)
        {
            base.LaunchScriptableValueEvent(value);
            if (value == null || value.Count == 0)
            {
                ListBecameEmpty?.Invoke();
            }
        }
    }
}