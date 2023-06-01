using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{
    [SerializeField] Button _button;
    public void Click()
    {
        _button.onClick?.Invoke();
    }
}
