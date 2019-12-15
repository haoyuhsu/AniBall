using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigMeshDeformer : MonoBehaviour
{
    MeshDeformer[] meshDeformers;

    float cooldownTime = 0f;            // 避免頻繁碰撞使得Mesh更新次數太大, 讓遊戲卡頓
    float deformInterval = 0.1f;        // 每次Update Mesh之間的時間間隔

    void Start()
    {
        meshDeformers = GetComponentsInChildren<MeshDeformer>();
    }

    void FixedUpdate()
    {
        if (cooldownTime <= 0f)
            return;
        cooldownTime -= Time.deltaTime;
    }

	void OnCollisionEnter(Collision other)
	{
        if (cooldownTime <= 0f)
        {
            /* 當碰撞發生時, 把碰撞點資訊給到各MeshDeformer去做Vertex更新 */
            foreach (MeshDeformer meshDeformer in meshDeformers)
            {
                meshDeformer.isCollided = true;
                meshDeformer.Deforming(other);
            }
            cooldownTime = deformInterval;
        }
	}

	void OnCollisionExit(Collision other)
	{
		foreach (MeshDeformer meshDeformer in meshDeformers)
        {
            meshDeformer.isCollided = false;
        }
	}
}
