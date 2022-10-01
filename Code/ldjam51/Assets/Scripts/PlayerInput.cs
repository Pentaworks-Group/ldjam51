using System;

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

        var yOffset = default(Single);

        if (Input.GetKey(KeyCode.Q))
        {
            yOffset = (Speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            yOffset = -(Speed * Time.deltaTime);
        }

        this.transform.Translate(horizontal, yOffset, vertical, Space.World);

        if (horizontal != 0 || vertical != 0 || yOffset != 0)
        {
            Debug.Log(this.transform.position);
        }
    }
}
