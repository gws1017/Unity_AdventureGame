using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystic : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform Handler;
    private RectTransform rectTransform;
    [SerializeField]
    private Canvas mainCanvas;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField,Range(10,150)]
    private float HandlerRange;
    private bool IsDrag = false;

    private Vector2 InputDirection;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDrag = true;
        ControllJoysticHandler(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControllJoysticHandler(eventData);

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        IsDrag = false;
        Handler.anchoredPosition = Vector2.zero;
        playerInput.ClearCache();
    }
    private void ControllJoysticHandler(PointerEventData eventData)
    {
        var scaledAnchoredPosition = rectTransform.anchoredPosition * mainCanvas.transform.localScale.x;
        var InputPos = eventData.position - scaledAnchoredPosition;
        var InputVector = InputPos.magnitude < HandlerRange ? InputPos : InputPos.normalized * HandlerRange;
        Handler.anchoredPosition = InputVector;
        InputDirection = InputVector / HandlerRange;
    }

    public void InputControlVector()
    {
        playerInput.HorizonItalInput = InputDirection.x;
        playerInput.VerticalInput = InputDirection.y;
    }

    private void Update()
    {
        if (IsDrag)
        {
            InputControlVector();
        }
    }
}
