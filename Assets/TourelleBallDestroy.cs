using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourelleBallDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.parent.TryGetComponent<BulletBehaviour>(out BulletBehaviour bullet))
        {
            bullet.Destroy();
        }
    }
}
