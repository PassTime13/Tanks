using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour
{
    //prefab of the shell
    public Rigidbody m_Shell;
    //A child of the tank where the shells are spawned
    public Transform m_FireTransform;
    //the force given to the shell when firing
    public float m_LaunchForce = 30f;

    
   
    private void Start()
    {
        

    }

    // Update is called once per frame
    private void Update()
    {
        //TODO put the game managert here to make sur egame isnt over

        if (Input.GetButtonUp("Fire1"))
        {
            Fire();

        }

    }

    private void Fire()
    {
        //Create an instance of the shell and store a reference to its rigidbody
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        //set the shells velocity to the launch force in the fire positions forward direction
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward;
        
    }


}
