using System;
using System.Collections;
using UnityEngine;

namespace Loading
{
    public abstract class LoaderBehaviour : MonoBehaviour
    {
        public static Action<LoaderBehaviour,Action, IEnumerator> LoaderStart;
        public static Action LoaderEnd;
        public void StartLoader()
        {
            LoaderStart?.Invoke(this,  LoaderAction, LoaderRoutine() );
        }

        protected abstract void LoaderAction();

        protected abstract IEnumerator LoaderRoutine();
        
    }
}