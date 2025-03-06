using UnityEngine;
using System.Collections;

public class RondeSpawner : MonoBehaviour
{

    public RondePattern bulletPattern;
    
    private float internalTimer;
    public bool firing;

    void Start()
    {
        
        //SpawnVolley();
        internalTimer = bulletPattern.volleyCooldown;

    }
    
    // Update is called once per frame
    void Update()
    {

        if (firing == true)
        {

            internalTimer += Time.deltaTime;
            if (internalTimer >= bulletPattern.volleyCooldown)
            {

                StartCoroutine(nameof(SpawnVolley));
                internalTimer = 0.0f;

            }

        }
        
    }

    IEnumerator SpawnVolley()
    {

        for (int h = 0; h < bulletPattern.horizontalNum; h++)
        {
            
            for (int v = 0; v < bulletPattern.verticalNum; v++)
            {
                
                Vector3 newSpawnPos;
                float xAlpha;
                float yBeta;

                //We don't need horiz curve if we're at 1
                if (bulletPattern.horizontalNum == 1)
                {

                    xAlpha = 0;

                }
                else
                {
                    
                    xAlpha = Mathf.Lerp(bulletPattern.horizSpreadMin, bulletPattern.horizSpreadMax,
                        (float)h / Mathf.Max(bulletPattern.horizontalNum - 1, 1));
                    
                }
                
                float xRad = (xAlpha - 90) * Mathf.Deg2Rad;

                //We don't need vert curve if we're at 1
                if (bulletPattern.verticalNum == 1)
                {

                    yBeta = 0;

                }
                else
                {
                    
                    yBeta = Mathf.Lerp(bulletPattern.vertSpreadMin, bulletPattern.vertSpreadMax,
                        (float)v / Mathf.Max(bulletPattern.verticalNum - 1, 1));
                    
                }

                float yRad = (yBeta - 180) * Mathf.Deg2Rad;
                
                //Math stuff to determine spawn positions based on angles
                newSpawnPos.x = Mathf.Cos(xRad) * Mathf.Cos(yRad);
                newSpawnPos.y = Mathf.Sin(yRad);
                newSpawnPos.z = Mathf.Sin(xRad) * Mathf.Cos(yRad);



                //If we're firing a set of bullets in a burst (ie two bullet burst, three bullet burst etc)
                if (bulletPattern.bulletBurst > 1)
                {

                    //For each burst bullet in the pattern..
                    //We start at 1 so we don't get offset * 0; better solution required
                    //but I forgot how to map values between -1 and 1 :skull:
                    for (int i = 1; i < bulletPattern.bulletBurst + 1; i++)
                    {

                        //Assign new offset based on defined offset, needs redoing
                        newSpawnPos.x *= bulletPattern.offset.x + (bulletPattern.offset.x * i);
                        newSpawnPos.y *= bulletPattern.offset.y + (bulletPattern.offset.y * i);
                        newSpawnPos.z *= bulletPattern.offset.z + (bulletPattern.offset.z * i);

                        if (bulletPattern.ignoreYMotion)
                        {

                            newSpawnPos = Vector3.zero;
                            newSpawnPos.x = Mathf.Cos(xRad);
                            newSpawnPos.z = Mathf.Sin(xRad);
                            newSpawnPos = -newSpawnPos;

                        }
                        
                        GameObject newBullet = RondeBulletPool.instance.GrabBullet();
                        if (!newBullet)
                            break;
                        
                        newBullet.transform.position = transform.TransformPoint(newSpawnPos);
                        newBullet.transform.rotation = Quaternion.LookRotation(transform.TransformDirection(newSpawnPos));
                        
                        //TODO: Replace with better solution
                        RondeBullet bullRef = newBullet.GetComponent<RondeBullet>();
                        
                        //TODO: Maybe assign the anim graphs here
                        bullRef.dir = bulletPattern.bulletDirection;
                        bullRef.speed = bulletPattern.bulletSpeed;
                        bullRef.bulletLife = bulletPattern.bulletLife;
                        bullRef.emitLocation = transform.position;

                        newBullet.SetActive(true);
                        
                        //burst cooldown
                        yield return new WaitForSeconds(bulletPattern.burstCooldown);

                    }

                }
                else
                {
                    
                    newSpawnPos.x *= bulletPattern.offset.x;
                    newSpawnPos.y *= bulletPattern.offset.y;
                    newSpawnPos.z *= bulletPattern.offset.z;
                    
                    if (bulletPattern.ignoreYMotion)
                    {

                        newSpawnPos = Vector3.zero;
                        newSpawnPos.x = Mathf.Cos(xRad);
                        newSpawnPos.z = Mathf.Sin(xRad);
                        newSpawnPos = -newSpawnPos;

                    }
                    
                    //TODO: Same shit as above, bullet pool + better solutions
                    GameObject newBullet = RondeBulletPool.instance.GrabBullet();

                    if (!newBullet)
                        break;
                    
                    newBullet.transform.position = transform.TransformPoint(newSpawnPos);
                    newBullet.transform.rotation = Quaternion.LookRotation(transform.TransformDirection(newSpawnPos));
                        
                    //TODO: Replace with better solution
                    RondeBullet bullRef = newBullet.GetComponent<RondeBullet>();
                        
                    //TODO: Maybe assign the anim graphs here
                    bullRef.dir = bulletPattern.bulletDirection;
                    bullRef.speed = bulletPattern.bulletSpeed;
                    bullRef.bulletLife = bulletPattern.bulletLife;
                    bullRef.emitLocation = transform.position;

                    newBullet.SetActive(true);
                    
                }

            }
            
        }
        
    }
}
