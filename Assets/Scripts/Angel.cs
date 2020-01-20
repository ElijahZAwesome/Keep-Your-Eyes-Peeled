using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Angel : MonoBehaviour
{

    public GameObject player;
    public GameObject body;
    public bool active = true;
    [SerializeField]
    private bool seen;
    private NavMeshAgent agent;
    private float origSpeed;
    private PlayerScript playerScript;
    private Renderer myRenderer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
        origSpeed = agent.speed;
        InvokeRepeating("AngelUpdate", 0, 0.016666f);
        playerScript = player.GetComponent<PlayerScript>();
        myRenderer = body.GetComponent<Renderer>();
    }

    public bool CheckSeen()
    {
        bool isSeen = false;
        // Check if seen
        if (playerScript.lEye.state == Eye.EyeState.inHead && playerScript.rEye.state == Eye.EyeState.inHead)
        {
            if(myRenderer.IsVisibleFrom(playerScript.mainCam))
            {
                isSeen = true;
                seen = isSeen;
                return isSeen;
            } else
            {
                isSeen = false;
                seen = isSeen;
                return isSeen;
            }
        } else if(playerScript.lEye.state == Eye.EyeState.inHand && playerScript.rEye.state == Eye.EyeState.inHand)
        {
            isSeen = false;
            seen = isSeen;
            return isSeen;
        } else
        {
            if(myRenderer.IsVisibleFrom(playerScript.lEye.eyeCam))
            {
                if(playerScript.lEye.state == Eye.EyeState.inHead || playerScript.lEye.state == Eye.EyeState.attached)
                {
                    isSeen = true;
                }
            }
            if (GetComponent<Renderer>().IsVisibleFrom(playerScript.rEye.eyeCam))
            {
                if (playerScript.rEye.state == Eye.EyeState.inHead || playerScript.rEye.state == Eye.EyeState.attached)
                {
                    isSeen = true;
                }
            }
        }
        seen = isSeen;
        return isSeen;
    }

    void AngelUpdate()
    {
        if (active)
        {
            CheckSeen();
            if (seen)
            {
                agent.speed = 0.0f;
            } else
            {
                agent.speed = origSpeed;
            }
            agent.destination = player.transform.position;
        }
    }
}