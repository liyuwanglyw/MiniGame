using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Animator _animator;
    public string Openlevel;
    public GameObject opendoor;
    public bool _movementAllowed = true;
    int mygold = 20000;
    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_movementAllowed)
            return;



        if (Input.GetKey(KeyCode.A))
            {
                this.GetComponent<Transform>().localRotation = Quaternion.Euler(0, -90, 0);
            _animator.SetBool("Walk", true);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                this.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 90, 0);
            _animator.SetBool("Walk", true);
            }
            else if (Input.GetKey(KeyCode.W))
            {
                this.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("Walk", true);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                this.GetComponent<Transform>().localRotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("Walk", true);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                if (!Openlevel.Equals(""))
                {
                    MapControl.getInstance().StartLevel(Openlevel,mygold ,Over);
                }
            }
            else
            {
                _animator.SetBool("Walk", false);
            }
       
       
    }
    
    private void Walk()
    {
      //  AnimatorOverrideController  anim = Robin.GetComponent<Animator >().runtimeAnimatorController ;
      
        
;
    }

    public void Over()
    {
        GameObject.Find("关卡完成音效 ").GetComponent<AudioSource>().Play();
        GameObject.Find("关卡完成音效2_门打开").GetComponent<AudioSource>().Play();
        Invoke("over2", 0.7f);
       
    }

    void over2()
    {
        Debug.Log("over");
        MapControl.getInstance().HideGame();
        opendoor.GetComponent<Doorcontrol>().isdooropen = true;
        Openlevel = null;
    }
}
