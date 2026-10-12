using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [HideInInspector]
    public List<Route> readyRoutes = new();

    private int totalRoutes;
    private int successfulParks;

    private bool levelEnded;

    public UnityAction<Route> OnCarEntersPark;
    public UnityAction OnCarCollision;

    private void Awake()
    {
        Instance = this;

        OnCarEntersPark += OnCarEntersParkHandler;
        OnCarCollision += OnCarCollisionHandler;
    }

    /// <summary>
    /// Har naye level ke start par LevelManager ye function call karega.
    /// </summary>
    public void PrepareLevel(GameObject levelRoot)
    {
        readyRoutes.Clear();

        successfulParks = 0;
        levelEnded = false;

        // Sirf current level ke routes count honge.
        totalRoutes = levelRoot.GetComponentsInChildren<Route>(true).Length;

        Debug.Log(
            "Level Prepared. Total Routes: " + totalRoutes
        );
    }

    private void OnCarCollisionHandler()
    {
        if (levelEnded)
            return;

        levelEnded = true;

        Debug.Log("Level Failed");

        // Thora delay, phir fail panel.
        DOVirtual.DelayedCall(1f, () =>
        {
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.ShowLevelFail();
            }
        });
    }

    private void OnCarEntersParkHandler(Route route)
    {
        if (levelEnded)
            return;

        route.car.StopDancingAnim();

        successfulParks++;

        Debug.Log(
            "Parked: " + successfulParks + "/" + totalRoutes
        );

        if (successfulParks >= totalRoutes)
        {
            levelEnded = true;

            Debug.Log("Level Complete");

            DOVirtual.DelayedCall(1.3f, () =>
            {
                if (LevelManager.Instance != null)
                {
                    LevelManager.Instance.ShowLevelComplete();
                }
            });
        }
    }

    public void RegisterRoute(Route route)
    {
        if (levelEnded)
            return;

        // Same route double register na ho.
        if (!readyRoutes.Contains(route))
        {
            readyRoutes.Add(route);
        }

        if (readyRoutes.Count == totalRoutes)
        {
            MoveAllCars();
        }
    }

    private void MoveAllCars()
    {
        foreach (Route route in readyRoutes)
        {
            route.car.Move(route.linePoints);
        }
    }

    private void OnDestroy()
    {
        OnCarEntersPark -= OnCarEntersParkHandler;
        OnCarCollision -= OnCarCollisionHandler;

        if (Instance == this)
        {
            Instance = null;
        }
    }
}