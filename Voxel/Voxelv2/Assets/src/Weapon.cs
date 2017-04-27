using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {

    public enum WeaponType
    {
        None,
        FlameCannon,
        Akimbo,
    }

    protected int damage = 0;
    protected int explosionsize = -1;
    protected float charge = 0;
    protected float firerate = 0;
    protected float timeUntilFire = 0;

    protected Transform player = null;
    protected WeaponType type = WeaponType.None;

    

	// Use this for initialization
	void Start () {
        player = transform.parent;
	}

    void Update()
    {
        if (timeUntilFire >= 0) timeUntilFire -= Time.deltaTime;
    }

    protected virtual bool CanFire()
    {

        return timeUntilFire < 0;
    }
    protected abstract void ShootPrimary();
    protected abstract void ShootSecondary();

    

    //Use this when input is pressed to check weapon status and fire the weapon if ready
    public virtual void TriggerPrimaryFire()
    {
        if (CanFire())
        {
            ShootPrimary();
            timeUntilFire = 1 / firerate;
        }
    }
    public virtual void TriggerSecondaryFire()
    {
        if (CanFire())
        {
            ShootSecondary();
            timeUntilFire = 1 / firerate;
        }
    }



    //Accessors
    public void SetDamage(int _damage) { damage = _damage; }
    public void SetFireRate(float _firerate) { firerate = _firerate; }
    public void SetExplosionSize(int _explosionsize) { explosionsize = _explosionsize; }

    public int GetDamage() { return damage; }
    public float GetFireRate() { return firerate; }
    public int GetExplosionSize() { return explosionsize; }
    public float GetCharge() { return charge; }

    public void ResetCharge() { charge = 0; }
    public void ChargeWeapon() { if (CanFire()) charge += Time.deltaTime; }
}
