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
<<<<<<< HEAD
        Vector3 note_pos = pos + new Vector3(-2.7f, -0.5f);
=======
        Vector3 note_pos = pos + new Vector3(-2.5f, -0.5f);
>>>>>>> f5c718f5b3a6b3225076fcfed437a8180bbe26be
        transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, note_pos);
    }
    
}
