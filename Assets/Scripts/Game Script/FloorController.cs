using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public GameObject floors;
    int numFloors = 0;
    int randomIndex;
    Transform tf;
    GameObject floorObject;

    /* 取得隨機可生成位置 */
    public Vector3 GetRandomFloorPosition()
    {
        numFloors = floors.transform.childCount;                              // 目前存在Floor個數
        randomIndex = Random.Range(0, numFloors);                             // 取得隨機Index
        tf = floors.transform.GetChild(randomIndex);                          // 取得該Index對應物件的Transform
        while (tf.gameObject.GetComponent<Rigidbody>().isKinematic == false)  // 如果該地板正在掉落, 則重新選取生成位置
        {
            randomIndex = Random.Range(0, numFloors);
            tf = floors.transform.GetChild(randomIndex);
        }
        return tf.position;
    }

    /* 取得隨機地板物件 */
    public GameObject GetRandomFloorObject()
    {
        numFloors = floors.transform.childCount;                              // 目前存在Floor個數
        randomIndex = Random.Range(0, numFloors);                             // 取得隨機Index
        floorObject = floors.transform.GetChild(randomIndex).gameObject;      // 取得該Index對應物件的GameObject
        return floorObject;
    }
}
