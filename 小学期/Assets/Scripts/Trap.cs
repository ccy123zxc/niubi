using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public float speed = 1.0f;       // 控制运动速度
    public float distance = 5.0f;    // 控制来回运动的总距离 (从起始点到最远点再回到起始点的距离)

    private Vector3 startPos;        // 记录物体的初始位置

    private void Awake()
    {
        startPos = transform.position;
    }



    private void Update()
    {

        float offset = Mathf.PingPong(Time.time * speed, distance);
        Vector3 newPosition = startPos + Vector3.right * offset;
        transform.position = newPosition;
    }


}
