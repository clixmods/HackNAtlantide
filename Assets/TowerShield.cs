using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerShield : MonoBehaviour
{
    private IDamageable _idamageable;
    [SerializeField] private List<Character> charactersToProtect;
    private GameObject[] fxTrailGameObjects;
    [SerializeField] private GameObject fxTrail;
    [SerializeField] private Transform pivotStartTrail;

    public UnityEvent OnStart;
    private void Start()
    {
        _idamageable = GetComponent<IDamageable>();
        fxTrailGameObjects = new GameObject[charactersToProtect.Count];
        for (int i = 0; i < charactersToProtect.Count; i++)
        {
            charactersToProtect[i].SetInvulnerability(true);
            //ennemie cancel focusable
            charactersToProtect[i].GetComponent<Focusable>().IsTargetable = false;

            var gameObjectFX = Instantiate(fxTrail);
            var component = gameObjectFX.AddComponent<InteractableTrailLinker>();
            fxTrailGameObjects[i] = gameObjectFX;
            // Destroy trail if the character die
            charactersToProtect[i].OnDeath += (sender, args) =>
            {
                // Prevent if the tower was disabled before
                if (gameObjectFX != null)
                {
                    Destroy(gameObjectFX);
                }
            };
            component.SetTransformsLink(pivotStartTrail, charactersToProtect[i].transform);
        }
        _idamageable.OnDeath += TowerDisable;
        OnStart?.Invoke();
    }

    private void TowerDisable(object sender, EventArgs e)
    {
        for (int i = 0; i < charactersToProtect.Count; i++)
        {
            Destroy(fxTrailGameObjects[i]);
            Character character = charactersToProtect[i];
            if (character != null)
            {
                character.SetInvulnerability(false);
            }
            //let the ennemie be focusable
            charactersToProtect[i].GetComponent<Focusable>().IsTargetable = true;
        }
    }
}
