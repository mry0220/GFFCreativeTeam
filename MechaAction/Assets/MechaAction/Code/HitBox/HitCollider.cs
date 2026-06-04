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

    private Coroutine m_viewCoroutine;

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
                var damageable = col.GetComponentInParent<IDamage>();
                if (damageable == null) continue;
                if(hitSet.Contains(damageable)) continue;

                var team = col.GetComponentInParent<ITeam>();
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

                    overrideEffectData = m_overrideEffect,
                    overrideAudioData = m_overrideAudio
                };

                damageable.TakeDamage(data,result);
            }
        }

        if (m_isViewCollider)
        {
            if (m_viewCoroutine != null) return;

            m_viewCoroutine = StartCoroutine(ViewColliderTime());
        }
    }

    private IEnumerator ViewColliderTime()
    {
        m_isVisible = true;
        yield return new WaitForSeconds(0.5f);
        m_isVisible = false;

        m_viewCoroutine = null;

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
