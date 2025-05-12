using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public float jumpForce;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;

    public AudioClip jumpSfx;
    public AudioClip crashSfx;
    public AudioClip keepSfx;

    private Rigidbody rb;
    private InputAction jumpAction;
    private bool isOnGround = true;

    private Animator playerAnim;
    private AudioSource playerAudio;

    public bool gameOver = false;

    public int Hp;

    public int CoinCount;
    private bool isCoinDouble = false;

    int jumpCount;

    public TMP_Text HpUI;
    public TMP_Text CoinUI;
    public TMP_Text SpeedUI;


    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        Hp = 100;
        jumpForce = 10;
        CoinCount = 0;
        jumpAction = InputSystem.actions.FindAction("Jump");
        gameOver = false;
        jumpCount = 0;
        UpdateUI("hp");
        UpdateUI("coin");
        UpdateUI("speed");
    }

    void Update()
    {
        if (jumpAction.triggered && jumpCount < 2 && !gameOver)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            jumpCount++;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSfx);

            if (jumpCount == 1)
            {
                isOnGround = false;
            }
        }
    }

    void  OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Bomb"))
        {
            if (gameOver) return;

            Hp -= 50;
            UpdateUI("hp");

            if (Hp <= 0)
            {
                HandleGameOverNoCoroutine();
            }
            else
            {
                if (explosionParticle.isPlaying)
                    explosionParticle.Stop();
                explosionParticle.Play();

                dirtParticle.Stop();

                if (!playerAudio.isPlaying)
                    playerAudio.PlayOneShot(crashSfx);
            }

            ProjectileObjectPool.GetInstance().Return(collision.gameObject);
        }


        if (collision.gameObject.CompareTag("Coin"))
        {
            CoinCount += isCoinDouble ? 2 : 1;
            Destroy(collision.gameObject);
            UpdateUI("coin");
            playerAudio.PlayOneShot(keepSfx);
        }


        if (collision.gameObject.CompareTag("CoinDouble"))
        {
            StartCoroutine(TemporaryCoinDouble(5f));
            Destroy(collision.gameObject);
            playerAudio.PlayOneShot(keepSfx);
        }


        if (collision.gameObject.CompareTag("FirstAid"))
        {
            Hp += 50;
            if (Hp >= 100) {
                Hp = 100;
            }
            Destroy(collision.gameObject);
            UpdateUI("hp");
            playerAudio.PlayOneShot(keepSfx);
        }


        if (collision.gameObject.CompareTag("ItemTime"))
        {
            StartCoroutine(TemporarySpeedBoost(5f, 5f));
            Destroy(collision.gameObject);
            UpdateUI("speed");
            playerAudio.PlayOneShot(keepSfx);
        }

        if (collision.gameObject.CompareTag("TimeReset"))
        {
            MoveSpeed.speed = 5f;
            Destroy(collision.gameObject);
            UpdateUI("speed");
            playerAudio.PlayOneShot(keepSfx);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isOnGround)
            {
                isOnGround = true;
                dirtParticle.Play();
                jumpCount = 0;
            }
        }
    }

    private void HandleGameOverNoCoroutine()
    {
        Hp = 0;
        Debug.Log("Game Over!");
        MoveSpeed.speed = 0f;
        gameOver = true;

        playerAnim.SetBool("Death_b", true);
        playerAnim.SetInteger("DeathType_int", 1);

        if (explosionParticle.isPlaying)
            explosionParticle.Stop();
        explosionParticle.Play();

        dirtParticle.Stop();
        GameResultManager.SaveResult(CoinCount.ToString(), CountdownTimer.currentTimeText);
        Invoke(nameof(LoadEndScene), 2f);

        if (!playerAudio.isPlaying)
            playerAudio.PlayOneShot(crashSfx);
    
    }


    private void LoadEndScene()
    {
        SceneManager.LoadScene("EndGame");
    }

    private IEnumerator TemporarySpeedBoost(float newSpeed, float duration)
    {
        float originalSpeed = MoveSpeed.speed;
        MoveSpeed.speed = newSpeed;
        UpdateUI("speed");

        yield return new WaitForSeconds(duration);

        MoveSpeed.speed = originalSpeed;
        UpdateUI("speed");
    }


    private IEnumerator TemporaryCoinDouble(float duration)
    {
        isCoinDouble = true;
        yield return new WaitForSeconds(duration);
        isCoinDouble = false;
    }
    

    public void UpdateUI(string type)
    {
        if (type == "hp") {
            HpUI.text = Hp.ToString();

        } else if (type == "coin") {
            CoinUI.text = CoinCount.ToString();

        } else if (type == "speed") {
            SpeedUI.text = Mathf.RoundToInt(MoveSpeed.speed).ToString();

        }
    }

}