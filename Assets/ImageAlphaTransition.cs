using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ImageAlphaTransition : LoadingWorkerBehaviour
{
    enum StateTransition
    {
        IsEnabling,
        IsDisabling
    }
    private Image _image;
    [SerializeField] private float timeToActiveEmissive = 1f;
    [SerializeField] private float timeToResetEmissive = 0.5f;
    private StateTransition _stateTransition = StateTransition.IsDisabling;
    public override bool WorkIsDone { get; set; }

    private void Awake()
    {
        base.Awake();
        _image = GetComponentInChildren<Image>();
    }
    public void ActiveBlackScreen()
    {
        StartCoroutine(LerpColor( Color.black, timeToActiveEmissive, StateTransition.IsEnabling ));
    }

    public void RemoveBlackScreen()
    {
        StartCoroutine(LerpColor(Color.clear, timeToResetEmissive, StateTransition.IsDisabling));
    }
    
    IEnumerator LerpColor( Color colorTarget, float timeTransition, StateTransition stateTransition)
    {
        WorkIsDone = false;
        yield return new WaitForSecondsRealtime(1);
        _stateTransition = stateTransition;
        float timeElapsed = 0;
        // Assign our new value.
        Color initialColor = _image.color;  

        while (timeElapsed < timeTransition)
        {
            if (_stateTransition != stateTransition)
            {
                yield break;
            }
            timeElapsed += Time.unscaledDeltaTime;
            var t = timeElapsed / timeTransition;
            // Assign our new value.
            _image.color = Color.Lerp(initialColor, colorTarget, t);
            yield return null;
        }
        WorkIsDone = true;
      
    }
}
