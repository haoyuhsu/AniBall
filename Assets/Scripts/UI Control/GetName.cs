using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetName : MonoBehaviour
{
    public Text nameText;
    string MyName;

    void Start()
    {
        MyName = gameObject.transform.parent.name;
        nameText.text = MyName;
    }
}
