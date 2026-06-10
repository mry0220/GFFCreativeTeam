using UnityEngine;

public class BlurObject : MonoBehaviour
{
    ///<summary> Blur shader property ID </summary>
    private static readonly int PROPERTY_TRAIL_DIR = Shader.PropertyToID("_TrailDir");

    [SerializeField]
    private Renderer m_renderer;

    private Material m_material;

    private Vector3 m_trailPos;

    ///<summary> Trail speed </summary>
    [SerializeField]
    private float m_trailRate = 10f;

    private void Awake()
    {
        m_material = m_renderer.material;
        m_trailPos = transform.position;
    }

    private void Update()
    {
        m_trailPos = Vector3.Lerp(m_trailPos, transform.position, Mathf.Clamp01(Time.deltaTime * m_trailRate));

        Vector3 dir = transform.InverseTransformDirection(m_trailPos - transform.position);
        m_material.SetVector(PROPERTY_TRAIL_DIR, dir);
    }
}
