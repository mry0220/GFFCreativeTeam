using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SDrone_move : MonoBehaviour
{
    [Header("¶‚Ö‚Ì‘¬“x"),SerializeField] private float _horizontalSpeed = 3f;   // ¶‚Ö‚ÌˆÚ“®‘¬“x
    [Header("¶‚Ö‚Ì‘¬“x"),SerializeField] private float _verticalAmplitude = 5f; // ã‰º‚ÌU‚ê•
    [SerializeField] private float _verticalSpeed = 3f; // ã‰º‚Ì‘¬‚³

    private Rigidbody _rb;
    private float _startY;
    private float _timeOffset;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _startY = transform.position.y;
        _timeOffset = Random.Range(0f, Mathf.PI * 2f);  // “G‚²‚Æ‚É“®‚«‚ğ‚¸‚ç‚·
    }

    private void FixedUpdate()
    {
        float newY = _startY + Mathf.Sin(Time.time * _verticalSpeed + _timeOffset) * _verticalAmplitude;
        Vector3 newVelocity = new Vector3(-_horizontalSpeed, (newY - transform.position.y) / Time.fixedDeltaTime, 0f);
        _rb.velocity = newVelocity;
    }
}
