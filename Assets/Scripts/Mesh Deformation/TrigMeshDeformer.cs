using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigMeshDeformer : MonoBehaviour
{
    MeshDeformer[] meshDeformers;

    void Start()
    {
        meshDeformers = GetComponentsInChildren<MeshDeformer>();
    }

	void OnCollisionEnter(Collision other)
	{
        /* 當碰撞發生時, 把碰撞點資訊給到各MeshDeformer去做Vertex更新 */
		foreach (MeshDeformer meshDeformer in meshDeformers)
        {
            meshDeformer.isCollided = true;
            meshDeformer.Deforming(other);
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
