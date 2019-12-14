using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallFloor : MonoBehaviour
{   
    public GameObject floors;
    public FloorController floorController;
    public float fallInterval = 1.0f;
    GameObject floorToFall;

    void Start()
    {
        StartCoroutine(FloorVanish());     // 每隔一段時間就使得地板掉落
    }

    IEnumerator FloorVanish()
    {
        while(true)
        {
            floorToFall = floorController.GetRandomFloorObject();           // 取得隨機地板物件
            floorToFall.transform.position -= new Vector3(0, 0.5f, 0);      // 將地板往下位移一小段距離避免卡住
            floorToFall.GetComponent<Rigidbody> ().isKinematic = false;     // 將Kinematic設為false, 使其可以移動
            yield return new WaitForSeconds(fallInterval);
        }
    }

}
