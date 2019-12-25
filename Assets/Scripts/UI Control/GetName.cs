using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetName : MonoBehaviour
{
    public Text nameText;
    public PlayerColor playerColor;
    string MyName;

    void Start()
    {
        MyName = gameObject.transform.parent.name;
        nameText.color = playerColor.myColor;
        nameText.text = MyName;
    }
}
