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

    private Coroutine m_viewCoroutine;

    public void AttackCollider(DamageData data,TeamType myteam)
    {
        HashSet<Entity> hitSet = new HashSet<Entity>();

        foreach (var hitBox in hitBoxes)
        {
            if (hitBox.m_Pos == null) continue;

            Collider[] hits = Physics.OverlapSphere(
                hitBox.m_Pos.position,
                hitBox.m_radius
            );

            foreach (var col in hits)
            {
                var entity = col.GetComponentInParent<Entity>();
                if (entity == null) continue;
                if(hitSet.Contains(entity)) continue;

                if(entity.Team == myteam) continue;

                hitSet.Add(entity);

                Vector3 hitPoint = col.ClosestPoint(hitBox.m_Pos.position);
                Vector3 hitNormal = (hitPoint - hitBox.m_Pos.position).normalized;

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal
                };

                entity.OnTakeDamage(data,result);
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
            Gizmos.DrawWireSphere(
                hitBox.m_Pos.position,
                hitBox.m_radius);
        }
    }
}
