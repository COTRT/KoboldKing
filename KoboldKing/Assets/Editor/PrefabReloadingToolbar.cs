using UnityEngine;
using UnityEditor;
using Assets.Scripts.Events;

public class PrefabReloadingToolbar : ScriptableObject
{
    [MenuItem("Custom Tools/Reload Prefabs")]
    static void SendReloadPrefabsRequest()
    {
        Messenger.Broadcast(ResourceEvent.PREFABS_RELOADING);
    }
}