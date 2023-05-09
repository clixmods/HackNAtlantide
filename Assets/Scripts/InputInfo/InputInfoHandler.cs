using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputInfoHandler : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private Dictionary<InputInfoScriptableObject, GameObject> _dictionary = new Dictionary<InputInfoScriptableObject, GameObject>();
    private void Awake()
    {
        InputInfoScriptableObject.InputInfoInit += InputInfoInit;
        InputInfoScriptableObject.InputInfoSend += InputInfoSend;
        InputInfoScriptableObject.InputInfoRemove += InputInfoRemove;
    }

    private void InputInfoInit(InputInfoScriptableObject obj)
    {
        var newGameObject = Instantiate(prefab, transform);
        newGameObject.GetComponent<UIInputInfo>().Send(obj);
        _dictionary.Add(obj, newGameObject);
    }

    private void InputInfoRemove(InputInfoScriptableObject obj)
    {
        if(_dictionary.TryGetValue(obj, out GameObject value))
            value.SetActive(false);
        else
        {
            InputInfoInit(obj);
            InputInfoRemove(obj);
        }
    }

    private void InputInfoSend(InputInfoScriptableObject obj)
    {
        if(_dictionary.TryGetValue(obj, out GameObject value))
            _dictionary[obj].SetActive(true);
        else
        {
            InputInfoInit(obj);
            InputInfoSend(obj);
        }
    }

    private void OnDestroy()
    {
        InputInfoScriptableObject.InputInfoInit -= InputInfoInit; 
        InputInfoScriptableObject.InputInfoSend -= InputInfoSend;
        InputInfoScriptableObject.InputInfoRemove -= InputInfoRemove;
    }
}
