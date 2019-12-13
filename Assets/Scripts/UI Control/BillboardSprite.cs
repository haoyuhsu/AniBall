using System.Collections;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    Transform MyCameraTransform;           // 攝影機位置
    public Transform MyPlayerTransform;    // 玩家位置
	private Transform MyTransform;         // Slider Canvas位置
	public bool alignNotLook = true;
    public float offset = 0.5f;            // Y軸的offset

	void Start () {
		MyTransform = this.transform;
		MyCameraTransform = Camera.main.transform;
	}
	
	// Update is called once per frame
	void LateUpdate () {

		/* 把Slider顯示在玩家的頭上 */
        MyTransform.position = MyPlayerTransform.position + new Vector3(0, offset, 0);

		/* 將Slider對齊面向Camera視角 */
		if (alignNotLook)
			MyTransform.forward = MyCameraTransform.forward;
		else
			MyTransform.LookAt (MyCameraTransform, Vector3.up);
	}
}
