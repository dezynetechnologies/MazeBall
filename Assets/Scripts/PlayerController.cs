using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float speedAc = 10;
    public Text countText;
    public Text winText;

    private Vector3 zeroAc;
    private Vector3 curAc;
    private float sensH = 10;
    private float sensV = 10;
    private float smooth = 0.5f;

    public Text timerLabel;
    private float time;

    
    private Rigidbody rb;
   
    private  static int count;
    public AudioSource audio1;
    public AudioSource audio2;

    void Start()
    {
        print("start() called");
        rb = GetComponent<Rigidbody>();
        ResetAxis();
        count = 0;
        SetCountText();
        winText.text = " ";
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        audio1 = GetComponent<AudioSource>();      
    }
    void Update()
    {
        time+= Time.deltaTime;

        var minutes = Mathf.FloorToInt(time / 60); //Divide the guiTime by sixty to get the minutes.
        var seconds = time % 60;//Use the euclidean division for the seconds.
        var fraction = (time * 100) % 100;

        timerLabel.text = string.Format("Timer:"+"{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);

        if(time>=39)
        {
            winText.text = "You Lose...!!";
            timerLabel.text = "40 sec over";
            restartCurrentScene();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();


    }

    void FixedUpdate()
    {
        print("FixedUpdate() called");
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * speed);
        }
        else
        {

            curAc = Vector3.Lerp(curAc, Input.acceleration - zeroAc, Time.deltaTime / smooth);

            float moveX = Mathf.Clamp(curAc.x * sensH, -1, 1);

            float moveY = Mathf.Clamp(curAc.y * sensV, -1, 1);

            Vector3 movement = new Vector3(moveX, 0.0f, moveY);
            rb.AddForce(movement * speed * Time.deltaTime);   
        }
    }

  /*  void onCollisionEnter(Collider other) {
        print("OnTriggerEnter() called");
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Goals"))
        {
            Destroy(gameObject);
        }
    }*/

    void OnTriggerEnter(Collider other)
    {
        print("OnTriggerEnter() called");
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            audio1.Play();        
        }
        else if(other.gameObject.CompareTag("Goals"))
        {
            Destroy(gameObject);
        }
    }

   void SetCountText()
    {
        countText.text = "Count:" + count.ToString();
        if (count >= 10)
        {
            audio2 = winText.GetComponent<AudioSource>();
            winText.text = "You Win..!!";
            audio2.Play();          
        }
    }

    void ResetAxis()
    {
        print("ResetAxis() called");
        zeroAc = Input.acceleration;
        curAc = Vector3.zero;
    }

    public void restartCurrentScene()
    {
        if (time >= 45)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}



