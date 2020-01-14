using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public Text countText;
    public Text timerText;
    public Text mainScreenText;

    private Rigidbody rb;
    public GameObject gameLight;

    private int count;
    private float remainingTime;
    public float speed;

    private bool isStarted = false;

    private string STR_WELCOME = "Welcome to 'Ryan Nitz' Roll-A-Ball. Push 'S' to Start";
    private string STR_WIN = "YOU WON";
    private string STR_LOSE = "Time's Up! You Lose";
    private string STR_EMPTY = "";
    private string STR_RESTART = "Push 'R' to restart";


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        remainingTime = 30;
        timerText.text = "Time: 30s";
        mainScreenText.text = STR_WELCOME;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
       
        if (Input.GetKeyDown(KeyCode.S))
        {
            isStarted = true;
            gameLight.GetComponent<Light>().intensity = 1;
            mainScreenText.text = STR_EMPTY;
        }

        //duplicated control, but said to have ui/timer handled in update(), wouldn't normally do this.
        if (isStarted == true)
        {
            updateGameTimer();
        }
    }

    void FixedUpdate()
    {
        //disallows movement after game end
        if (isStarted == true)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            toggleEndScreen(STR_WIN);
        }
    }

    void updateGameTimer()
    {
        remainingTime -= Time.deltaTime;
        int remainingSeconds = (int)remainingTime;
        if (remainingSeconds > 0)
        {
            //Debug.Log(remainingSeconds.ToString());
            timerText.text = "Time: " + remainingSeconds.ToString() + "s";
        }
        else
        {
            remainingSeconds = 0;
            toggleEndScreen(STR_LOSE);
        }
        

    }

    void toggleEndScreen(string message)
    {
        isStarted = false;
        rb.velocity = new Vector3(0,0,0);
        gameLight.GetComponent<Light>().intensity = 0;
        mainScreenText.text = message + "\n" + STR_RESTART;
        timerText.text = "Time: 0s";


    }
}