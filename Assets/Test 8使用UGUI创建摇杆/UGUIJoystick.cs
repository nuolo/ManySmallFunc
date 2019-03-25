using UnityEngine;

using UnityEngine.UI;

using UnityEngine.EventSystems;

using System.Collections;

public class UGUIJoystick : MonoBehaviour, IDragHandler
{

    public RectTransform joystickImage;

    public Vector3 startPos;

    public int maxDis = 70;

    public float speed = 5;

    // Use this for initialization

    void Start()
    {

        startPos = joystickImage.position;

    }

    // Update is called once per frame

    void Update()
    {

        if (Application.platform == RuntimePlatform.Android)

        {

            if (Input.touchCount <= 0)

            {

                if (Vector3.Distance(joystickImage.position, startPos) > 0.01f)

                {

                    joystickImage.position = joystickImage.position - (joystickImage.position - startPos).normalized * speed;

                }

            }

        }

        else

        {

            if (!Input.GetMouseButton(0))

            {

                if (Vector3.Distance(joystickImage.position, startPos) > 0.01f)

                {

                    joystickImage.position = joystickImage.position - (joystickImage.position - startPos).normalized * speed;

                }

            }

        }

    }

    public void OnDrag(PointerEventData eventData)

    {

        Vector3 wordPos;

        //将UGUI的坐标转为世界坐标

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(joystickImage, eventData.position, eventData.pressEventCamera, out wordPos))

            joystickImage.position = wordPos;

        Vector3 dir = (joystickImage.position - startPos).normalized;

        if (Vector3.Distance(joystickImage.position, startPos) >= maxDis)

        {

            joystickImage.position = startPos + dir * maxDis;

        }

    }

}