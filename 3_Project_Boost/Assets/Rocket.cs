using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audio;
    
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

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        { //can thrust while rotating
            rigidBody.AddRelativeForce(Vector3.up);
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
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back);
        }
        rigidBody.freezeRotation = false;
    }
}
