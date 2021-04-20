using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public bool isDead = false;

    public GameObject deathCamera;
    private GameObject deathPanel;
    public GameObject playerCamera;
    private FirstPersonLook fps;

    private Animator animator;
    private Pause pause;
    private Rigidbody rigidbody;
    protected bool m_HasAudioPlayed;
    private float currentSfxVolume;
    private float[] topScores = new float[3];
    private float scoreOnDeath;
    private string newHighscore;
    private GameObject highScoreInput;

    void Start()
    {
        playerCamera.GetComponent<Camera>().enabled = true;
        deathCamera.GetComponent<Camera>().enabled = false;

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        pause = GetComponent<Pause>();
        deathPanel = GameObject.Find("/DeathPanel");
        highScoreInput = GameObject.Find("/DeathPanel/Main/UserInput");
        playerCamera = GameObject.Find("/Player/First person camera");
        fps = playerCamera.GetComponent<FirstPersonLook>();

        deathPanel.SetActive(false);
        highScoreInput.SetActive(false);
        RigidbodyState(true);
        ColliderState(false);
    }

    public void OnDeath()
    {
        pause.canPause = false;

        Score Score = this.GetComponent<Score>();
        Score.playerDeaths++;

        fps.sensitivity = 0;

        animator.enabled = false;

        RigidbodyState(false);
        ColliderState(true);

        playerCamera.GetComponent<Camera>().enabled = false;
        deathCamera.GetComponent<Camera>().enabled = true;
        deathCamera.transform.position = playerCamera.transform.position;

        isDead = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        deathPanel.SetActive(true);

        topScores[0] = PlayerPrefs.GetFloat("HighScore", 0);
        topScores[1] = PlayerPrefs.GetFloat("SecondScore", 0);
        topScores[2] = PlayerPrefs.GetFloat("ThirdScore", 0);
        scoreOnDeath = PlayerPrefs.GetFloat("CurrentScore", 0);
        newHighscore = PlayerPrefs.GetString("NewHighScore", "False");


        if (scoreOnDeath > topScores[2] && scoreOnDeath < topScores[1])
        {
            highScoreInput.SetActive(true);
            highScoreInput.GetComponent<AddScore>().ButtonFunction();
        }
        if (scoreOnDeath > topScores[1] && scoreOnDeath < topScores[0])
        {
            highScoreInput.SetActive(true);
            highScoreInput.GetComponent<AddScore>().ButtonFunction();
        }
        if (newHighscore == "True")
        {
            highScoreInput.SetActive(true);
            highScoreInput.GetComponent<AddScore>().ButtonFunction();
        }
    }

    public void RigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigid in rigidbodies)
        {
            rigid.isKinematic = state;
        }

        if (state)
        {
            GetComponent<Rigidbody>().isKinematic = !state;
        }
    }

    public void ColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider coll in colliders)
        {
            coll.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene("Level_1");
    }
}
