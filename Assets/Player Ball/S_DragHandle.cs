using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class S_DragHandle : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector2 originalPosition;
    private Vector2 dragStartPosition;
    private Vector2 dragEndPosition;
    public GameObject playerBall;
    public float offset = 0f;

    void Start()
    {
        UpdatePosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!playerBall.GetComponent<S_PlayerBall>().isMoving)
        {
            transform.position = eventData.position;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!playerBall.GetComponent<S_PlayerBall>().isMoving)
        {
            dragStartPosition = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!playerBall.GetComponent<S_PlayerBall>().isMoving)
        {
            dragEndPosition = eventData.position;
            playerBall.GetComponent<S_PlayerBall>().SetDragVector(dragEndPosition - dragStartPosition);
        }

        //hiding handle for now
        gameObject.SetActive(false);
    }

    public void EnableHandle()
    {
        UpdatePosition();
        gameObject.SetActive(true);
    }

    public void UpdatePosition()
    {
        //positioning handle over ball
        Vector3 ballScreenPos = Camera.main.WorldToScreenPoint(playerBall.transform.position);
        transform.position = new Vector3(ballScreenPos.x, ballScreenPos.y + offset, ballScreenPos.z);
    }
}
