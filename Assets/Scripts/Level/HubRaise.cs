using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubRaise : MonoBehaviour
{
    private Animation _animation;
    Transform playerParent;
    private PlayerMovement _playerMovement;
    private void Awake()
    {
        _animation = GetComponent<Animation>();
        this.enabled = false;
    }
    
    public void Raise(bool value)
    {
        if (value)
        {
            _playerMovement = FindObjectOfType<PlayerMovement>();
            playerParent = _playerMovement.transform.parent;
            _playerMovement.transform.parent = transform;
            // Do animation
            _animation.Play();
            this.enabled = true;
        }
      

    }
    private void Update()
    {
        if (!_animation.isPlaying)
        {
            _playerMovement.transform.parent = playerParent;
            this.enabled = false;
        }
    }
}
