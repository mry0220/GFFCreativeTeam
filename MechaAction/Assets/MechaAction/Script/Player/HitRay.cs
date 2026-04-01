using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRay : MonoBehaviour
{
    [SerializeField] private bool m_isViewCollider;
    private bool m_isVisible;

    [System.Serializable]
    public class AttackHitBox
    {
        public Transform m_Pos;
        public float m_radius;

        public float m_distance;
    }
    [SerializeField] private AttackHitBox[] hitBoxes;

    [SerializeField] private int m_count;//貫通人数
    [SerializeField] private LayerMask m_layer;//ignoreLayer
    [SerializeField] private EffectDataSO m_overrideEffect;
    [SerializeField] private AudioDataSO m_overrideAudio;

    public void AttackCastAll(DamageData data, TeamType myteam)
    {
        HashSet<IDamage> hitSet = new HashSet<IDamage>();

        foreach (var hitBox in hitBoxes)
        {
            if (hitBox.m_Pos == null) continue;

            RaycastHit[] hits = Physics.SphereCastAll(
                hitBox.m_Pos.position,
                hitBox.m_radius,
                data.attackDir,
                hitBox.m_distance
            );

            foreach (var hit in hits)
            {
                var col = hit.collider;
                var damageable = col.GetComponent<IDamage>();
                if (damageable == null) continue;
                if (hitSet.Contains(damageable)) continue;

                var team = col.GetComponent<ITeam>();
                if (team != null)
                {
                    // 同じチームなら無視
                    if (team.Team == myteam) continue;
                }
                else
                {
                    continue;
                }

                hitSet.Add(damageable);

                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal,

                    overrideEffect = m_overrideEffect,
                    overrideAudio = m_overrideAudio
                };

                damageable.TakeDamage(data, result);
            }
        }

        if (m_isViewCollider)
        {
            StartCoroutine(ViewColliderTime());
        }
    }

    public void AttackCast(DamageData data, TeamType myteam)
    {
        foreach (var hitBox in hitBoxes)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                hitBox.m_Pos.position,
                hitBox.m_radius,
                data.attackDir,
                hitBox.m_distance
            );

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                var col = hit.collider;

                if(col.gameObject.layer == m_layer)
                {
                    break;
                }

                var damageable = col.GetComponent<IDamage>();
                if (damageable == null) continue;

                var team = col.GetComponent<ITeam>();
                if (team != null)
                {
                    // 同じチームなら無視
                    if (team.Team == myteam) continue;
                }
                else
                {
                    continue;
                }

                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal,
                    overrideEffect = m_overrideEffect,
                    overrideAudio = m_overrideAudio
                };

                damageable.TakeDamage(data, result);

                break;
            }
        }

        if (m_isViewCollider)
        {
            StartCoroutine(ViewColliderTime());
        }
    }

    public void AttackCastPenetration(DamageData data, TeamType myteam)
    {
        foreach (var hitBox in hitBoxes)
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                hitBox.m_Pos.position,
                hitBox.m_radius,
                data.attackDir,
                hitBox.m_distance
            );

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            int hitCount = 0;

            foreach (var hit in hits)
            {
                var col = hit.collider;

                if (col.gameObject.layer == m_layer)
                {
                    break;
                }

                var damageable = col.GetComponent<IDamage>();
                if (damageable == null) continue;

                var team = col.GetComponent<ITeam>();
                if (team != null)
                {
                    // 同じチームなら無視
                    if (team.Team == myteam) continue;
                }
                else
                {
                    continue;
                }

                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal,
                    overrideEffect = m_overrideEffect,
                    overrideAudio = m_overrideAudio
                };

                damageable.TakeDamage(data, result);

                hitCount++;

                if (hitCount >= m_count)
                    break;
            }
        }

        if (m_isViewCollider)
        {
            StartCoroutine(ViewColliderTime());
        }
    }

    private IEnumerator ViewColliderTime()
    {
        m_isVisible = true;
        yield return new WaitForSeconds(3f);
        m_isVisible = false;

        yield break;
    }

    private void OnDrawGizmos()
    {
        if (!m_isVisible) return;

        Gizmos.color = Color.red;

        foreach (var hitBox in hitBoxes)
        {
            if (hitBox.m_Pos == null) continue;
            Vector3 start = hitBox.m_Pos.position;
            Vector3 end = start + hitBox.m_Pos.forward * hitBox.m_distance;

            // 開始地点
            Gizmos.DrawWireSphere(start, hitBox.m_radius);

            // 終了地点
            Gizmos.DrawWireSphere(end, hitBox.m_radius);

            // 線
            Gizmos.DrawLine(start, end);
        }
    }
}
