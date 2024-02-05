using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ReturnEmail : MonoBehaviour
{
    private Pose _originPoint;
    private Rigidbody rb;
    public float speed;

    private void Awake(){
        _originPoint.position = this.transform.position;
        _originPoint.rotation = this.transform.rotation;
        rb = GetComponent<Rigidbody>();
        Debug.Log("Entered Awake");
    }
    public void ObjectReleased(SelectExitEventArgs arg0)
    {
        rb.Sleep();
        StartCoroutine(MoveToOriginPoint());
        Debug.Log("Entered");
    }

    private IEnumerator MoveToOriginPoint()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < 1.0f)
        {
            transform.position = Vector3.Lerp(startPosition, _originPoint.position, elapsedTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _originPoint.rotation, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        transform.position = _originPoint.position;
        transform.rotation = _originPoint.rotation;

        rb.WakeUp();
    }
}
