using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
    public class transferbox : MonoBehaviour
    {
        public GameObject disablecamera;
        public GameObject enablecamera;
        public GameObject dest;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)                    //OnTriggerStar   刚开始接触就运行下面的命令
        {
            Vector3 telepoint = dest.transform.position;
            Debug.Log(telepoint);
            GameObject.Find("Robin").GetComponent<PlayerInput>()._movementAllowed = false;
            GameObject.Find("Robin").transform.position = telepoint;
            disablecamera.SetActive(false);
            enablecamera.SetActive(true);
            Invoke("getc", 0.1f);
        }

        private void OnTriggerStay(Collider other)
        {
            Vector3 telepoint = GameObject.Find("room1to2spawnpoint").transform.position;
            Debug.Log(telepoint);
            GameObject.Find("Robin").transform.position = telepoint;
        }
        void getc()
        {
            GameObject.Find("Robin").GetComponent<PlayerInput>()._movementAllowed = true;
        }
    }
}
