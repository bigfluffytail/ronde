using UnityEngine;

[CreateAssetMenu(fileName = "RondePattern", menuName = "Ronde/Ronde Bullet Pattern")]
public class RondePattern : ScriptableObject
{

    public enum BulletTypes { Standard, Aimed, Homing }

    public BulletTypes currentType;

    [Header("Horizontal Volley")]
    public int horizontalNum;
    public float horizSpreadMin;
    public float horizSpreadMax;

    [Header("Vertical Volley")]
    public int verticalNum;
    public float vertSpreadMin;
    public float vertSpreadMax;

    [Header("Bullet Burst")]
    public int bulletBurst;
    public float burstCooldown;
    
    [Header("Offset")]
    public Vector3 offset = new Vector3(1,1,1);
    public bool ignoreYMotion;

    [Header("Rate of Fire")]
    public float volleyCooldown;

    [Header("Bullet Info")]
    public Vector3 bulletDirection;
    public float bulletSpeed;
    public float bulletLife;



}
