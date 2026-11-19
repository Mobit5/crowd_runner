using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    public GameObject followerPrefab;
    public int startCount = 0;          // يبدأ بصفر تابعين (القائد فقط)
    public float spacing = 0.45f;       // مسافة قريبة جداً بين الشخصيات
    public float trackHalfWidth = 4.6f; // نصف عرض المضمار المسموح للحشد

    private readonly List<GameObject> followers = new List<GameObject>();

    public int Count => followers.Count;

    void Start()
    {
        SetCount(startCount);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetCount(Count + 10);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetCount(Count - 5);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetCount(Count * 2);
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
        float playerX = transform.position.x;

        // حساب الحدود المسموحة محلياً نسبة لموقع اللاعب الحالي
        float minLocalX = -trackHalfWidth - playerX;
        float maxLocalX = trackHalfWidth - playerX;

        for (int i = 0; i < followers.Count; i++)
        {
            float r = spacing * Mathf.Sqrt(i + 1);
            float a = i * golden;

            float rawX = Mathf.Cos(a) * r;
            float rawZ = Mathf.Sin(a) * r;

            // تقييد المحور X ليبقى داخل حدود المضمار دائماً
            float clampedX = Mathf.Clamp(rawX, minLocalX, maxLocalX);

            followers[i].transform.localPosition = new Vector3(clampedX, 0f, rawZ);
        }
    }

    // تحديث الحدود باستمرار أثناء حركة اللاعب يميناً ويساراً
    void LateUpdate()
    {
        if (followers.Count > 0)
        {
            Arrange();
        }
    }
}