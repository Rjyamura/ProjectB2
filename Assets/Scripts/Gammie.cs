﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gammie : MonoBehaviour
{
    [SerializeField] private bool cageFree = false;
    [SerializeField] private bool movetoPlayer = false;
    [SerializeField] private bool transformGammie = true;
    [SerializeField] public bool transformDrake = false;

    private Animator _anim;
    private SpriteRenderer _spriteR;
    private DialogTrigger _dt;
    private EventManager _eventManager;

    [SerializeField] private float speed;
    [SerializeField] private Transform target;
    [SerializeField] private Transform position;
    [SerializeField] private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _spriteR = GetComponent<SpriteRenderer>();
        _dt = GetComponent<DialogTrigger>();

        _eventManager = EventManager.Instance != null ? EventManager.Instance : FindObjectOfType<EventManager>();
        if (_eventManager == null)
            Debug.Log("Event Manager is NULL");

        _eventManager.FreeGammieSceneActive += StartGammieDialogue;
    }
 

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (_dt.activeOnEnter)
            _dt.SetActiveOnEnterFalse();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bird Cage") && cageFree == false)
        {
            cageFree = true;
            
            movetoPlayer = true;
        }
    }
    private void Movement()
    {
        if (movetoPlayer == true)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, target.position, step);
      
            if (transform.position.x > player.position.x)
            {
                //flips gammy left
                _spriteR.flipX = false;
              
            } else if (transform.position.x < player.position.x)
            {
                //flips gammy right
                _spriteR.flipX = true;
            }
        }
    }


    public void TransformtoGammie()
    {
    
        StartCoroutine(toDrake());

    }
    
    public void TransformtoDrake()
    {
        StartCoroutine(toDrake());
    }
    IEnumerator toDrake()
    {
        //coroutine to test transform back to drake
        yield return new WaitForSeconds(1f);
        if (transformDrake == true)
        {
            _anim.SetTrigger("toDrake");
            transformDrake = false;
        }
        else
        {
            _anim.SetTrigger("toGammie");
            transformDrake = true;
        }
    }

    public void StartGammieDialogue() => _dt.TriggerDialog();

    private void OnDestroy() => _eventManager.FreeGammieSceneActive -= StartGammieDialogue;

}
