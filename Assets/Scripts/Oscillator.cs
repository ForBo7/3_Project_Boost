using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor; // 0 for not moved, 1 for fully moved.
    Vector3 startingPosition; // must be stored for absolute movement

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // set movement factor

        if (period >= Mathf.Epsilon)
        {
            float cycles = Time.time / period; // grows continually from 0

            const float tau = Mathf.PI * 2; // about 6.28
            float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

            movementFactor = rawSinWave / 2f + 0.5f;
            Vector3 offset = movementVector * movementFactor;
            transform.position = startingPosition + offset;
        }

        
    }
}

















// todo: Try to add rotation oscillation

//    public class Oscillator : MonoBehaviour
//{
//    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
//    [SerializeField] float period = 2f;

//    float movementFactor; // 0 for not moved, 1 for fully moved.
//    Vector3 startingPosition; // must be stored for absolute movement

//    // Start is called before the first frame update
//    void Start()
//    {
//        startingPosition = transform.position;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // set movement factor

//        float cycles = Time.time / period; // grows continually from 0

//        const float tau = Mathf.PI * 2; // about 6.28
//        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

//        movementFactor = rawSinWave / 2f + 0.5f;
//        Vector3 offset = movementVector * movementFactor;
//        transform.position = startingPosition + offset;
//    }
//}