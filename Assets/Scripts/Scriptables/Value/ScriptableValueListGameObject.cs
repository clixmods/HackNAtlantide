using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Value/ListGameObject")]
public class ScriptableValueListGameObject : ScriptableValue<List<GameObject>>
{
    public override List<GameObject> Value { get => base.Value; set => base.Value = value; }

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
