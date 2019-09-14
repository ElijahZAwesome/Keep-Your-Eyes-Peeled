using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{

    public enum EyeState
    {
        inHead = 0,
        inHand = 1,
        attached = 2,
    }

    public EyeState state;
    public Camera eyeCam;
    public bool left = false;

    // Start is called before the first frame update
    void Start()
    {
        state = EyeState.inHead;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
