using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    private float followSpeed = 1.5f;

    // Start is called before the first frame update
    private void Start()
    {
        transform.LookAt(transform.position);
    }

    // Update is called once per frame
    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, followSpeed * Time.deltaTime);
    }
}
