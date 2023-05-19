using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemyEffect : MonoBehaviour
{
    [SerializeField] GameObject _meshDestroy;
    public void DeadEffect()
    {
        Debug.Log("yoo");
            _meshDestroy.transform.parent = null;
            _meshDestroy.transform.position = transform.position;
            _meshDestroy.transform.rotation = transform.rotation;
            _meshDestroy.SetActive(true);
    }
}
