using System.Collections;
using UnityEngine;

public class BigEnemy : MonoBehaviour
{
    private enum EnemyState
    {
        Look,          //�T��
        Move,          //����
        Charge,        //�ːi
        Jump,          //�W�����v
        BigJump,       //��W�����v
        Wait,          //����
        Attack,        //�ߐڍU��
        Damage
    }

    private EnemyState _state = EnemyState.Look;

    private Transform _player;
    private Rigidbody _rb;
    private Big_Attack _attack;


    public int Dir => _dir;
    private int _dir = -1;
    private float _jumpPower = 13f;
    private float _bigjumpPower = 18f;
    private float _moveSpeed = 5f;
    private bool _moveStop = false;
    private bool _iswait = false;//wait�R���[�`���̏d����h��
    private bool _ismove = false;//move�R���[�`���̏d����h��
    private bool _ischarge = false;//charge�R���[�`���̏d����h��
    private bool _isjump = false;//jump�R���[�`���̏d����h��
    private bool _isbigjump = false;//bigjump�R���[�`���̏d����h��
    private bool _isattack = false;//attack�R���[�`���̏d����h��

    Vector3 velocity;

    private float _fallTime;
    Vector3 origin;
    private bool _isGrounded;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody>();
        _attack = GetComponent<Big_Attack>();
    }

    private void Start()
    {
        
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
        _isGrounded = Physics.SphereCast(origin, 0.4f, Vector3.down, out hit, 2f, LayerMask.GetMask("Grounded"));
        //Debug.Log(_isGrounded);
        Debug.DrawRay(transform.position, transform.forward * 10f, Color.cyan);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(origin, 0.4f);
        Gizmos.DrawWireSphere(origin + Vector3.down * 2f, 0.4f);
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
               
                Direction();
                velocity.x = _dir * _moveSpeed;

                if (!_ismove)//�R���[�`���̏d���h��
                    StartCoroutine(MoveTimelimit());
                break;

            case EnemyState.Charge:
                if (!_ischarge)//�R���[�`���̏d���h��
                    StartCoroutine(Charge());
                break;

            case EnemyState.Jump:
                if(!_isjump)
                StartCoroutine(Jump());
                break;

            case EnemyState.BigJump:
                if (!_isbigjump)//�R���[�`���̏d���h��
                    StartCoroutine(BigJump());
                break;

            case EnemyState.Wait:
                Direction();
                if (!_iswait)//�R���[�`���̏d���h��
                    StartCoroutine(Wait());//���΂炭�҂���Move��
                break;

            case EnemyState.Attack:
                if (!_isattack)//�R���[�`���̏d���h��
                    StartCoroutine(Attack());
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
        
    }

    private void Direction()
    {
        if (!_isGrounded)
        {
            return;
        }

        if (_moveStop)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }

        if (_rb.position.x < _player.position.x)
        {
            if (_dir == -1)
            {
                StartCoroutine(Waitturn(1));
            }
            _dir = 1;
        }
        else
        {
            if (_dir == 1)
            {
                StartCoroutine(Waitturn(-1));
            }
            _dir = -1;
        }
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


    private IEnumerator Waitturn(int _newdirection)
    {
        _moveStop = true;
        yield return new WaitForSeconds(0.5f);

        _dir = _newdirection;
        _moveStop = false;

        yield break;
    }

    private IEnumerator MoveTimelimit()
    {
        _ismove = true;
        Debug.Log("Move");
        yield return new WaitForSeconds(3f);

        _state = EnemyState.Wait;
        _ismove = false;
        yield break;
    }

    private IEnumerator Charge()
    {
        _ischarge = true;
        Debug.Log("Charge����");
        yield return new WaitForSeconds(0.2f);

        _rb.AddForce(_dir * 20f, 0f, 0f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.8f);
        _ischarge = false;
        _state = EnemyState.Wait;
        yield break;
    }

    private IEnumerator Jump()
    {
        _isjump = true;
        int _rand = Random.Range(1, 4);//1�ȏ�4����
        if (Vector3.Distance(transform.position, _player.position) < 5f)
        {
            Debug.Log("backjump");
            _rb.AddForce(_dir * -7f, _jumpPower, 0f, ForceMode.Impulse);
            yield return new WaitForSeconds(1f);

            yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                                   //�㏑������new Vector3���g���Ȃ���(AI�Q��)
            _rb.linearVelocity = Vector3.zero;

            _state = EnemyState.Wait;
            _isjump = false;
            yield break;
        }
        else
        {
            Debug.Log("frontjump");
            _rb.AddForce(_dir * 9f, _jumpPower, 0f, ForceMode.Impulse);
            yield return new WaitForSeconds(0.4f);
            if (_rand == 1)
            {
                StartCoroutine(JumpAttack());
                _isjump = false;
                yield break;
            }
            yield return new WaitForSeconds(0.6f);

            yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                                   //�㏑������new Vector3���g���Ȃ���(AI�Q��)
            _rb.linearVelocity = Vector3.zero;

            _state = EnemyState.Attack;
            _isjump = false;
            yield break;
        }
        
    }

    private IEnumerator BigJump()
    {
        _isbigjump = true;
        Debug.Log("bigjump");
        //int _rand = Random.Range(1, 4);//1�ȏ�4����
        
        _rb.AddForce(_dir * 20f, _bigjumpPower, 0f, ForceMode.Impulse);

        yield return new WaitForSeconds(1f);

        yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                               //�㏑������new Vector3���g���Ȃ���(AI�Q��)
        _rb.linearVelocity = Vector3.zero;

        _state = EnemyState.Wait;

        _isbigjump = false;
        yield break;
    }

    private IEnumerator Wait()
    {
        _iswait = true;
        Debug.Log("wait");
        yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                               //�㏑������new Vector3���g���Ȃ���(AI�Q��)
        _rb.linearVelocity = Vector3.zero; //move�̌�Ȃǃs�^���ƂƂ߂���

        yield return new WaitForSeconds(2f);

        int _rand = Random.Range(0, 10);//0�ȏ�10����
        
        if(Vector3.Distance(transform.position, _player.position) > 10f)
        {
            if( _rand >= 0 && _rand <5)
            {
                _state = EnemyState.BigJump;
            }
            else if(_rand >= 5 && _rand < 8)
            {
                _state = EnemyState.Jump;
            }
            else
            {
                _state = EnemyState.Move;
            }
        }
        else if (Vector3.Distance(transform.position, _player.position) <= 10f &&
            Vector3.Distance(transform.position, _player.position) > 4f)
        {
            if (_rand >= 0 && _rand < 5)
            {
                _state = EnemyState.Jump;
            }
            else if (_rand >= 5 && _rand < 8)
            {
                _state = EnemyState.Move;
            }
            else
            {
                _state = EnemyState.Charge;
            }
        }
        else
        {
            if (_rand >= 0 && _rand < 5)
            {
                _state = EnemyState.Charge;
            }
            else
            {
                _state = EnemyState.Attack;
            }
        }
        _iswait = false;
        yield break;
    }

    private IEnumerator Attack()
    {
        _isattack = true;
        yield return new WaitForSeconds(0.5f);

        _attack.Attack();
        _state = EnemyState.Wait;
        _isattack = false;
        yield break;
    }

    private IEnumerator JumpAttack()
    {
        yield return new WaitForFixedUpdate(); //�R���[�`��������FixedUpdate(Update?)��
                                               //�㏑������new Vector3���g���Ȃ���(AI�Q��)
        _rb.linearVelocity = new Vector3(0f, -50f, 0f);
        _attack.JumpAttack();
        _state = EnemyState.Wait;

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
