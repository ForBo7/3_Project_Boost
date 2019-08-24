using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 1.5f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelComplete;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftRCSParticles;
    [SerializeField] ParticleSystem rightRCSParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem levelCompleteParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionsDisabled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
        

        if (!isTransitioning)
        {
            ThrustSequence();
            Rotate();

        }
        
        //Camera.main.transform.position = this.transform.position + (Vector3.forward * -10f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collisionsDisabled)
        {
            return; // this stops everything beneath from executing/quits the function
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log(collision.gameObject.tag);
                break;
            case "Finish":
                StartLevelCompleteSequence(collision);
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartLevelCompleteSequence(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        isTransitioning = true;
        audioSource.Stop();
        mainEngineParticles.Stop();
        levelCompleteParticles.Play();
        audioSource.PlayOneShot(levelComplete);
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        mainEngineParticles.Stop();
        deathParticles.Play();
        audioSource.PlayOneShot(deathSound);
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
        audioSource.PlayOneShot(levelComplete);
        Debug.Log("Next level loaded");
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("First level loaded");
    }

    private void ThrustSequence()
    {
        if (Input.GetKey(KeyCode.W)) // 'W' has its own if statement so that turning while thrusting can happen
        {
            ApplyThrust();
        }
        else
        {
            StopApplyingThrust();
        }
    }

    private void StopApplyingThrust()
    {
        mainEngineParticles.Stop();
        audioSource.Stop();
    }

    private void ApplyThrust()
    {
        Thrusting();
        PlayThrustAudio();
    }

    private void Thrusting()
    {
        float flightThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * flightThisFrame);
        mainEngineParticles.Play();
    }

    private void PlayThrustAudio()
    {
        if (!audioSource.isPlaying) // So that multiple instances of the audio don't start playing.
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rcsThrust * Time.deltaTime);
        }
    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true; // take manual control of rotation
        transform.Rotate(Vector3.forward * rotationThisFrame);
        //rightRCSParticles.Play();
        rigidBody.freezeRotation = false; // resume physics control of rotation
    }

    // Debug/Developer tools

    private void DebugKeys()
    {
        InstantAdvanceToNextLevel();
        HitBoxToggle();
    }

    private void InstantAdvanceToNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    private void HitBoxToggle()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled; // a simple toggle
        }
    }

}















//public class Rocket : MonoBehaviour
//{
//    [SerializeField] float rcsThrust = 100f;
//    [SerializeField] float mainThrust = 100f;

//    Rigidbody rigidBody;
//    AudioSource audioSource;

//    enum State { Alive, Dying, Transcending };
//    State state = State.Alive;

//    // Start is called before the first frame update
//    void Start()
//    {
//        rigidBody = GetComponent<Rigidbody>();
//        audioSource = GetComponent<AudioSource>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        Thrust();
//        Rotate();
//        //Camera.main.transform.position = this.transform.position + (Vector3.forward * -10f);
//    }

//    void OnCollisionEnter(Collision collision)
//    {
//        switch (collision.gameObject.tag)
//        {
//            case "Friendly":
//                Debug.Log(collision.gameObject.tag);
//                break;
//            case "Finish":
//                Debug.Log(collision.gameObject.tag);
//                state = State.Transcending;
//                Invoke("LoadNextLevel", 20f);
//                break;
//            default:
//                SceneManager.LoadScene(0);
//                break;
//        }
//    }

//    private void LoadNextLevel()
//    {
//        SceneManager.LoadScene(1); // todo allow for more than 2 levels
//        Debug.Log("Next level loaded");
//    }

//    private void Thrust()
//    {
//        float flightThisFrame = mainThrust * Time.deltaTime;

//        if (Input.GetKey(KeyCode.W)) // 'W' has its own if statement so that turning while thrusting can happen
//        {
//            rigidBody.AddRelativeForce(Vector3.up * flightThisFrame);
//            if (!audioSource.isPlaying) // So that multiple instances of the audio don't start playing.
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

