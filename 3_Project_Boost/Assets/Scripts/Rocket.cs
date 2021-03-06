﻿using UnityEngine;
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
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip levelLoad;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem levelLoadParticles;

    [SerializeField] float levelLoadDelay = 2f;

    State state = State.Alive;

    bool collisionDetect = true;
    

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
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondToDebugInput();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        if (collisionDetect)
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
                    StartSuccessSequence();
                    break;
                default:
                    StartDeathSequence();
                    break;
            }
        }
    }

   private void StartSuccessSequence()
   {
        state = State.Transceding;
        audio.Stop();
        mainEngineParticles.Stop();
        audio.PlayOneShot(levelLoad);
        levelLoadParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dead;
        audio.Stop();
        mainEngineParticles.Stop();
        audio.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
        }
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audio.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void RespondToRotateInput() 
    {
        rigidBody.angularVelocity = Vector3.zero;

        float rotationThisFrame = rcsThrust * Time.deltaTime;
     
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward*rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.back*rotationThisFrame);
        }
    }

    private void ApplyThrust()
    {
        //can thrust while rotating
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!(audio.isPlaying))
        {
            audio.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToDebugInput()
    {
        if (Input.GetKey(KeyCode.L))
        {
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            collisionDetect = !collisionDetect;
        }
    }
}
