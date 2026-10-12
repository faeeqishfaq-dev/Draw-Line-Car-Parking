using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [HideInInspector]
    public bool isActive = true;

    [HideInInspector]
    public Vector3[] linePoints;

    public float maxLinelength;

    [SerializeField]
    private LinesDrawer linesDrawer;

    [Space]
    public Line line;
    public Park park;
    public Car car;

    [Space]
    [Header("Colors")]
    public Color carColor;

    [SerializeField]
    private Color lineColor;

    [SerializeField]
    private Color parkColor;

    private void OnEnable()
    {
        isActive = true;

        if (linesDrawer != null)
        {
            linesDrawer.OnParkLinkedToLine += OnParkLinkedToLineHandler;
        }
    }

    private void OnDisable()
    {
        if (linesDrawer != null)
        {
            linesDrawer.OnParkLinkedToLine -= OnParkLinkedToLineHandler;
        }
    }

    private void OnParkLinkedToLineHandler(
        Route route,
        List<Vector3> points
    )
    {
        if (route == this)
        {
            linePoints = points.ToArray();

            Game.Instance.RegisterRoute(this);
        }
    }

    public void Disactivate()
    {
        isActive = false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;

        if (line == null || park == null || car == null)
            return;

        if (line.lineRenderer == null || car.bottomTransform == null)
            return;

        if (line.lineRenderer.positionCount < 2)
            line.lineRenderer.positionCount = 2;

        line.lineRenderer.SetPosition(
            0,
            car.bottomTransform.position
        );

        line.lineRenderer.SetPosition(
            1,
            park.transform.position
        );

        car.SetColor(carColor);
        park.SetColor(carColor);
        line.SetColor(lineColor);
    }
#endif
}