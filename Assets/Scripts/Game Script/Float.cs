using System.Collections;
using UnityEngine;

public class Float : MonoBehaviour
{
    /* 物體漂浮參數控制 */
    public float degreesPerSecond = 15.0f;   // 物體旋轉速度
    public float amplitude = 0.2f;           // 上下震盪強度
    public float maxfrequency = 1.3f;
    public float minfrequency = 1f;
    float frequency;
 
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
 

    void Start () {
        posOffset = transform.position;
        frequency = Random.Range(minfrequency, maxfrequency);  // 取得隨機上下震盪頻率
    }
     
    void Update () {
        /* 物體繞著y軸做旋轉 */
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);
 
        /* 物體做上下震盪 */
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = tempPos;
    }
}
