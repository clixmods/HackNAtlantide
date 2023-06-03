using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableHealth : Character
{
    public UnityEvent OnDead;
    [SerializeField] float _respawnTimeAfterDeadByAttack;
    Vector3 _initialPos;
    [SerializeField] GameObject potInteractablePrefab;
    [SerializeField] GameObject _meshDestroy;
    // Mesh destroy
    private Rigidbody[] _rigidbodiesFromMeshDestroy;
    public override void Awake()
    {
        base.Awake();
        _initialPos = transform.position;
        if (_meshDestroy != null)
        {
            _rigidbodiesFromMeshDestroy = _meshDestroy.GetComponentsInChildren<Rigidbody>();
        }
    }
    public override void Dead()
    {
        base.Dead();
        OnDead?.Invoke();
        /*if (_meshDestroy != null)
        {
            _meshDestroy.transform.position = transform.position;
            _meshDestroy.transform.rotation = transform.rotation;
            _meshDestroy.SetActive(true);

            foreach (Rigidbody rb in _rigidbodiesFromMeshDestroy)
            {
                rb.AddForce(Random.onUnitSphere * 5f, ForceMode.Impulse);
            }
        }
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<BoxCollider>());
        GetComponent<Rigidbody>().isKinematic = true;
        StartCoroutine(Respawn());*/
    }
    /*IEnumerator Respawn()
    {
        Debug.Log("respawn");
        yield return new WaitForSeconds(_respawnTimeAfterDeadByAttack);
        Instantiate(potInteractablePrefab,_initialPos, Quaternion.identity);
    }*/
}
