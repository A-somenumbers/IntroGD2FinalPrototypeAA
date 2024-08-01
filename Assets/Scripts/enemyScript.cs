using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyScript : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatGround;
    public LayerMask whatPlayer;

    //patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkpointRange;

    //attacking
    public float timebtwnAttack;
    bool attacked;
    public GameObject bullet;
    public Transform spawn;
    public float shootForce;

    //states
    public float sightRange, attackRange;
    public bool playerInSight, playerInRange;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }
    private void Patroling()
    {
        if (!walkPointSet) searchWalkPoint();
        if (walkPointSet) agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }
    private void searchWalkPoint()
    {
        float randomZ = Random.Range(-walkpointRange, walkpointRange);
        float randomX = Random.Range(-walkpointRange, walkpointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z+randomZ);
        if (Physics.Raycast(walkPoint,-transform.up,2f,whatGround)) walkPointSet = true; ;
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        
        if (!attacked)
        {
            shoot();
            attacked = true;
            Invoke(nameof(ResetAttack), timebtwnAttack);
        }
    }
    private void shoot()
    {
        GameObject bulletObj = Instantiate(bullet, spawn.transform.position, spawn.transform.rotation) as GameObject;
        Rigidbody bulletRb = bulletObj.GetComponent<Rigidbody>();
        bulletRb.useGravity = false;
        bulletRb.AddForce(bulletRb.transform.forward * shootForce);
        Destroy(bulletObj, timebtwnAttack);
    }
    private void ResetAttack()
    {
        attacked = false;
    }

    // Update is called once per frame
    void Update()
    {
        playerInRange = Physics.CheckSphere(transform.position, attackRange, whatPlayer);

        if (playerInRange) AttackPlayer();
        else Patroling();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("weapon") && Input.GetMouseButton(0))
        {
            Destroy(gameObject);
        }
    }


}
