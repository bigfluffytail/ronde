using System.Collections.Generic;
using UnityEngine;

public class RondeBulletPool : MonoBehaviour
{

    public static RondeBulletPool instance;
    private List<GameObject> pooledBullets = new List<GameObject>();
    [SerializeField] private int bulletAmt = 500;
    [SerializeField] private GameObject bulletPrefab;
    
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    
    void Start()
    {
        for (int i = 0; i < bulletAmt; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            pooledBullets.Add(bullet);
        }
    }

    public GameObject GrabBullet()
    {

        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)
            {
                return pooledBullets[i];
            }
        }

        return null;

    }
}
