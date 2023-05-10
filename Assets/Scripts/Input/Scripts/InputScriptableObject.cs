using _2DGame.Scripts.Save;
using System;
using UnityEngine;


    public abstract class InputScriptableObject<T> : ScriptableObjectSaveable
    {
        public event Action<T> OnValueChanged;
        [SerializeField] bool isActive = true;
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        public void ChangeValue(T value)
        {
            if(isActive)
            {
                OnValueChanged?.Invoke(value);
            }
        }
        public override void OnLoad(string data)
        {
            //TODO
            throw new NotImplementedException();
        }

        public override void OnSave(out SaveData saveData)
        {
            //TODO
            throw new NotImplementedException();
        }
        public override void OnReset()
        {
            //TODO
            IsActive = true;
        }
}
