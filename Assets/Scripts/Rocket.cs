using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // todo somewhere stop sound on death
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        
        //Camera.main.transform.position = this.transform.position + (Vector3.forward * -10f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return; // this stops everything beneath from executing/ quits the function
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log(collision.gameObject.tag);
                break;
            case "Finish":
                Debug.Log(collision.gameObject.tag);
                state = State.Transcending;
                //Invoke("LoadNextLevel", 2f);
                LoadNextLevel();
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 2f);
                //LoadFirstLevel();
                break;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // todo allow for more than 2 levels
        Debug.Log("Next level loaded");
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
        Debug.Log("First level loaded");
    }

    private void Thrust()
    {
        float flightThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.W)) // 'W' has its own if statement so that turning while thrusting can happen
        {
            rigidBody.AddRelativeForce(Vector3.up * flightThisFrame);
            if (!audioSource.isPlaying) // So that multiple instances of the audio don't start playing.
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control of rotation
    }


}

//public class Rocket : MonoBehaviour
//{

//    // todo fix lighting bug
//    [SerializeField] float rcsThrust = 100f;
//    [SerializeField] float mainThrust = 100f;

//    Rigidbody rigidBody;
//    AudioSource audioSource;

//    enum State { Alive, Dying, Transcending }
//    State state = State.Alive;

//    // Use this for initialization
//    void Start()
//    {
//        rigidBody = GetComponent<Rigidbody>();
//        audioSource = GetComponent<AudioSource>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        // todo somewhere stop sound on death
//        if (state == State.Alive)
//        {
//            Thrust();
//            Rotate();
//        }
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        if (state != State.Alive) { return; } // ignore collisions when dead

//        switch (collision.gameObject.tag)
//        {
//            case "Friendly":
//                // do nothing
//                break;
//            case "Finish":
//                state = State.Transcending;
//                Invoke("LoadNextLevel", 1f); // parameterise time
//                break;
//            default:
//                print("Hit something deadly");
//                state = State.Dying;
//                Invoke("LoadFirstLevel", 1f); // parameterise time
//                break;
//        }
//    }

//    private void LoadNextLevel()
//    {
//        SceneManager.LoadScene(1); // todo allow for more than 2 levels
//    }

//    private void LoadFirstLevel()
//    {
//        SceneManager.LoadScene(0);
//    }

//    private void Thrust()
//    {
//        if (Input.GetKey(KeyCode.W)) // can thrust while rotating
//        {
//            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
//            if (!audioSource.isPlaying) // so it doesn't layer
//            {
//                audioSource.Play();
//            }
//        }
//        else
//        {
//            audioSource.Stop();
//        }
//    }

//    private void Rotate()
//    {
//        rigidBody.freezeRotation = true; // take manual control of rotation

//        float rotationThisFrame = rcsThrust * Time.deltaTime;
//        if (Input.GetKey(KeyCode.A))
//        {
//            transform.Rotate(Vector3.forward * rotationThisFrame);
//        }
//        else if (Input.GetKey(KeyCode.D))
//        {
//            transform.Rotate(-Vector3.forward * rotationThisFrame);
//        }

//        rigidBody.freezeRotation = false; // resume physics control of rotation
//    }
//}

