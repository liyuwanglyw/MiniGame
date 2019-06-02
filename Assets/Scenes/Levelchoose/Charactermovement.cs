using UnityEngine;
using System.Collections;
using System.Linq;
public class Charactermovement : MonoBehaviour
{
    private Transform mmTransform;
    private Rigidbody mmRigidbody;
    //属性

    //开始事件 Awake(),Start()

    void Start()
    {
        //获取自身 Transform组件和Rigidbody组件的引用
        mmTransform = gameObject.GetComponent<Transform>();
        mmRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    //更新事件，Update(),FixUpdate
    void Update()
    {
        PlayerMove();
    }
    //方法

    private void PlayerMove()
    {
        //使用系统预设的w,a,s,d 控制Cube移动
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        //刚体移动的特点：物体的位置+方向，太快就方向*一个小数，使之慢一点
        mmRigidbody.MovePosition(mmTransform.position + dir * 0.2f);
    }

}