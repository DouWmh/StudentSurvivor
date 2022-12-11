using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {

    }



    private void Update()
    {
        transform.position = target.position + offset;
    }
}

