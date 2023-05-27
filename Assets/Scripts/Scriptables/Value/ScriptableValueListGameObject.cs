using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/ListGameObject")]
public class ScriptableValueListGameObject : ScriptableValue<List<GameObject>>
{
    public override List<GameObject> Value { get => base.Value; set => base.Value = value; }
    public float count;
    public void AddUnique(GameObject gameObject)
    {
        /*bool isInList = false;
        for(int i = 0; i < Value.Count; i++)
        {
            if (Value[i] != gameObject)
            {
                isInList = true;
            }
        }
        if(!isInList)
        {
            Value.Add(gameObject);
        }*/
        Value.Add(gameObject);
        count = Value.Count;
        ApplyGameStateCombat();
    }
    public void RemoveUnique(GameObject gameObject)
    {
        /*for (int i = 0; i < Value.Count; i++)
        {
            if (Value[i] == gameObject)
            {
                Value.RemoveAt(i);
                return;
            }
        }*/
        Value.Remove(gameObject);
        count = Value.Count;
        ApplyGameStateCombat();
    }
    //TODO A changer de place ou renommmer la classe ?
    void ApplyGameStateCombat()
    {
        GameStateManager.Instance.combatStateObject.SetActive(Value.Count > 0);
    }
}
