using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;

    [SerializeField] private float minPointsDistance;

    [HideInInspector] public List<Vector3> points = new List<Vector3>();
    [HideInInspector] public int pointsCount = 0;
    [HideInInspector] public float length;

    private float pointFixedYAxis;


    private Vector3 prevPoint;

    private void Start()
    {
        if (lineRenderer == null)
            return;

        // Safe Y initialization
        if (lineRenderer.positionCount > 0)
            pointFixedYAxis = lineRenderer.GetPosition(0).y;
        else
            pointFixedYAxis = transform.position.y;

        Clear();
    }

    public void Init()
    {
        if (lineRenderer == null)
            return;

        points.Clear();
        pointsCount = 0;
        lineRenderer.positionCount = 0;

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        if (lineRenderer != null)
            lineRenderer.positionCount = 0;

        pointsCount = 0;
        points.Clear();

        gameObject.SetActive(false);
        length = 0;
    }

    public void AddPoint(Vector3 newPoint)
    {
        if (lineRenderer == null)
            return;

        if (pointsCount == 0)
            prevPoint = newPoint;
        length += Vector3.Distance(prevPoint, newPoint);
        prevPoint = newPoint;

        newPoint.y = pointFixedYAxis;

        if (pointsCount >= 1 &&
            Vector3.Distance(newPoint, GetLastPoint()) < minPointsDistance)
        {
            return;
        }

        points.Add(newPoint);
        pointsCount++;

        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(pointsCount - 1, newPoint);
    }

    private Vector3 GetLastPoint()
    {
        if (lineRenderer == null || pointsCount <= 0)
            return transform.position;

        return lineRenderer.GetPosition(pointsCount - 1);
    }

    public void SetColor(Color color)
    {
        if (lineRenderer == null)
            return;

        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }
}