using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    public static Respawn Instance;

    public Vector3 checkpointPosition;
    public int checkpointSceneIndex = -1;
    public bool hasCheckpoint = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 position, int sceneIndex)
    {
        checkpointPosition = position;
        checkpointSceneIndex = sceneIndex;
        hasCheckpoint = true;
    }
}
