
using System.Dynamic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketMovement : MonoBehaviour
{
    Rigidbody body;
    Vector3 moveUp = Vector3.up;
    AudioSource sound;
   
    [SerializeField] float speed = 100f;
    [SerializeField] float thrust = 1000f;

    [SerializeField] AudioClip mainThrust;
    [SerializeField] AudioClip levelChange;
    [SerializeField] AudioClip youDied;

    [SerializeField] ParticleSystem rocketPart;
    [SerializeField] ParticleSystem succesPart;
    [SerializeField] ParticleSystem deadPart;

    enum State { Alive, Dead, Transending, Debug }
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
        {
            StopCollision();
        }
        if (state == State.Alive || state == State.Debug)
        {
            Rotate();
            Thrust();
        }
    }

    //stops collision on debug can do with boolean
    private void StopCollision()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            state = State.Transending;
            LoadNextScene();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (state == State.Alive)
            {
                state = State.Debug;
            }
            else
            {
                state = State.Alive;
            }
        }
    }

    //Rotate Rocket
    private void Rotate()
    {
        body.angularVelocity = Vector3.zero;
        float rotSpeed = speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotSpeed);
        }
    }

    //Rocket movement upwards
    private void Thrust()
    {
        float thrSpeed = thrust * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sound.PlayOneShot(mainThrust);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            body.AddRelativeForce(moveUp * thrSpeed);
            rocketPart.Play();
        }
        else
        {
            sound.Stop();
            rocketPart.Stop();
        }
    }

    //Check collision of the rocket
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;

            case "Finish":
                state = State.Transending;
                Invoke("LoadNextScene", 2f);
                sound.Stop();
                rocketPart.Stop();
                sound.PlayOneShot(levelChange);
                succesPart.Play();
                break;

            default:
                state = State.Dead;
                Invoke("LoadNextScene", 2f);
                sound.Stop();
                sound.PlayOneShot(youDied);
                rocketPart.Stop();
                deadPart.Play();
                break;
        }
    }

    //Level loader
    private void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;
        if (state == State.Transending && currentScene < totalScenes)
        {
            SceneManager.LoadScene( + 1);
            state = State.Alive;
        } else if (state == State.Dead)
        {
            SceneManager.LoadScene(currentScene);
            state = State.Alive;
        }
    }
}
