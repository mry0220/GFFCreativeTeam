using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private Toggle _commandtoggle;

    private void Awake()
    {
        _commandtoggle = GetComponent<Toggle>();
       
        if(_commandtoggle != null)
            _commandtoggle.onValueChanged.AddListener(CommandCheck);
    }

    public void CommandCheck(bool isOn)
    {
        if (isOn)
        {
            GManager.Instance.CommandCheck(true);
            Debug.Log("on");
        }
        else
        {
            GManager.Instance.CommandCheck(false);
            Debug.Log("off");
        }
    }

    public void OptionButton()
    {
        GManager.Instance.Option();
    }
}
