using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPosControl : MonoBehaviour
{
    RectTransform rectTransform = null;

    public Transform target = null;

    [SerializeField]
    Vector2 offset = Vector2.zero;

    [SerializeField]
    TextMeshProUGUI damageText;

    //Vector3 screenPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }

    //ê∂ê¨éûÇ…àÍìxÇæÇØåƒÇ‘
    //public void Initialize(Vector3 worldPos)
    //{
    //    screenPos = RectTransformUtility.WorldToScreenPoint(
    //        Camera.main,
    //        worldPos
    //    );
    //}

    void Update()
    {
        rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, target.position) + offset;

        //screenPos += Vector3.up * 40f * Time.deltaTime;
        //rectTransform.position = screenPos + (Vector3)offset;
    }
}
