using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChoose : BaseScene
{
    private bool IsLeft =false ;
    private bool IsRight = false;
    // Start is called before the first frame update
   protected override void Init()
    {
        base.Init();
    }

    public override void Load(Vector3 init_pos, bool isAsync = true)
    {
        EventManager.getInstance().StartListening("DoorLeft",OrDoorTrigger);
        EventManager.getInstance().StartListening("DoorRight", AndDoorTrigger);
        base.Load(init_pos, isAsync);
    }

    public override void SceneFlow(Operate.Command operate)
    {
        base.SceneFlow(operate);
    }

    public override void Save(string filename, string extend_filename = null)
    {
        base.Save(filename, extend_filename);
    }
    public override void LoadSceneFile(string filename)
    {
        base.LoadSceneFile(filename);
    }
    public override void Close()
    {
        base.Close();
        EventManager.getInstance().StopListening("DoorLeft", OrDoorTrigger);
        EventManager.getInstance().StopListening("DoorRight", AndDoorTrigger);
    }

    public void OrDoorTrigger()
    {
        //开始游戏
        //while the game is over{ 
        IsLeft = true;
        OpenDoor();
        //}
        //changesceneto
    }
    public void AndDoorTrigger()
    {
        //开始游戏
        //while the game is over{ 
        IsRight = true;
        OpenDoor();
        //}
        //changesceneto
    }

    public void OpenDoor()
    {
        if (IsLeft)
        {
            GameObject.FindGameObjectWithTag("LeftDoor").transform.position = new Vector3(0, 0, 1.5f);
        }
        if (IsRight )
        {
            GameObject .FindGameObjectWithTag("RightDoor").transform.position = new Vector3(0, 0, 1.5f);
        }
    }
}
