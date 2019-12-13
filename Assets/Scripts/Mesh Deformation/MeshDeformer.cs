using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh;
	Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;
    public float springForce = 30f;
    public float damping = 7f;
    float uniformScale = 1f;
	public float force = 2.5f;
    public float forceOffset = 0.1f;

    void Start () {
		deformingMesh = GetComponent<MeshFilter>().mesh;
		originalVertices = deformingMesh.vertices;
		displacedVertices = new Vector3[originalVertices.Length];
		for (int i = 0; i < originalVertices.Length; i++) {
			displacedVertices[i] = originalVertices[i];
		}
        vertexVelocities = new Vector3[originalVertices.Length];
	}

    void Update () {
        uniformScale = transform.localScale.x;
		for (int i = 0; i < displacedVertices.Length; i++) {
			UpdateVertex(i);
		}
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
	}

    void AddForceToVertex (int i, Vector3 point, float force)
    {
        Vector3 pointToVertex = displacedVertices[i] - point;
        pointToVertex *= uniformScale;
		float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
		float velocity = attenuatedForce * Time.deltaTime;
		vertexVelocities[i] += pointToVertex.normalized * velocity;
	}

    void UpdateVertex (int i)
    {
		Vector3 velocity = vertexVelocities[i];
		Vector3 displacement = displacedVertices[i] - originalVertices[i];
        displacement *= uniformScale;
		velocity -= displacement * springForce * Time.deltaTime;
		velocity *= 1f - damping * Time.deltaTime;
		vertexVelocities[i] = velocity;
		displacedVertices[i] += velocity * (Time.deltaTime / uniformScale);
	}

    void AddDeformingForce (Vector3 point, float force)
    {
        point = transform.InverseTransformPoint(point);
		for (int i = 0; i < displacedVertices.Length; i++) {
			AddForceToVertex(i, point, force);
		}
	}

	/*bool isCollided;
	void OnCollisionEnter(Collision other)
	{
		Debug.Log("Collide : " + gameObject.name);
		isCollided = true;
		Deforming(other);
	}

	void OnCollisionExit(Collision other)
	{
		isCollided = false;
	}*/

	public float deformingTime = 0.8f;
    public bool isCollided;
	public void Deforming(Collision other)
	{
		/* Deforming函式透過傳入碰撞點位置去更新每個Vertex的位置 */
		float timer = 0f;
		ContactPoint[] collisionPoints = other.contacts;
		while(isCollided && timer < deformingTime)
		{
			timer += Time.deltaTime;
			for (int i = 0; i < collisionPoints.Length; i++)
			{
				Vector3 point = collisionPoints[i].point;
				//point += collisionPoints[i].normal * forceOffset;
				AddDeformingForce(point, force);
			}
		}
	}
}
