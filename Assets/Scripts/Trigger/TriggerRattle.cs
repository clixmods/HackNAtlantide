using AudioAliase;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class TriggerRattle : Trigger
{
    [Header("Settings Alias")]
    [SerializeField]
    [Tooltip("The alias ID for the rattling sound")]
    private Alias aliasRattle;

    [SerializeField] private bool playAliasOnTriggerEnter = true;
    [SerializeField] private bool playAliasOnTriggerStay;
    [SerializeField] private bool playAliasOnTriggerExit;
    /// <summary>
    /// Method that is called when the script is validated in the Unity editor.
    /// </summary>
    protected override void OnValidate()
    {
        base.OnValidate();
        #if UNITY_EDITOR
        if (aliasRattle != null)
        {
            gameObject.name = $"Rattle Sound : {aliasRattle.aliasName}";
        }
        #endif
    }
    protected override void TriggerEnter(Collider other)
    {
        base.TriggerEnter(other);
        if(playAliasOnTriggerEnter)
            transform.PlaySoundAtPosition(aliasRattle);
    }

    protected override void TriggerStay(Collider other)
    {
        base.TriggerStay(other);
        if(playAliasOnTriggerStay)
            transform.PlaySoundAtPosition(aliasRattle);
    }

    protected override void TriggerExit(Collider other)
    {
        base.TriggerExit(other);
        if(playAliasOnTriggerExit)
            transform.PlaySoundAtPosition(aliasRattle);
    }
}
