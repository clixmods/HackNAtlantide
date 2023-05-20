using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShield : MonoBehaviour
{
    [SerializeField] private List<Character> charactersToProtect;
    [SerializeField] private GameObject fxTrail;
    [SerializeField] private Transform pivotStartTrail;
    private void Awake()
    {
        for (int i = 0; i < charactersToProtect.Count; i++)
        {
            charactersToProtect[i].SetInvulnerability(true);
            var gameObjectFX = Instantiate(fxTrail);
            var component = gameObjectFX.AddComponent<InteractableTrailLinker>();
            component.SetTransformsLink(pivotStartTrail, charactersToProtect[i].transform);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
