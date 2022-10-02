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
        //var horizontal = Input.GetAxis("Horizontal") * Speed;
        //var vertical = Input.GetAxis("Vertical") * Speed;


        var horizontal = default(Single);
        if (Input.GetKey(KeyCode.K))
        {
            horizontal = (Speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.H))
        {
            horizontal = -(Speed * Time.deltaTime);
        }

        var vertical = default(Single);
        if (Input.GetKey(KeyCode.U))
        {
            vertical = (Speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.J))
        {
            vertical = -(Speed * Time.deltaTime);
        }

        var yOffset = default(Single);

        if (Input.GetKey(KeyCode.Y))
        {
            yOffset = (Speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.I))
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
