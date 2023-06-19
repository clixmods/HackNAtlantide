using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserbehaviour : MonoBehaviour
{
    [SerializeField] Transform originPosition;
    [SerializeField] LayerMask RaycastDetectable;
    [SerializeField] GameObject particuleLaser;
    [SerializeField] Transform parent;
    [SerializeField] GameObject burntLine;
    GameObject currentHitObject;

    private void Awake()
    {
        //burntLine.transform.parent = null;
        parent = transform.parent;
    }
    private void OnEnable()
    {
        /*positionsLine = new();
        burntLine.positionCount = 0;*/
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        if(Physics.Raycast(originPosition.position,transform.up,out hit, 100f, RaycastDetectable, QueryTriggerInteraction.Ignore))
        {
            transform.parent = null;
            transform.localScale = new Vector3(transform.localScale.x, (originPosition.position - hit.point).magnitude/2f, transform.localScale.z);
            transform.position = originPosition.position + transform.up * transform.localScale.y;

            //Effect
            if (hit.collider.gameObject != currentHitObject && hit.collider.gameObject.layer!=6)
            {
                currentHitObject = hit.collider.gameObject;
                burntLine = Instantiate(burntLine, hit.point, Quaternion.identity);
            }
            particuleLaser.transform.position = hit.point;
            burntLine.transform.position = hit.point;



            transform.parent = parent;
            


            //positionsLine.Add(hit.point);
            //if(hit.collider.gameObject.layer != 6)
            /*burntLine.positionCount += 1;
            
            burntLine.SetPositions(positionsLine.ToArray());*/
        }
        
    }
}
