using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    private static LevelManager instance = null;
    private int load_index;
    private string load_name;

    private int m_level_index = 1;
    private int m_max_level = 3;

    private int[] m_scene_table;
    private string[] m_scene_args;

    private AsyncOperation async_op;

    private void Awake()
    {
        instance = this;
        m_scene_table = new int[m_max_level];
        m_scene_args = new string[m_max_level];
        m_scene_table[0] = 0;
        m_scene_table[1] = 1;
        m_scene_table[2] = 2;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static LevelManager getInstance()
    {
        return instance;
    }

    //加载下一场景
    public void loadNext(bool isAsync = false)
    {
        if (m_level_index < m_max_level)
        {
            Debug.Log("Load Scene:" + m_level_index);
            if (!isAsync)
            {
                LoadScene(m_scene_table[m_level_index++]);
            }
            else
            {
                LoadSceneAsync(m_scene_table[m_level_index++]);
            }
        }
    }

    //同步加载场景，加载完成跳转
    public void LoadScene(int index, bool isAsync = false, bool allow_activation = true)
    {
        load_index = index;
        if (isAsync)
        {
            LoadSceneAsync(index, allow_activation);
        }
        else
        {
            SceneManager.LoadScene(index);
            string event_name = string.Format("LoadScene{0}", load_index);
            EventManager.getInstance().TriggerEvent(event_name);
        }
    }

    //同步加载场景，加载完成跳转
    public void LoadScene(string name, bool isAsync = false, bool allow_activation = true)
    {
        load_name = name;
        if (isAsync)
        {
            LoadSceneAsync(name, allow_activation);
        }
        else
        {
            SceneManager.LoadScene(name);
            string event_name = string.Format("LoadScene{0}", load_name);
            EventManager.getInstance().TriggerEvent(event_name);
        }
    }

    //异步加载场景，加载完成后跳转
    public void LoadSceneAsync(int index, bool allow_activation = true)
    {
        load_index = index;
        async_op = SceneManager.LoadSceneAsync(index);
        async_op.allowSceneActivation = allow_activation;
        StartCoroutine(loadingByIndex());
    }

    //异步加载场景，加载完成后跳转
    public void LoadSceneAsync(string name, bool allow_activation = true)
    {
        async_op = SceneManager.LoadSceneAsync(name);
        async_op.allowSceneActivation = allow_activation;
        StartCoroutine(loadingByName());
    }

    private IEnumerator loadingByIndex()
    {
        while (!async_op.isDone)
        {
            yield return null;
        }
        string event_name = string.Format("LoadScene{0}", load_index);
        EventManager.getInstance().TriggerEvent(event_name);
        yield return null;
    }

    private IEnumerator loadingByName()
    {
        while (!async_op.isDone)
        {
            yield return null;
        }
        string event_name = string.Format("LoadScene{0}", load_name);
        EventManager.getInstance().TriggerEvent(event_name);
        yield return null;
    }

    public void AllowActivation()
    {
        async_op.allowSceneActivation = true;
    }

    public int getMaxLevelCount()
    {
        return m_max_level;
    }

    public int getSceneIndex()
    {
        return load_index;
    }

    public string getActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
