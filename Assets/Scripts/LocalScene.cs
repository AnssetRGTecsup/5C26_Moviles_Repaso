using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenesManager;

public class LocalScene : MonoBehaviour
{
    [SerializeField] private SceneConfiguration LocalConfiguration;

    [ContextMenu("Enable GameObjects")]
    public void EnableGameObjects()
    {
        LocalConfiguration.EnableGameObjects(this.gameObject);
    }

    [ContextMenu("Disable GameObjects")]
    public void DisableGameObjects()
    {
        LocalConfiguration.DisableGameObjects(this.gameObject);
    }
}
