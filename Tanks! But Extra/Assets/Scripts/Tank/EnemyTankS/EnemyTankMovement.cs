using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTankMovement : MonoBehaviour
{
    //tank will stop moving towarsds the player once it reaches this distance
    public float m_CloseDistance = 8f;
    //the tanks turret object
    public Transform m_Turret;

    //transform reference to the enemy's spawn
    public Transform m_EnemySpawnPoint;

    //A reference to the player - this will be set when the enemy is loaded
    private GameObject m_Player;
    //A reference to the nav mesh agent component
    private NavMeshAgent m_NavAgent;
    //a reference to the rigid body component
    private Rigidbody m_Rigidbody;

    //list for the transforms and an integer for the current waypoint the enemy tank is following for enemytank idle patrol
    public List<Transform> _waypoints = new List<Transform>();
    private int currentWaypoint;



    //will be set to true when this tank should follow the player
    private bool m_Follow;


    private void Awake()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Follow = false;

    }

    public void OnEnable()
    {
        //when the tank is turned on, make sure it is not kinematic
        m_Rigidbody.isKinematic = false;

        //reset it at the spawn point
        EnemySpawn();

        //ensure they aren't just going to spawn and target straight to the player if the last game ended with follow = true
        m_Follow = false;

        

    }

    private void OnDisable()
    {
        //when the tank is turned off, set the kinematic so it stops moving
        m_Rigidbody.isKinematic = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = true;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            m_Follow = false;

        }

    }

    public void EnemySpawn()
    {
        m_Rigidbody.MovePosition(m_EnemySpawnPoint.position);

    }




    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Follow == false)
        {

            if (_waypoints.Count <= 0)
            {
                return;
                
            }
            
            if (currentWaypoint < _waypoints.Count)
            {
                if (Vector3.Distance(transform.position, _waypoints[currentWaypoint].position) > 5)
                {
                    m_NavAgent.SetDestination(_waypoints[currentWaypoint].position);
                    m_NavAgent.isStopped = false;

                }
               
                else
                {
                    currentWaypoint++;

                }

            }

            else
            {
                currentWaypoint = 0;
                
            }

        }

        else 
        {
            //get distance from player to enemy tank
            float distance = (m_Player.transform.position - transform.position).magnitude;

            //if distance is less than stop distance, then stop moving
            if (distance > m_CloseDistance)
            {
                m_NavAgent.SetDestination(m_Player.transform.position);
                m_NavAgent.isStopped = false;

            }

            //if distance between enemy and player is less than close distance tank will stop and shoot
            else 
            {
                m_NavAgent.isStopped = true;
                
            }

            if (m_Turret != null)
            {
                m_Turret.LookAt(m_Player.transform);
                
            }
            
        }


    }

}
