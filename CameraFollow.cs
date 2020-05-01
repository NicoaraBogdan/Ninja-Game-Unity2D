using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float xMin = 0f;
    private float xMax = 3.8f;
    private float yMin = 0.35f;
    private float yMax = 8.6f;

    private Transform player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(player.position.x, xMin, xMax),
                                         Mathf.Clamp(player.position.y, yMin, yMax),
                                         -1);
    }
}
