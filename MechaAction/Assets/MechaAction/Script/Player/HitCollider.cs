using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour
{
    [SerializeField] private bool m_isViewCollider;
    private bool m_isVisible;

    [System.Serializable]
    public class AttackHitBox
    {
        public Transform m_Pos;
        public float m_radius;
    }
    [SerializeField] private AttackHitBox[] hitBoxes;

    [SerializeField] private EffectDataSO m_overrideEffect;
    [SerializeField] private AudioDataSO m_overrideAudio;

    public void AttackCollider(DamageData data,TeamType myteam)
    {
        HashSet<IDamage> hitSet = new HashSet<IDamage>();

        foreach (var hitBox in hitBoxes)
        {
            if (hitBox.m_Pos == null) continue;

            Collider[] hits = Physics.OverlapSphere(
                hitBox.m_Pos.position,
                hitBox.m_radius
            );

            foreach (var col in hits)
            {
                var damageable = col.GetComponent<IDamage>();
                if (damageable == null) continue;
                if(hitSet.Contains(damageable)) continue;

                var team = col.GetComponent<ITeam>();
                if (team != null)
                {
                    // ìØÇ∂É`Å[ÉÄÇ»ÇÁñ≥éã
                    if (team.Team == myteam) continue;
                }
                else
                {
                    continue;
                }

                hitSet.Add(damageable);

                Vector3 hitPoint = col.ClosestPoint(hitBox.m_Pos.position);
                Vector3 hitNormal = (hitPoint - hitBox.m_Pos.position).normalized;

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal,

                    overrideEffect = m_overrideEffect,
                    overrideAudio = m_overrideAudio
                };

                damageable.TakeDamage(data,result);
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
            Gizmos.DrawWireSphere(
                hitBox.m_Pos.position,
                hitBox.m_radius);
        }
    }
}
