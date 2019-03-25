using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using UnityEngine.EventSystems;

public class PointerClickHandler : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerDownHandler,IPointerExitHandler,IPointerUpHandler
{
    public class my_UnityEvent : UnityEvent<string, PointerEventData> { }
    public static my_UnityEvent m_UnityEvent_Click = new my_UnityEvent(); //声明点击静态的字段
    public static my_UnityEvent m_UnityEvent_Enter = new my_UnityEvent(); //声明进入静态的字段
    public static my_UnityEvent m_UnityEvent_Down = new my_UnityEvent(); //声明进入静态的字段
    public static my_UnityEvent m_UnityEvent_Exit = new my_UnityEvent(); //声明进入静态的字段
    public static my_UnityEvent m_UnityEvent_Up = new my_UnityEvent(); //声明进入静态的字段


    public void OnPointerClick(PointerEventData eventData)               //实现点击接口
    {
        m_UnityEvent_Click.Invoke(gameObject.name,eventData);          //点击时，调用事件
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_UnityEvent_Enter.Invoke(gameObject.name, eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        m_UnityEvent_Down.Invoke(gameObject.name, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_UnityEvent_Exit.Invoke(gameObject.name, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_UnityEvent_Up.Invoke(gameObject.name, eventData);
    }
}
