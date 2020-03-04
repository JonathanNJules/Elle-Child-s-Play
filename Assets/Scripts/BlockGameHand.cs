using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BlockGameHand : MonoBehaviour
{
    public float gripValue;
    public Transform currentBlock;
    private RaycastHit hit;
    public GameObject beam;

    InputDevice rightHand;
    public float gripThreshold = 0.6f;
    public bool rightGripDown, rightGripReset = true;

    void Start()
    {
        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        rightHand = rightHandDevices[0];
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        rightHand.TryGetFeatureValue(CommonUsages.grip, out gripValue);

        if (rightGripDown == true)
            rightGripDown = false;
        if (gripValue > gripThreshold && rightGripReset == true)
        {
            rightGripDown = true;
            rightGripReset = false;
        }
        if (rightGripReset == false && gripValue < gripThreshold)
            rightGripReset = true;

        if (currentBlock != null && gripValue == 0)
            DropCurrentBlock();

        if (rightGripDown == true)
            print("iss tru");

        if (currentBlock != null)
            currentBlock.GetComponent<Rigidbody>().velocity = new Vector3(100, 200, 300);


        bool seeBlock = false;
        Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.transform.CompareTag("Block"))
            {
                seeBlock = true;
                if(rightGripDown == true && currentBlock != hit.transform)
                    ChangeCurrentBlock(hit.transform);
            }
        }

        if (seeBlock && beam.activeSelf == false)
            beam.SetActive(true);
        else if (!seeBlock && beam.activeSelf == true)
            beam.SetActive(false);
    }

    void ChangeCurrentBlock(Transform newBlock)
    {
        if (currentBlock != null)
        {
            currentBlock.GetComponent<Rigidbody>().isKinematic = false;
            currentBlock.parent = null;
        }

        currentBlock = newBlock;
        currentBlock.GetComponent<Rigidbody>().isKinematic = true;
        currentBlock.parent = transform;
    }

    void DropCurrentBlock()
    {
        if (currentBlock != null)
        {
            currentBlock.GetComponent<Rigidbody>().isKinematic = false;
            currentBlock.parent = null;
            currentBlock = null;
        }
    }
}
