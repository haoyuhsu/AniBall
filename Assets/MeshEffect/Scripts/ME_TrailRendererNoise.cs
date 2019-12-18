using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ME_TrailRendererNoise : MonoBehaviour
{

    [Range(0.01f, 10)]
    public float MinVertexDistance = 0.1f;
    public float VertexTime = 1;
    public float TotalLifeTime = 3;
    public bool SmoothCurves = false;
    public bool IsRibbon;
    public bool IsActive = true;

    [Range(0.001f, 10)] public float Frequency = 1f;

    [Range(0.001f, 10)] public float TimeScale = 0.1f;

    [Range(0.001f, 10)] public float Amplitude = 1;

    public float Gravity = 1;

    public float TurbulenceStrength = 1;

    public bool AutodestructWhenNotActive;
    LineRenderer lineRenderer;
    Transform t;
    Vector3 prevPos;

    List<Vector3> points = new List<Vector3>(500);
    List<float> lifeTimes = new List<float>(500);
    List<Vector3> velocities = new List<Vector3>(500);

    private float randomOffset;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        t = transform;
        prevPos = t.position;

        points.Insert(0, t.position);
        lifeTimes.Insert(0, VertexTime);
        velocities.Insert(0, Vector3.zero);
        randomOffset = Random.Range(0, 10000000) / 1000000f;
    }

    private void OnEnable()
    {
        points.Clear();
        lifeTimes.Clear();
        velocities.Clear();
        //if (Autodestruct) Destroy(gameObject, TotalLifeTime);
    }

    void Update()
    {
        if(IsActive) AddNewPoints();
        UpdatetPoints();

        if (SmoothCurves && points.Count > 2)
        {
            UpdateLineRendererBezier();
        }
        else
        {
            UpdateLineRenderer();
        }

        if(AutodestructWhenNotActive && !IsActive && points.Count <= 1) Destroy(gameObject, TotalLifeTime);
    }

    void AddNewPoints()
    {
        if ((t.position - prevPos).magnitude > MinVertexDistance ||
            IsRibbon && points.Count == 0 ||
            IsRibbon && points.Count > 0 && (t.position - points[0]).magnitude > MinVertexDistance)
        {
            prevPos = t.position;
            points.Insert(0, t.position);
            lifeTimes.Insert(0, VertexTime);
            velocities.Insert(0, Vector3.zero);
        }
    }

    void UpdatetPoints()
    {
        for (var i = 0; i < lifeTimes.Count; i++)
        {
            lifeTimes[i] -= Time.deltaTime;
            if (lifeTimes[i] <= 0)
            {
                var removedRange = lifeTimes.Count - i;
                lifeTimes.RemoveRange(i, removedRange);
                points.RemoveRange(i, removedRange);
                velocities.RemoveRange(i, removedRange);
                return;
            }
            else
            {
                CalculateTurbuelence(points[i], TimeScale, Frequency, Amplitude, Gravity, i);

            }
        }
    }

    void UpdateLineRendererBezier()
    {
        if (SmoothCurves && points.Count > 2)
        {
            InterpolateBezier(points, SmoothCurvesScale);
            var bezierPositions = GetDrawingPoints();
            lineRenderer.positionCount = bezierPositions.Count - 1;
            lineRenderer.SetPositions(bezierPositions.ToArray());
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = Mathf.Clamp(points.Count - 1, 0, Int32.MaxValue);
        lineRenderer.SetPositions(points.ToArray());
    }

    void CalculateTurbuelence(Vector3 position, float speed, float scale, float height, float gravity, int index)
    {
        float sTime = Time.timeSinceLevelLoad * speed + randomOffset;
        float xCoord = position.x * scale + sTime;
        float yCoord = position.y * scale + sTime + 10;
        float zCoord = position.z * scale + sTime + 25;
        position.x = (Mathf.PerlinNoise(yCoord, zCoord) - 0.5f) * height * Time.deltaTime;
        position.y = (Mathf.PerlinNoise(xCoord, zCoord) - 0.5f) * height * Time.deltaTime - gravity * Time.deltaTime;
        position.z = (Mathf.PerlinNoise(xCoord, yCoord) - 0.5f) * height * Time.deltaTime;
        points[index] += position * TurbulenceStrength;
    }

    private List<Vector3> controlPoints = new List<Vector3>();
    private int curveCount;
    private const float MinimumSqrDistance = 0.01f;
    private const float DivisionThreshold = -0.99f;
    private const float SmoothCurvesScale = 0.5f;

    #region Bezier

    public void InterpolateBezier(List<Vector3> segmentPoints, float scale)
    {
        controlPoints.Clear();

        if (segmentPoints.Count < 2)
            return;

        for (int i = 0; i < segmentPoints.Count; i++)
        {
            if (i == 0) // is first
            {
                Vector3 p1 = segmentPoints[i];
                Vector3 p2 = segmentPoints[i + 1];

                Vector3 tangent = (p2 - p1);
                Vector3 q1 = p1 + scale * tangent;

                controlPoints.Add(p1);
                controlPoints.Add(q1);
            }
            else if (i == segmentPoints.Count - 1) //last
            {
                Vector3 p0 = segmentPoints[i - 1];
                Vector3 p1 = segmentPoints[i];
                Vector3 tangent = (p1 - p0);
                Vector3 q0 = p1 - scale * tangent;

                controlPoints.Add(q0);
                controlPoints.Add(p1);
            }
            else
            {
                Vector3 p0 = segmentPoints[i - 1];
                Vector3 p1 = segmentPoints[i];
                Vector3 p2 = segmentPoints[i + 1];
                Vector3 tangent = (p2 - p0).normalized;
                Vector3 q0 = p1 - scale * tangent * (p1 - p0).magnitude;
                Vector3 q1 = p1 + scale * tangent * (p2 - p1).magnitude;

                controlPoints.Add(q0);
                controlPoints.Add(p1);
                controlPoints.Add(q1);
            }
        }

        curveCount = (controlPoints.Count - 1) / 3;
    }

    public List<Vector3> GetDrawingPoints()
    {
        List<Vector3> drawingPoints = new List<Vector3>();

        for (int curveIndex = 0; curveIndex < curveCount; curveIndex++)
        {
            List<Vector3> bezierCurveDrawingPoints = FindDrawingPoints(curveIndex);

            if (curveIndex != 0)
                //remove the fist point, as it coincides with the last point of the previous Bezier curve.
                bezierCurveDrawingPoints.RemoveAt(0);

            drawingPoints.AddRange(bezierCurveDrawingPoints);
        }

        return drawingPoints;
    }

    private List<Vector3> FindDrawingPoints(int curveIndex)
    {
        List<Vector3> pointList = new List<Vector3>();

        Vector3 left = CalculateBezierPoint(curveIndex, 0);
        Vector3 right = CalculateBezierPoint(curveIndex, 1);

        pointList.Add(left);
        pointList.Add(right);

        FindDrawingPoints(curveIndex, 0, 1, pointList, 1);

        return pointList;
    }

    private int FindDrawingPoints(int curveIndex, float t0, float t1,
        List<Vector3> pointList, int insertionIndex)
    {
        Vector3 left = CalculateBezierPoint(curveIndex, t0);
        Vector3 right = CalculateBezierPoint(curveIndex, t1);

        if ((left - right).sqrMagnitude < MinimumSqrDistance)
            return 0;

        float tMid = (t0 + t1) / 2;
        Vector3 mid = CalculateBezierPoint(curveIndex, tMid);

        Vector3 leftDirection = (left - mid).normalized;
        Vector3 rightDirection = (right - mid).normalized;

        if (Vector3.Dot(leftDirection, rightDirection) > DivisionThreshold || Mathf.Abs(tMid - 0.5f) < 0.0001f)
        {
            int pointsAddedCount = 0;

            pointsAddedCount += FindDrawingPoints(curveIndex, t0, tMid, pointList, insertionIndex);
            pointList.Insert(insertionIndex + pointsAddedCount, mid);
            pointsAddedCount++;
            pointsAddedCount += FindDrawingPoints(curveIndex, tMid, t1, pointList, insertionIndex + pointsAddedCount);

            return pointsAddedCount;
        }

        return 0;
    }

    public Vector3 CalculateBezierPoint(int curveIndex, float t)
    {
        int nodeIndex = curveIndex * 3;
        Vector3 p0 = controlPoints[nodeIndex];
        Vector3 p1 = controlPoints[nodeIndex + 1];
        Vector3 p2 = controlPoints[nodeIndex + 2];
        Vector3 p3 = controlPoints[nodeIndex + 3];

        return CalculateBezierPoint(t, p0, p1, p2, p3);
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0; //first term

        p += 3 * uu * t * p1; //second term
        p += 3 * u * tt * p2; //third term
        p += ttt * p3; //fourth term

        return p;
    }

    #endregion
}
