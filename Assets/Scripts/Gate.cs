using UnityEngine;

public class Gate : MonoBehaviour
{
    public enum Operation { Add, Subtract, Multiply, Divide }

    public Operation operation = Operation.Multiply;
    public int value = 3;

    void OnTriggerEnter(Collider other)
    {
        CrowdManager crowd = other.GetComponent<CrowdManager>();
        if (crowd == null) return;

        int count = crowd.Count;
        switch (operation)
        {
            case Operation.Add: count += value; break;
            case Operation.Subtract: count -= value; break;
            case Operation.Multiply: count *= value; break;
            case Operation.Divide: count /= Mathf.Max(1, value); break;
        }
        crowd.SetCount(count);

        if (transform.parent != null) Destroy(transform.parent.gameObject);
        else Destroy(gameObject);
    }
}