using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonerKol : MonoBehaviour
{
    public float Yon;
    bool Don;
    public void DonmeyeBasla()
    {
        Don=true;
    }
    void Update()
    {
        if (Don)
            transform.Rotate(0, 0, Yon, Space.Self);

    }
}
