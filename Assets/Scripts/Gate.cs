using UnityEngine;

public class Gate : MonoBehaviour
{
    public enum Operation { Add, Subtract, Multiply, Divide }
    public Operation operation = Operation.Add;
    public int value = 10;

    void OnTriggerEnter(Collider other)
    {
        CrowdManager crowd = other.GetComponent<CrowdManager>();
        if (crowd == null) return;

        // نضيف 1 لأن القائد نفسه يُحسب كلاعب أول في الحشد
        int totalPlayers = crowd.Count + 1;

        switch (operation)
        {
            case Operation.Add:
                totalPlayers += value;
                break;
            case Operation.Subtract:
                totalPlayers -= value;
                break;
            case Operation.Multiply:
                totalPlayers *= value;
                break;
            case Operation.Divide:
                totalPlayers /= Mathf.Max(1, value);
                break;
        }

        // نعيد تعيين عدد التابعين (بطرح القائد الأساسي، وبحد أدنى 0)
        crowd.SetCount(Mathf.Max(0, totalPlayers - 1));

        if (transform.parent != null) Destroy(transform.parent.gameObject);
        else Destroy(gameObject);
    }
}