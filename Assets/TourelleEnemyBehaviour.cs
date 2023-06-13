using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TourelleEnemyBehaviour : MonoBehaviour
{
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _rangeOfDetection;
    [SerializeField] float _shootDelay;
    bool _isActivated;
    float _distanceWithPlayerSqr;
    bool _inRange;

    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _bulletSpawn;
    [SerializeField] float _height = 1.5f;
    [SerializeField] Animator _animator;
    int ActivateID = Animator.StringToHash("Activate");
    int DeactivateID = Animator.StringToHash("Deactivate");
    int ShootID = Animator.StringToHash("Shoot");
    public UnityEvent OnStartActivate;
    public UnityEvent OnEndActivate;
    public UnityEvent OnStartDeactivate;
    public UnityEvent OnEndDeactivate;
    public UnityEvent OnShoot;
    public UnityEvent OnCharging;
    private void Update()
    {
        _distanceWithPlayerSqr = (PlayerInstanceScriptableObject.Player.transform.position - transform.position).sqrMagnitude;
        _inRange = _distanceWithPlayerSqr < _rangeOfDetection * _rangeOfDetection && Mathf.Abs(PlayerInstanceScriptableObject.Player.transform.position.y - transform.position.y) <5;

        if (_inRange && !_isActivated)
        {
            StartCoroutine(Activate());
        }
    }

    IEnumerator Activate()
    {
        OnStartActivate?.Invoke();
        _animator.CrossFade(ActivateID, 0.1f);
        _isActivated = true;
        yield return new WaitForSeconds(1);
        StartCoroutine(AimPlayer());
        OnEndActivate?.Invoke();
    }
    IEnumerator Desactivate()
    {
        OnStartDeactivate?.Invoke();
        _animator.CrossFade(DeactivateID, 0.1f);
        yield return new WaitForSeconds(1);
        _isActivated = false;
        OnEndDeactivate?.Invoke();
    }
    IEnumerator AimPlayer()
    {
        float shootTime = 0;
        bool hasPlayedShootAnim = false;
        while(_inRange)
        {
            //Aim
            FacePlayer(PlayerInstanceScriptableObject.Player.transform.position);

            //Shoot
            if(!hasPlayedShootAnim)
            {
                _animator.CrossFade(ShootID, 0.01f);
                hasPlayedShootAnim = true;
                OnCharging?.Invoke();
            }
            shootTime += Time.deltaTime;
            if(shootTime > _shootDelay)
            {
                Shoot();
                shootTime = 0;
                hasPlayedShootAnim=false;
            }

            yield return null;
        }
        StartCoroutine(Desactivate());
    }
    void FacePlayer(Vector3 playerPos)
    {
        Vector3 playerFlatPos = new Vector3(playerPos.x, 0, playerPos.z);
        Vector3 flatPos = new Vector3(transform.position.x, 0, transform.position.z);
        Quaternion _targetRotation = Quaternion.LookRotation((playerFlatPos - flatPos), Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
    }
    void Shoot()
    {
        OnShoot?.Invoke();
        GameObject bullet = Instantiate(_bulletPrefab, _bulletSpawn.position, Quaternion.identity);
        Vector3 playerpos = PlayerInstanceScriptableObject.Player.transform.position;
        bullet.GetComponent<BulletBehaviour>().Direction = (transform.forward * Vector3.Distance(playerpos, transform.position) - (_bulletSpawn.position.y-playerpos.y)*Vector3.up).normalized;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _rangeOfDetection);
    }
}
