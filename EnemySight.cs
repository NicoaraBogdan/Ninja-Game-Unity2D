﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField]
    private Enemy enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            enemy.Target = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        if (collision.name == "Player")
        {
            enemy.Target = null;
        }
    }
}
