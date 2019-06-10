using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.LuisPedroFonseca.ProCamera2D.TopDownShooter
{
    public class transferbox : MonoBehaviour
    {
        public GameObject disablecamera;
        public GameObject enablecamera;
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
            print("你碰着我了");
            Vector3 telepoint = GameObject.Find("room1to2spawnpoint").transform.position;
            Debug.Log(telepoint);
            GameObject.Find("Robin").GetComponent<PlayerInput>()._movementAllowed = false;
            GameObject.Find("Robin").transform.position = telepoint;
            enablecamera.SetActive(true);
            disablecamera.SetActive(false);
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
