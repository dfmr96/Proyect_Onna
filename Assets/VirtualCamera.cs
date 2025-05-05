using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;
using UnityEngine;

public class VirtualCamera : MonoBehaviour
{
    private Transform playerTransform;
    private CinemachineVirtualCamera cam;
    private void Start()
    {
        playerTransform = PlayerHelper.GetPlayer().transform;
        cam = GetComponent<CinemachineVirtualCamera>();
        cam.Follow = playerTransform;
    }
}
