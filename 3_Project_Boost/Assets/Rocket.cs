using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audio;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    // Start is called before the first frame update
    void Start() 
    {
        rigidBody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() 
    {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("You're OK!");
                break;
            case "Fuel":
                print("You've gained fuel!");
                break;
            case "Finish":
                print("Finished Level");
                SceneManager.LoadScene(1);
                break;
            default:
                print("You dead boi");
                SceneManager.LoadScene(0);
                break;
        }
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
