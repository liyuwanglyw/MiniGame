using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private Animator _animator;
    public string Openlevel;
    public GameObject opendoor;

    // Start is called before the first frame update
    void Start()
    {
        _animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       if(Input .GetKey  (KeyCode.A))
        {
            this.GetComponent<Transform>().localRotation = Quaternion .Euler (0,-90,0);
            _animator.SetBool("Walk", true);
        }
       else if(Input.GetKey(KeyCode.D))
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
            if(Openlevel != null)
            {
                MapControl.getInstance().StartLevel(Openlevel, Over);
            }
        }
        else 
        {
            _animator.SetBool("Walk", false );
        }
       
    }
    
    private void Walk()
    {
      //  AnimatorOverrideController  anim = Robin.GetComponent<Animator >().runtimeAnimatorController ;
      
        
;
    }

    public void Over()
    {
        Debug.Log("over");
        MapControl.getInstance().HideGame();
        opendoor.GetComponent<Doorcontrol>().isdooropen = true;
        Openlevel = null;
    }
}
