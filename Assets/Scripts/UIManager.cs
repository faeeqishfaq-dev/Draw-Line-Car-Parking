using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] LinesDrawer linesDrawer;

    [Space]
    [SerializeField] private CanvasGroup availableLineCanvasGroup;
    [SerializeField] private GameObject availableLineHolder;
    [SerializeField] private Image availableLineFill;

    private bool isAvailableLineUIActive = false;

    [Space]
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeDuration;

    private Route activeRoute;



    private void Start()
    {
        fadePanel.DOFade(0F, fadeDuration).From(1f);

        availableLineCanvasGroup.alpha = 0f;

        linesDrawer.OnBeginDraw += OnBeginDrawHandler;
        linesDrawer.OnDraw      += OnDrawHandler;
        linesDrawer.OnEndDraw   += OnEndDrawHandler;

    }


    private void OnBeginDrawHandler(Route route)
    {
        activeRoute = route;

        availableLineFill.color = activeRoute.carColor;
        availableLineFill.fillAmount = 1f;
        availableLineCanvasGroup.DOFade(1f, .3f).From(0f);
        isAvailableLineUIActive = true;
    }

    private void OnDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            float maxLineLength = activeRoute.maxLinelength;
            float linelength = activeRoute.line.length;

            availableLineFill.fillAmount = 1 - (linelength / maxLineLength);
        }
    }

    private void OnEndDrawHandler()
    {
        if (isAvailableLineUIActive)
        {
            isAvailableLineUIActive = false;
            activeRoute = null;

            availableLineCanvasGroup.DOFade(0f, .3f).From(1f);

        }
    }

}
