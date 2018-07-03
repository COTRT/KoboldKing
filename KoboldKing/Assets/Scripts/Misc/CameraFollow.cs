using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    private GameObject _player;
    private Vector3 offset;

    // Use this for initialization
    private void Start()
    {
        offset = _player.transform.position - transform.position;
    }

    // Update is called once per frame after update
    void LateUpdate()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            transform.position = _player.transform.position + offset;
        }
    }
}

