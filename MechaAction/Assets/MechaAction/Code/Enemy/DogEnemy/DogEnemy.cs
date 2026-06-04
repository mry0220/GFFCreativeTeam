using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogEnemy : MonoBehaviour
{
    private enum EnemyState
    {
        Look,          //�T��
        Move,          //�ǐ�
        Wait,          //���˗p��(����Ȃ�)
        Attack,         //����
        Damage
    }

    private EnemyState _state = EnemyState.Look;

    private Transform _player;
    private Rigidbody _rb;
    private Dog_Attack _attack;

    public int Dir => _dir;
    private int _dir = -1;
    private float _jumpPower = 7f;
    private bool _moveStop = false;
    private bool _iswait;//wait�R���[�`���̏d����h��
    private bool _ismove;//move�R���[�`���̏d����h��
    private bool _isattack;//attack�R���[�`���̏d����h��

    private float _fallTime;
    Vector3 origin;
    private bool _isGrounded;

    Vector3 velocity;
    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<Dog_Attack>();
    }

    private void Update()
    {
        if (_dir == 1)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (_dir == -1)
        {
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }

        RaycastHit hit;
        origin = transform.position + Vector3.down;
        _isGrounded = Physics.SphereCast(origin, 0.4f, Vector3.down, out hit, 1f, LayerMask.GetMask("Grounded"));
        //Debug.Log(_isGrounded);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, 0.4f);
        Gizmos.DrawWireSphere(origin + Vector3.down * 1f, 0.4f);
    }

    private void FixedUpdate()
    {
        velocity = _rb.linearVelocity;

        switch (_state)
        {
            case EnemyState.Look:
                Look();
                if (Vector3.Distance(transform.position, _player.position) < 13f)
                {
                    Debug.Log("����");
                    _state = EnemyState.Wait;
                }
                break;

            case EnemyState.Move:
                if (Vector3.Distance(transform.position, _player.position) > 17f)
                {
                    _state = EnemyState.Look;
                }
                Direction();
                if(!_ismove)//�R���[�`���̏d���h��
                    StartCoroutine(Move());
                break;

            case EnemyState.Wait:
                Direction();
                if(!_iswait)//�R���[�`���̏d���h��
                    StartCoroutine(Wait());//���΂炭�҂���Move��
                break;

            case EnemyState.Attack:
                if(!_isattack)//�R���[�`���̏d���h��
                    StartCoroutine(Attack());
                _state = EnemyState.Wait;
                break;
        }

        if (!_isGrounded)
        {
            _Gravity();
        }
        else
        {
            _fallTime = 0;
        }
        _rb.linearVelocity = velocity;
    }

    private void Look()
    {
        //anim
    }

    private void Direction()
    {
        if(!_isGrounded)
        {
            return;
        }

        if (_moveStop)
        {
            velocity.x = 0f;
            _rb.linearVelocity = velocity;
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            if (_dir == -1)
            {
                Debug.Log("right");
                StartCoroutine(Waitturn(1));
            }
            _dir = 1;
        }
        else
        {
            if (_dir == 1)
            {
                Debug.Log("left");
                StartCoroutine(Waitturn(-1));
            }
            
            _dir = -1;
        }

    }


    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);
        _dir = _newdirection;
        _moveStop = false;

        yield break;
    }

    private IEnumerator Move()
    {
        _ismove = true;
        //int _rand = Random.Range(1, 4);
        if (Vector3.Distance(transform.position, _player.position) > 7f)
        {
            Debug.Log("frontjump");
            _rb.AddForce(_dir * 13f, _jumpPower, 0f, ForceMode.Impulse);

            yield return new WaitForSeconds(0.5f);

            
            Debug.Log("�҂�");
            yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                                       //�㏑������new Vector3���g���Ȃ���(AI�Q��)
            _rb.linearVelocity = Vector3.zero;
            _state = EnemyState.Attack;
            
            
        }
        else if(Vector3.Distance(transform.position, _player.position) <= 7f &&
            Vector3.Distance(transform.position, _player.position) > 3f)
        {
            _state = EnemyState.Attack;
        }
        else
        {
            Debug.Log("backjump");
            _rb.AddForce(_dir * -9f, _jumpPower, 0f, ForceMode.Impulse);

            yield return new WaitForSeconds(0.4f);

            Debug.Log("�҂�");
            yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                                       //�㏑������new Vector3���g���Ȃ���(AI�Q��)
            _rb.linearVelocity = Vector3.zero;
            _state = EnemyState.Attack;
        }

        _ismove = false;
        yield break;
    }

    private void _Gravity()
    {
        _fallTime += Time.deltaTime;

        float _fallSpeed = Physics.gravity.y * _fallTime * 2f * 2f; //Unity�̕W���d�͂ɔC�������Ȃ� fallSpeed �͕s�v

        velocity.y += _fallSpeed * Time.fixedDeltaTime; // Y���x�ɏ��X�ɉ��Z
                                                        //Time.fixedDeltaTime �������Z���t���[�����[�g�Ɉˑ������Ȃ����ߕK�{
        if (velocity.y < -20f)//�������x�̐���
        {
            velocity.y = -20f;
        }
    }

    private IEnumerator Wait()
    {
        _iswait = true;
        yield return new WaitForSeconds(2f);
        _state = EnemyState.Move;
        _iswait = false;
        yield break;
    }

    private IEnumerator Attack()
    {
        _isattack = true;
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Attack");
        _attack.GunAttack();
        _isattack = false;
        yield break;
    }

    #region ��_������
    public IEnumerator _ReturnNormal(float time)
    {
        yield return new WaitForSeconds(time);
        _state = EnemyState.Look;
        yield break;
    }

    public void SKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(0.5f));
        //anim
    }

    public void BKnockBack(int dir, int knockback)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(1.0f));
        //anim
    }

    public void ElectStun(int dir, int knockback, float electtime)
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.AddForce(dir * knockback, knockback * 0.4f, 0f, ForceMode.Impulse);
        _state = EnemyState.Damage;
        StartCoroutine(_ReturnNormal(electtime));
    }
    #endregion

}
