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
        public Transform m_startPos;
        public Transform m_endPos;
        public float m_radius;

    }
    [SerializeField] private AttackHitBox[] hitBoxes;

    [SerializeField] private int m_count;//ä—í êlêî
    [SerializeField] private LayerMask m_layer;//ignoreLayer

    private Coroutine m_viewCoroutine;

    public void AttackCastAll(DamageData data, TeamType myteam)
    {
        HashSet<Entity> hitSet = new();

        foreach (var hitBox in hitBoxes)
        {
            Vector3 start = hitBox.m_startPos.position;
            Vector3 end = hitBox.m_endPos.position;

            Vector3 dir = (end - start).normalized;

            float distance = Vector3.Distance(start, end);

            RaycastHit[] hits = Physics.SphereCastAll(
                hitBox.m_startPos.position,
                hitBox.m_radius,
                dir,
                distance
            );

            foreach (var hit in hits)
            {
                var col = hit.collider;

                var entity = col.GetComponentInParent<Entity>();
                if (entity == null) continue;
                if (hitSet.Contains(entity)) continue;

                if(entity.Team == myteam) continue;

                hitSet.Add(entity);

                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal
                };

                entity.OnTakeDamage(data, result);
            }
        }

        if (m_isViewCollider)
        {
            if (m_viewCoroutine != null) return;

            m_viewCoroutine = StartCoroutine(ViewColliderTime());
        }
    }

    public void AttackCast(DamageData data, TeamType myteam)
    {
        foreach (var hitBox in hitBoxes)
        {
            Vector3 start = hitBox.m_startPos.position;
            Vector3 end = hitBox.m_endPos.position;

            Vector3 dir = (end - start).normalized;

            float distance = Vector3.Distance(start, end);

            RaycastHit[] hits = Physics.SphereCastAll(
                hitBox.m_startPos.position,
                hitBox.m_radius,
                dir,
                distance
            );

            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

            foreach (var hit in hits)
            {
                var col = hit.collider;

                if(col.gameObject.layer == m_layer)
                {
                    break;
                }

                var entity = col.GetComponentInParent<Entity>();
                if (entity == null) continue;

                if(entity.Team == myteam) continue;

                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal
                };

                entity.OnTakeDamage(data, result);

                break;
            }
        }

        if (m_isViewCollider)
        {
            if (m_viewCoroutine != null) return;

            m_viewCoroutine = StartCoroutine(ViewColliderTime());
        }
    }

    public void AttackCastPenetration(DamageData data, TeamType myteam)
    {
        foreach (var hitBox in hitBoxes)
        {
            Vector3 start = hitBox.m_startPos.position;
            Vector3 end = hitBox.m_endPos.position;

            Vector3 dir = (end - start).normalized;

            float distance = Vector3.Distance(start, end);

            RaycastHit[] hits = Physics.SphereCastAll(
                hitBox.m_startPos.position,
                hitBox.m_radius,
                dir,
                distance
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

                var entity = col.GetComponentInParent<Entity>();
                if (entity == null) continue;
             
                if(entity.Team == myteam) continue;

                Vector3 hitPoint = hit.point;
                Vector3 hitNormal = hit.normal;

                DamageResult result = new DamageResult
                {
                    hitPoint = hitPoint,
                    hitNormal = hitNormal
                };

                entity.OnTakeDamage(data, result);

                hitCount++;

                if (hitCount >= m_count)
                    break;
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

            Vector3 start = hitBox.m_startPos.position;
            Vector3 end = hitBox.m_endPos.position;

            Gizmos.DrawWireSphere(start, hitBox.m_radius);

            Gizmos.DrawWireSphere(end, hitBox.m_radius);

            Gizmos.DrawLine(start, end);
        }
    }
}
