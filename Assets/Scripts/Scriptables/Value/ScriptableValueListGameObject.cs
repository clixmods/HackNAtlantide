using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/ListGameObject")]
public class ScriptableValueListGameObject : ScriptableValue<List<GameObject>>
{
    public override List<GameObject> Value { get { return base.Value; } set => base.Value = value; }

    public void AddUnique(GameObject gameObject)
    {
        //securité
        if (base.Value == null) 
        { base.Value = new(); }

        Value.Add(gameObject);
        ApplyGameStateCombat();
    }
    public void RemoveUnique(GameObject gameObject)
    {
        //securité
        if (base.Value == null)
        { base.Value = new(); }

        if (Value.Contains(gameObject))
        {
            Value.Remove(gameObject);
        }
        ApplyGameStateCombat();
    }
    public void ResetList()
    {
        Debug.Log("reset");
        Value.RemoveRange(0, Value.Count);
    }
    //TODO A changer de place ou renommmer la classe ?
    void ApplyGameStateCombat()
    {
        GameStateManager.Instance.combatStateObject.SetActive(Value.Count > 0);
    }
}
