using Assets.Scripts.Data;
using Assets.Scripts.Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] float high = 5; 
    private Rigidbody _rigidbody;
    private float timer, periodTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        _rigidbody.velocity = -transform.forward * velocity;
        periodTime = high / velocity;
        timer = 0;
    }

    void Update()
    {
        if (GameHandler.Instance.IsGameProcess)
        {
            timer += Time.deltaTime;
            if (timer > periodTime)
            {
                _rigidbody.velocity *= -1;
                timer = 0f;
            }
        }
        else
            _rigidbody.Sleep();
    }
}
