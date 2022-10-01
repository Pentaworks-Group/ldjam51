using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    const float Speed = 10f;
    const float Rotation = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxis("Horizontal") * Speed;
        var vertical = Input.GetAxis("Vertical") * Speed;

        horizontal *= Time.deltaTime;
        vertical *= Time.deltaTime;

        this.transform.Translate(horizontal, vertical,0 );
    }
}
