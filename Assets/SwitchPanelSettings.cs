using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchPanelSettings : MonoBehaviour
{
    [SerializeField] List<Button> _panelButtons;
    [SerializeField] InputButtonScriptableObject _rightInput;
    [SerializeField] InputButtonScriptableObject _leftInput;
    int currentIndexPanel = 0;
    // Start is called before the first frame update
    private void OnEnable()
    {
        _rightInput.OnValueChanged += MoveRight;
        _rightInput.OnValueChanged += MoveLeft;
    }
    private void OnDisable()
    {
        _rightInput.OnValueChanged -= MoveRight;
        _rightInput.OnValueChanged -= MoveLeft;
    }
    void MoveRight(bool value)
    {
        currentIndexPanel++;
        _panelButtons[currentIndexPanel % _panelButtons.Count].onClick?.Invoke();
    }
    void MoveLeft(bool value)
    {
        currentIndexPanel--;
        _panelButtons[currentIndexPanel % _panelButtons.Count].onClick?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        /*if(InputManager.IsGamepad())
        {
            for (int i = 0; i < _panelButtons.Count; i++)
            {
                if (_panelButtons[i].navigation.mode == Navigation.Mode.Automatic)
                {
                    navigationActive.mode = Navigation.Mode.None;
                    _panelButtons[i].navigation = navigationActive;
                }
                    
            }
        }
        else
        {
            for (int i = 0; i < _panelButtons.Count; i++)
            {
                if (_panelButtons[i].navigation.mode == Navigation.Mode.Automatic)
                {
                    navigationActive.mode = Navigation.Mode.Automatic;
                    _panelButtons[i].navigation = navigationActive;
                }
            }
        }*/
    }

}
