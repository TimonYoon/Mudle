using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum DragType
{

}

public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{


    public DragAndDropType dragType;
    Animator anim;
    private Transform inventoryTr;

    public Item item { get; private set; }
    public Cooking cooking { get; private set; }
    Transform originalTr;

    CanvasGroup canvasGroup;

    public Transform target;

    private void Start()
    {
        if (target == null)
            target = this.transform;

        anim = GetComponent<Animator>();
        item = GetComponent<Item>();
        cooking = GetComponent<Cooking>();

        canvasGroup = target.GetComponent<CanvasGroup>();

        Transform[] tr = target.GetComponentsInParent<Transform>();
        for (int i = 0; i < tr.Length; i++)
        {
            if (tr[i] != target)
            {
                originalTr = tr[i];
                break;
            }                
        }
        inventoryTr = GameObject.Find("Inventory").transform;
    }

    public void OnDrag(PointerEventData eventData)
    {
        target.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition); //.transform.position;// Input.mousePosition);
        //Debug.Log("Point : " + target.position);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        target.SetParent(inventoryTr);
        target.gameObject.SetActive(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 원하는 드랍 포인트가 아닐경우 요리제거 안함
        Drop drop = null; 
        if(eventData.pointerEnter != null)
            drop = eventData.pointerEnter.GetComponent<Drop>();

        if (drop && drop.canDropType == dragType)
        {
            if(dragType == DragAndDropType.Food)
            {
                //Cooking cooking = GetComponent<Cooking>();
                if(cooking)
                {                    
                    drop.OnDrop(cooking);
                    cooking.CookingClear();
                    item.id = "none";
                    item.InitImage();
                }                    
            }
            else
            {
                drop.OnDrop(item);
            }
            
            
        }

        target.SetParent(originalTr);        
        target.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
        if(!cooking)
            target.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (anim)
            anim.SetTrigger("Pressed");
        SoundManager.Play(SoundType.Pickup);
    }
}
