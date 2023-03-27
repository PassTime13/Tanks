using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    //the tiem in seconds before th eshell is removed
    public float m_MaxLifeTime = 2f;
    //theamount of damagedone if the explosion is centred on a tank
    public float m_MaxDamage = 34f;
    //the maximum distance away from the explosion tanks can be and still be affected
    public float m_ExplosionRadius = 5;
    //the amount of force added to a tank at the centre of the explosion
    public float m_ExplosionForce = 100f;

    //reference to the particles that will play on explosion
    public ParticleSystem m_ExplosionParticles;

    
    
    // Start is called before the first frame update
    private void Start()
    {
        //ensure no stray shells
        Destroy(gameObject, m_MaxLifeTime);

    }


    private void OnCollisionEnter(Collision other)
    {
        //find the rigidbody of the collision object
        Rigidbody targetRigidbody = other.gameObject.GetComponent<Rigidbody>();

        //only tanks will have rigid body script
        if (targetRigidbody != null)
        {
            //add an explosion force
            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);

            //find the tankhealth script associated with the rigidbody
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

            if (targetHealth != null)
            {
                //calculate the amount of damage that the target should take based on its distance from the shell.
                float damage = CalculateDamage(targetRigidbody.position);

                //Deal this damage to the tank
                targetHealth.TakeDamage(damage);
                
            }
            
        }

        //Unparent the particles from the shell
        m_ExplosionParticles.transform.parent = null;

        //Play the particle system
        m_ExplosionParticles.Play();

        //once the particles have finished, destroy the gameobject they are on
        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

        //Destory the shell
        Destroy(gameObject);

    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        //create a vector from the shell to the target
        Vector3 explosionToTarget = targetPosition - transform.position;

        //calculate the distance from the shell to teh target
        float explosionDistance = explosionToTarget.magnitude;

        //calculate the proportion of the maximum distance (the explosionRadius) the target is away
        float relativeDistance = 
            (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        //CalculateDamage damage as this proportion of the maximum possible damage
        float damage = relativeDistance * m_MaxDamage;

        //make sure that the minimum damage is always 0
        damage = Mathf.Max(0f, damage);

        return damage;
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
