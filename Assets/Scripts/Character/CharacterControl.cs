using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    private GameObject Robin = GameObject.Find("Robin");
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Walk();
    }
    
    private void Walk()
    {
        Animation anim = Robin.GetComponent<Animation >();
        AnimationClip walk = anim.GetClip("Run");

        anim.clip = walk;
        anim.Play();
    }
}
