using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArea : MonoBehaviour
{
    public enum AreaType
    {
        Normal,
        Enemy,
        Boss
    }

    [SerializeField] private Camera m_mainCamera;
    private CameraManager m_cameraManager;

    [SerializeField] private bool m_isViewCollider;
    [SerializeField] private AreaType m_type;

    private int m_priority;

    public int priority { get => m_priority; }

    public Bounds Bounds
    {
        get
        {
            var col = GetComponent<BoxCollider>();
            return new Bounds(transform.position + col.center, col.size);
        }
    }

    private void Awake()
    {
        m_cameraManager = m_mainCamera.GetComponent<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //var team = other.GetComponentInParent<ITeam>();

        //if (team == null) return;

        //if (team.Team != TeamType.Player) return;

        //if(m_cameraManager != null)
        //{
        //    m_cameraManager.SetArea(this);
        //}
    }

    private void OnDrawGizmos()
    {
        if (!m_isViewCollider) return;

        var col = GetComponent<BoxCollider>();
        if (col == null) return;

        if(m_type == AreaType.Normal)
        {
            Gizmos.color = Color.green;
        }
        else if(m_type == AreaType.Enemy)
        {
            Gizmos.color = Color.blue;
        }
        else if(m_type == AreaType.Boss)
        {
            Gizmos.color = Color.cyan;
        }
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(col.center, col.size);
    }
}
