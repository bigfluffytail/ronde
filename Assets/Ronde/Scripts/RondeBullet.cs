using System.Collections;
using UnityEngine;

public class RondeBullet : MonoBehaviour
{

    public Vector3 dir;
    public float speed;
    public Rigidbody rbRef;

    public float bulletLife;

    //Failsafe for bullets not going back to the pool
    private float maxDist = 50;

    public Vector3 emitLocation;
    
    void OnEnable()
    {
        
        MoveProjectile();
        StartCoroutine(nameof(WaitForBullet));
        InvokeRepeating(nameof(DistCheck), 0.3f, 0.3f);

    }

    void DistCheck()
    {

        if (Vector3.Distance(emitLocation, transform.position) > maxDist)
        {
            
            RevokeBullet();
            
        }

    }
    
    void MoveProjectile()
    {
        
        rbRef.AddRelativeForce(dir * speed, ForceMode.Impulse);
        
    }

    IEnumerator WaitForBullet()
    {

        yield return new WaitForSeconds(bulletLife);
        RevokeBullet();


    }

    void RevokeBullet()
    {

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        rbRef.angularVelocity = Vector3.zero;
        rbRef.linearVelocity = Vector3.zero;
        gameObject.SetActive(false);
        
    }

    void OnTriggerEnter(Collider coll)
    {
        
        RevokeBullet();
        
    }

}
