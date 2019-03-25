using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //获取动画控制器
    private Animator ani;
    //获取遥感脚本
    public EasyTouchMove touch;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //hor = 遥感脚本中的localPosition.x
        float hor = touch.Horizontal;
        //hor = 遥感脚本中的localPosition.y
        float ver = touch.Vertical;

        Vector3 direction = new Vector3(hor, 0, ver);

        if (direction != Vector3.zero)
        {
            //控制移动
            float newSpeed = Mathf.Lerp(ani.GetFloat("Speed"), 3, Time.deltaTime * 5);
            ani.SetFloat("Speed", newSpeed);
            //控制旋转
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
        }
        else
        {
            //停止移动
            float newSpeed = Mathf.Lerp(ani.GetFloat("Speed"), 0, Time.deltaTime * 5);
            ani.SetFloat("Speed", 0);
        }
    }
}
