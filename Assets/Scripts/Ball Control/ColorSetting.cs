using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetting : MonoBehaviour
{
    MeshRenderer[] meshRenderers;
    List<Vector4> colorList = new List<Vector4> ();

    void Start()
    {
        /* 取得Hierarchy底部各身體組件的Materials */
        meshRenderers = GetComponentsInChildren<MeshRenderer> ();

        /* 取得各組件的原始顏色 */
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            colorList.Add(meshRenderer.material.color);
        }
    }

    public void SetColor(Vector4 colorToSet)
    {
        /* 更新所有組件的Materials的顏色 */
        foreach(MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = colorToSet;
        }
    }

    public void ResetColor()
    {
        /* 還原所有組件的Materials的顏色 */
        for (int i=0; i<meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = colorList[i];
        }
    }
}
