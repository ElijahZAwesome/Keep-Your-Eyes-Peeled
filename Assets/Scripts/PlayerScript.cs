using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public GameObject leftEye, rightEye, leftMask, rightMask, blackoutImg;
    public Camera mainCam;
    public GameObject gameTexture, lEyeTexture, rEyeTexture;
    public float pickupRadius;
    public GameObject crosshair;
    private Eye lEye;
    private Eye rEye;
    private Vector3 origLPos, origRPos;
    private Quaternion origLRot, origRRot;
    [SerializeField]
    private bool leftEyeNear, rightEyeNear;

    // Start is called before the first frame update
    void Start()
    {
        lEye = leftEye.GetComponent<Eye>();
        rEye = rightEye.GetComponent<Eye>();
        lEye.left = true;
        origLPos = leftEye.transform.localPosition;
        origRPos = rightEye.transform.localPosition;
        origLRot = leftEye.transform.localRotation;
        origRRot = rightEye.transform.localRotation;
        leftEyeNear = false;
        rightEyeNear = false;
        UpdateEyes();
    }

    public Eye.EyeState LeftEyeSate()
    {
        return lEye.state;
    }

    public Eye.EyeState RightEyeSate()
    {
        return rEye.state;
    }

    public void CheckForEyes()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pickupRadius, ~0, QueryTriggerInteraction.Collide);
        List<GameObject> hitEyes = new List<GameObject>();
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.CompareTag("Eye"))
            {
                hitEyes.Add(hitColliders[i].gameObject);
            }
            i++;
        }

        i = 0;
        while (i <= 2)
        {
            if (hitEyes.Contains(leftEye))
            {
                if(lEye.state == Eye.EyeState.attached)
                {
                    leftEyeNear = true;
                }
            } else
            {
                if (lEye.state == Eye.EyeState.attached)
                {
                    leftEyeNear = false;
                }
            }
            if (hitEyes.Contains(rightEye))
            {
                if (rEye.state == Eye.EyeState.attached)
                {
                    rightEyeNear = true;
                }
            }
            else
            {
                if (rEye.state == Eye.EyeState.attached)
                {
                    rightEyeNear = false;
                }
            }
            i++;
        }
    }

    public void UpdateEyes()
    {
        if(rEye.state == Eye.EyeState.inHead && lEye.state == Eye.EyeState.inHead)
        {
            gameTexture.SetActive(true);
            gameTexture.transform.SetAsLastSibling();
            crosshair.transform.SetAsLastSibling();
            return;
        }
        switch (lEye.state)
        {
            case Eye.EyeState.inHead:
                {
                    gameTexture.transform.SetAsFirstSibling();
                    leftMask.SetActive(false);
                    lEyeTexture.SetActive(false);
                    break;
                }
            case Eye.EyeState.inHand:
                {
                    gameTexture.transform.SetAsFirstSibling();
                    leftMask.SetActive(true);
                    lEyeTexture.SetActive(false);
                    break;
                }
            case Eye.EyeState.attached:
                {
                    gameTexture.transform.SetAsFirstSibling();
                    leftMask.SetActive(true);
                    lEyeTexture.SetActive(true);
                    break;
                }
        }

        switch (rEye.state)
        {
            case Eye.EyeState.inHead:
                {
                    gameTexture.transform.SetAsFirstSibling();
                    rightMask.SetActive(false);
                    rEyeTexture.SetActive(false);
                    break;
                }
            case Eye.EyeState.inHand:
                {
                    gameTexture.transform.SetAsFirstSibling();
                    rightMask.SetActive(true);
                    rEyeTexture.SetActive(false);
                    break;
                }
            case Eye.EyeState.attached:
                {
                    gameTexture.transform.SetAsFirstSibling();
                    rightMask.SetActive(true);
                    rEyeTexture.SetActive(true);
                    break;
                }
        }

        crosshair.transform.SetAsLastSibling();
    }

    public void EquipLeftEye()
    {
        switch(lEye.state)
        {
            case Eye.EyeState.inHead:
                {
                    lEye.state = Eye.EyeState.inHand;
                    break;
                }
            case Eye.EyeState.inHand:
                {
                    lEye.state = Eye.EyeState.inHead;
                    break;
                }
            case Eye.EyeState.attached:
                {
                    break;
                }
        }
        UpdateEyes();
    }

    public void EquipRightEye()
    {
        switch (rEye.state)
        {
            case Eye.EyeState.inHead:
                {
                    rEye.state = Eye.EyeState.inHand;
                    break;
                }
            case Eye.EyeState.inHand:
                {
                    rEye.state = Eye.EyeState.inHead;
                    break;
                }
            case Eye.EyeState.attached:
                {
                    break;
                }
        }
        UpdateEyes();
    }

    public void AttachLeftEye()
    {
        switch(lEye.state)
        {
            case Eye.EyeState.attached:
                {
                    CheckForEyes();
                    if(leftEyeNear)
                    {
                        lEye.state = Eye.EyeState.inHand;
                        leftEye.transform.SetParent(mainCam.transform.parent.transform);
                        leftEye.transform.localPosition = origLPos;
                        leftEye.transform.localRotation = origLRot;
                    }
                    break;
                }
            default:
                {
                    RaycastHit hit;
                    Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

                    if (Physics.Raycast(ray, out hit))
                    {
                        Transform objectHit = hit.transform;

                        // Do something with the object that was hit by the raycast.
                        Vector3 newPos = hit.point;
                        Quaternion newRot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                        leftEye.transform.SetParent(null);
                        leftEye.transform.position = newPos;
                        leftEye.transform.rotation = newRot;
                        leftEye.transform.position += leftEye.transform.forward * 0.01f;
                    }
                    lEye.state = Eye.EyeState.attached;
                    break;
                }
        }
        UpdateEyes();
    }

    public void AttachRightEye()
    {
        switch (rEye.state)
        {
            case Eye.EyeState.attached:
                {
                    CheckForEyes();
                    if (rightEyeNear)
                    {
                        rEye.state = Eye.EyeState.inHand;
                        rightEye.transform.SetParent(mainCam.transform.parent.transform);
                        rightEye.transform.localPosition = origRPos;
                        rightEye.transform.localRotation = origRRot;
                    }
                    break;
                }
            default:
                {
                    RaycastHit hit;
                    Ray ray = mainCam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

                    if (Physics.Raycast(ray, out hit))
                    {
                        Transform objectHit = hit.transform;

                        // Do something with the object that was hit by the raycast.
                        Vector3 newPos = hit.point;
                        Quaternion newRot = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                        rightEye.transform.SetParent(null);
                        rightEye.transform.position = newPos;
                        rightEye.transform.rotation = newRot;
                        rightEye.transform.position += rightEye.transform.forward * 0.01f;
                    }
                    rEye.state = Eye.EyeState.attached;
                    break;
                }
        }
        UpdateEyes();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForEyes();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Left Eye button
            EquipLeftEye();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Right eye button
            EquipRightEye();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Left Eye button
            AttachLeftEye();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Right eye button
            AttachRightEye();
        }
    }
}
