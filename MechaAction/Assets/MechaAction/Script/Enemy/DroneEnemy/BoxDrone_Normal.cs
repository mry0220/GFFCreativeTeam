using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDrone_Normal : MonoBehaviour, IEnemy
{
    private enum EnemyState
    {
        Move,
        Damage
    }

    private EnemyState _state = EnemyState.Move;
    public bool CanMove => _state == EnemyState.Move;

    private Transform _player;
    private Rigidbody _rb;

    [SerializeField] EnemyAttackSO _enemyattackSO;
    private int _hitdamage;
    private int _hitknockback;
    private string _effectname;
    private string _audioname;

    private float _horizontalSpeed = 3f;   // ç∂Ç÷ÇÃà⁄ìÆë¨ìx
    private float _verticalAmplitude = 5f; // è„â∫ÇÃêUÇÍïù
    private float _verticalSpeed = 3f; // è„â∫ÇÃë¨Ç≥

    private int _clear;

    private float _PositionY;
    private float _timeOffset;
    private int _dir;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _clear = GManager.Instance.clear;
        var attackData = _enemyattackSO.GetEffect("BoxDrone_Normal");
        if (attackData != null)
        {
            _hitdamage = (int)(attackData.Hitdamage + (_clear * 10));
            _hitknockback = (int)(attackData.Hitknockback + (_clear * 2));
            _effectname = attackData.EffectName;
            _audioname = attackData.AudioName;
        }

        if (_rb.position.x < _player.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);//âE
            _dir = 1;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);//ç∂
            _dir = -1;
        }
        _timeOffset = Random.Range(0f, Mathf.PI * 2f);  // ìGÇ≤Ç∆Ç…ìÆÇ´ÇÇ∏ÇÁÇ∑
        _PositionY = transform.position.y;
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);

        if (!CanMove) return;
        float newY = _PositionY + Mathf.Sin(Time.time * _verticalSpeed + _timeOffset) * _verticalAmplitude;
        Vector3 newVelocity = new Vector3(_horizontalSpeed * _dir, (newY - transform.position.y) / Time.fixedDeltaTime, 0f);
        _rb.velocity = newVelocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var Interface = collision.gameObject.GetComponent<IPlayerDamage>();
            if (Interface != null)
            {
                Interface.TakeDamage(_hitdamage, _hitknockback, _dir, _effectname, _audioname);
            }
        }
    }

    #region îÌÉ_ÉÅèàóù
    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Move;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(1f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(2f));
        //anim
    }

    public void ElectStun(int dir, int knockback,float electtime)
    {
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(electtime));
    }
    #endregion

}
