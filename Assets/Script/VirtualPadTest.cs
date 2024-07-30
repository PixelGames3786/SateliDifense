using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPadTest : MonoBehaviour
{
    public FloatingJoystick JoyStick;

    public Rigidbody2D RB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RB.velocity = new Vector2(JoyStick.Horizontal,JoyStick.Vertical);
    }
}
