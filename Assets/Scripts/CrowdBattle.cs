using System.Collections;
using UnityEngine;

public class CrowdBattle : MonoBehaviour
{
    public CrowdManager enemyCrowd;     // Bu çarpışmadaki düşman kalabalığı
    public bool isFinalBattle = false;  // Sadece pist sonundaki final savaşı için işaretleyin
    public float stepDelay = 0.12f;     // Çarpışma sırasında askerlerin eksilme hızı
    public float enemyChargeSpeed = 3f; // Düşmanların oyuncuya doğru yaklaşma hızı

    private bool started = false;

    void Start()
    {
        // Çarpışma çizgisini otomatik olarak düşman ordusunun 4 birim önüne hizala
        if (enemyCrowd != null)
        {
            transform.position = new Vector3(
                0f,
                1.5f,
                enemyCrowd.transform.position.z - 4f
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController[] controllers = other.transform.root.GetComponentsInChildren<PlayerController>();
        CrowdManager playerCrowd = other.transform.root.GetComponentInChildren<CrowdManager>();

        if (playerCrowd == null || controllers.Length == 0 || started) return;
        started = true;

        // Oyuncudaki tüm hareket kontrolcülerini durdur (ayak animasyonları devam eder)
        foreach (PlayerController c in controllers)
        {
            c.canMove = false;
        }

        StartCoroutine(Fight(playerCrowd, controllers, other.transform.root.gameObject));
    }

    IEnumerator Fight(CrowdManager playerCrowd, PlayerController[] controllers, GameObject playerObj)
    {
        int totalPlayer = playerCrowd.Count + 1;

        while (totalPlayer > 0 && enemyCrowd.Count > 0)
        {
            if (Vector3.Distance(enemyCrowd.transform.position, playerObj.transform.position) > 1.8f)
            {
                enemyCrowd.transform.position = Vector3.MoveTowards(
                    enemyCrowd.transform.position,
                    new Vector3(playerObj.transform.position.x, enemyCrowd.transform.position.y, playerObj.transform.position.z),
                    enemyChargeSpeed * stepDelay * 4f
                );
            }

            totalPlayer--;
            enemyCrowd.SetCount(enemyCrowd.Count - 1);

            if (totalPlayer >= 1)
            {
                playerCrowd.SetCount(totalPlayer - 1);
            }

            yield return new WaitForSeconds(stepDelay);
        }

        if (totalPlayer <= 0 && enemyCrowd.Count > 0)
        {
            // Kamera yerinde kalır, yalnızca ana lider modeli gizlenir
            foreach (Transform child in playerObj.transform)
            {
                if (child.GetComponent<Camera>() == null)
                    child.gameObject.SetActive(false);
            }
            Debug.Log("KAYBETTİN! Düşman ordusu seni yendi.");
        }
        else if (isFinalBattle)
        {
            Debug.Log("KAZANDIN! Bölüm tamamlandı - Kalan asker: " + totalPlayer);
        }
        else
        {
            Debug.Log("GEÇTİN! Ara çarpışma kazanıldı - Kalan asker: " + totalPlayer);
            foreach (PlayerController c in controllers)
            {
                c.canMove = true; // İleri koşmaya tekrar devam et
            }
            Destroy(enemyCrowd.gameObject);
            Destroy(gameObject);
        }
    }
}