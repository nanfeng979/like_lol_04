using UnityEngine;

public abstract class LOLObject : MonoBehaviour
{
    public int ID { get; private set; }
    public string Name { get; protected set; }

    public LOLObject(int id, string name)
    {
        ID = id;
        Name = name;
    }

    public LOLObject()
    {
        ID = -1;
        Name = "DefaultLOLObject";
    }

    protected virtual void Start() { }
    
    protected virtual void Update() { }
}
