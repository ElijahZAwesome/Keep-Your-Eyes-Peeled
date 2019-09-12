using UnityEngine;
using UnityEngine.AI;

public class Angel : MonoBehaviour
{

    public GameObject player;
    public bool active = true;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
        InvokeRepeating("AngelUpdate", 0, 0.016666f);
        Time.timeScale = 0.6f;
    }

    void AngelUpdate()
    {
        if (active)
        {
            agent.destination = player.transform.position;
        }
    }
}