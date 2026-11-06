using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public GameObject followerPrefab;
    public int startCount = 10;
    public float spacing = 0.6f;

    private readonly List<GameObject> followers = new List<GameObject>();

    public int Count => followers.Count;

    void Start()
    {
        SetCount(startCount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetCount(Count + 10);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetCount(Count * 2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetCount(Count - 5);
    }

    public void SetCount(int target)
    {
        target = Mathf.Max(0, target);

        while (followers.Count < target)
            followers.Add(Instantiate(followerPrefab, transform));

        while (followers.Count > target)
        {
            Destroy(followers[followers.Count - 1]);
            followers.RemoveAt(followers.Count - 1);
        }

        Arrange();
    }

    void Arrange()
    {
        float golden = 137.5f * Mathf.Deg2Rad;
        for (int i = 0; i < followers.Count; i++)
        {
            float r = spacing * Mathf.Sqrt(i + 1);
            float a = i * golden;
            followers[i].transform.localPosition =
                new Vector3(Mathf.Cos(a) * r, 0f, Mathf.Sin(a) * r);
        }
    }
}