using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour
{
    [SerializeField] private Transform Target;
    [SerializeField] private Vector3 Target_offset;

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position,Target.position+Target_offset,0.125f);
    }
}
