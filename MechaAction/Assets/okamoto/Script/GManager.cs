using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager Instance;
    [SerializeField] private CameraManager _mainCamera;

    void Awake()
    {
        if (Instance == null) Instance = this;   //ˆê‰
        else Destroy(gameObject);
    }

    // Clamp’l‚ğ•ÏX
    public void SetCameraBounds(Vector2 min, Vector2 max)
    {
        _mainCamera.minPos = min;
        _mainCamera.maxPos = max;
    }
}
