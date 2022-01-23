using Assets.Scripts.Data;
using Assets.Scripts.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] Transform centerOfMass;
    [SerializeField] Transform axisPoint;
    [SerializeField] float rotateVelocity;
    [SerializeField] float angle;
    private Rigidbody _rigidbody;
    private float rotateTime;
    private float timer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        _rigidbody.centerOfMass = centerOfMass.localPosition;
        _rigidbody.angularVelocity = axisPoint.localPosition * rotateVelocity;
        rotateTime = angle * Mathf.Deg2Rad / rotateVelocity;
        timer = 0;
    }

    void Update()
    {

        if (GameHandler.Instance.IsGame && !GameHandler.Instance.IsPause)
        {
            timer += Time.deltaTime;
            _rigidbody.angularVelocity = axisPoint.localPosition * rotateVelocity;
            if (timer > rotateTime)
            {
                axisPoint.position *= -1;
                timer = 0f;
            }
        }
    }
}
