using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class transferbox : MonoBehaviour
    {
        public GameObject disablecamera;
        public GameObject enablecamera;
        public GameObject dest;
        public GameObject robin;
        // Start is called before the first frame update
        void Start()
        {
        robin = GameObject.Find("Robin");
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)                    //OnTriggerStar   刚开始接触就运行下面的命令
        {
            Vector3 telepoint = dest.transform.position;
            robin.GetComponent<CharacterControl>()._movementAllowed = false;
            robin.transform.position = telepoint;
        while (robin.transform.position != telepoint)
        {
            robin.transform.position = telepoint;
            Debug.Log("tran");
        }
            disablecamera.SetActive(false);
            enablecamera.SetActive(true);
            Invoke("getc", 0.5f);
        }

       
        void getc()
        {
            robin.GetComponent<CharacterControl>()._movementAllowed = true;
        }
    }

