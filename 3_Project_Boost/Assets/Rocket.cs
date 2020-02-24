using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    enum State
    {
        Alive,
        Dead,
        Transceding
    };

    Rigidbody rigidBody;
    AudioSource audio;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    State state = State.Alive;
    int level = 0;
    

    // Start is called before the first frame update
    void Start() 
    {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() 
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("You're OK!");
                break;
            case "Fuel":
                print("You've gained fuel!");
                break;
            case "Finish":
                state = State.Transceding;
                print("Finished Level");
                Invoke("LoadNextScene", 1f);
                break;
            default:
                state = State.Dead;
                print("You dead boi");
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadNextScene()
    {
        level++;
        SceneManager.LoadScene(level);
    }

    private void LoadFirstLevel()
    {
        level = 0;
        SceneManager.LoadScene(level);
    }

    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        { //can thrust while rotating
            rigidBody.AddRelativeForce(Vector3.up*thrustThisFrame);
            if (!(audio.isPlaying)) {
                audio.Play();
            }
            else
            {
                audio.Stop();
            }
        }
    }

    private void Rotate() 
    {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;
     
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward*rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back*rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    }
}
