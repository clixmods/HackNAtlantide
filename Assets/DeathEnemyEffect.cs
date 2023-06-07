using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemyEffect : MonoBehaviour
{
    [SerializeField] GameObject _meshDestroy;
    [SerializeField] GameObject _particuleDead;
    public void DeadEffect()
    {
            _particuleDead.transform.parent = null;
            _meshDestroy.transform.parent = null;
            _meshDestroy.transform.position = transform.position;
            _meshDestroy.transform.rotation = transform.rotation;
            _meshDestroy.SetActive(true);
            Rigidbody[] childrb = _meshDestroy.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in childrb)
            {
                rb.AddForce(Random.onUnitSphere * 5f, ForceMode.Impulse);
            }
    }
}
