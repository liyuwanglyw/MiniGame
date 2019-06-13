using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ModuleNote : MonoBehaviour
{
    public static ModuleNote instance;

    private Text text;
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNote(string note)
    {
        text.text = note;

    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos + new Vector3(-146, -20);
    }
    
}
