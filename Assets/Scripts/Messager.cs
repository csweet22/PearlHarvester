using Scripts.Utilities;
using UnityEngine;

public class Messager : Singleton<Messager>
{
    
    [SerializeField] private GameObject prefab;
    
    public void Say(string message)
    {
        Transform parent = MainCanvas.Instance.transform;
        GameObject obj = Instantiate(prefab, parent);
        Message msg = obj.GetComponent<Message>();
        msg.SetText(message);
    }
}