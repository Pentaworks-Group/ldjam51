using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class hello : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var resource = GameFrame.Base.Resources.Manager.Audio.Get("test");
        Debug.Log(@"Test");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
