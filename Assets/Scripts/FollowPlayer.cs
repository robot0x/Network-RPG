﻿using UnityEngine;
using UnityEngine.Networking;

public class FollowPlayer : MonoBehaviour
{
    [Header("镜头和玩家的位置偏移")]
    public Vector3 offsetPosition;
    [Header("运行时赋值的玩家组件")]
    public Transform player;

    [Header("拉近最小值")]
    public float minDistance = 2.0f;
    [Header("拉远最大值")]
    public float maxDistance = 10.0f;
    [Header("向下最小值")]
    public float minAngle = -20.0f;
    [Header("向上最大值")]
    public float maxAngle = 40.0f;
    [Header("放缩速度")]
    public float scrollSpeed = 2;
    [Header("旋转速度")]
    public float rotateSpeed = 2;

    void Update()
    {
        if (player == null) { return; }

        transform.position = offsetPosition + player.position;

        ScrollView();
        RotateView();
    }

    // 控制镜头的拉近拉远
    void ScrollView()
    {
        /*
            Vector.magnitude
                返回向量的长度，向量的长度是(x*x+y*y+z*z)的平方根。
            Input.GetAxis("Mouse ScrollWheel")
                鼠标向后滑动返回负数（拉近视野），向前滑动正数（拉远视野）。
            Mathf.Clamp(float value, float min, float max)
                限制value的值在min和max之间，如果value小于min，返回min。如果value大于max，返回max。否则返回value。
        */
        float distance = offsetPosition.magnitude;
        distance += Input.GetAxis("Mouse ScrollWheel") * -scrollSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // 改变位置偏移
        offsetPosition = offsetPosition.normalized * distance;
    }

    // 控制镜头的左右上下
    void RotateView()
    {
        /*
            Input.GetMouseButton(1)
                得到鼠标右键的按下。
            Input.GetAxis("Mouse X")
                得到鼠标水平方向的滑动。
            Input.GetAxis("Mouse Y")
                得到鼠标垂直方向的滑动。
            Transform.RotateAround(Vector3 point, Vector3 axis, float angle)
                一个物体围绕point位置axis轴旋转angle角度。
        */
        if (Input.GetMouseButton(1))
        {
            // 左右
            transform.RotateAround(player.position, player.up, rotateSpeed * Input.GetAxis("Mouse X"));
            // 记录镜头位置
            Vector3 originalPos = transform.position;
            // 记录镜头旋转
            Quaternion originalRotation = transform.rotation;

            // 上下 (会影响到的属性一个是Position，一个是Rotation)
            transform.RotateAround(player.position, transform.right, -rotateSpeed * Input.GetAxis("Mouse Y"));
            float x = transform.eulerAngles.x;
            // 限制上下范围
            if (maxAngle < x && x < 360 + minAngle)
            {
                transform.position = originalPos;
                transform.rotation = originalRotation;
            }

            // 改变位置偏移
            offsetPosition = transform.position - player.position;
        }
    }
}
