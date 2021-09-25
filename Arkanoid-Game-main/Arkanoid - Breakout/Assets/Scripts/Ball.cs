using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool inPlay;
    public float randBallPosition;
    public Transform paddleBall;
    public float speed;
    public Transform blueExplosion;
    public Transform greenExplosion;
    public Transform greyExplosion;
    public Transform orangeExplosion;
    public Transform pinkEplosion;
    public Transform purpleExplosion;
    public Transform redExplosion;
    public Transform tirkuazExplosion;
    public Transform yellowExplosion;
    public Transform powerupExtraLife;
    public Transform powerupEnlarge;
    public Transform powerupReduce;
    public Transform powerupSlow;
    public Transform powerCatch;
    public Transform paddleSize;
    public float paddleShift;
    public GameManager gm;   
    public int numberOfEnlargePowerup;
    public int maxNumberOfEnlargePowerup;
    public int numberOfReducePowerup;
    public int maxNumberOfReducePowerup;
    public bool extraLifeLimit = false;
    public bool slowLimit = false;
    public bool catchLimit = false;
    public int catchCount = 0;
    public Paddle paddle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        paddleShift = paddleSize.transform.localScale.x;
        randBallPosition = Random.Range (-paddleShift / 2, paddleShift / 2);
    }

    void Update()
    {
        if (gm.gameOver)
        {
            return;
        }

        if (speed < 500f)
        {
            speed += 10f * Time.deltaTime;
        }

        if (catchCount >= 3)
        {
            paddle.inCatch = false;
        }

        paddleShift = paddleSize.transform.localScale.x;

        numberOfEnlargePowerup = GameObject.FindGameObjectsWithTag ("enlarge").Length;
        numberOfReducePowerup = GameObject.FindGameObjectsWithTag ("reduce").Length;

        if (paddleShift >= 0.3f && paddleShift <= 0.5f)
        {
            maxNumberOfEnlargePowerup = 3;
        }

        else if (paddleShift >= 0.6f && paddleShift <= 0.8f)
        {
            maxNumberOfEnlargePowerup = 2;
        }

        else if (paddleShift >= 1.1f && paddleShift <= 1.3f)
        {
            maxNumberOfEnlargePowerup = 1;
        }

        else
        {
            maxNumberOfEnlargePowerup = 0;
        }


        if (paddleShift >= 0.3f && paddleShift <= 0.5f)
        {
            maxNumberOfReducePowerup = 0;
        }

        else if (paddleShift >= 0.6f && paddleShift <= 0.8f)
        {
            maxNumberOfReducePowerup = 1;
        }

        else if (paddleShift >= 1.1f && paddleShift <= 1.3f)
        {
            maxNumberOfReducePowerup = 2;
        }

        else
        {
            maxNumberOfReducePowerup = 3;
        }
     

        if (!inPlay)
        {
            transform.position = new Vector2 (paddleBall.position.x + randBallPosition, paddleBall.position.y);
        }

        if (Input.GetMouseButtonDown(0) && !inPlay)
        {
            inPlay = true;
            rb.AddForce(Vector2.up * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("bottom"))
        {
            if (!gm.gameOver)
            {
                gm.UpdateLives (-1);    
            }

            if (gm.numberOfBricks <= 0)
            {
                gm.LoadLevel();
            }

            rb.velocity = Vector2.zero;
            
            paddle.paddleChangeSize = 0.7f;
            paddle.transform.localScale = new Vector2 (paddle.paddleChangeSize, 1.5f);

            speed = 500f;

            randBallPosition = Random.Range (-paddleShift / 2, paddleShift / 2);

            inPlay = false;

            catchCount = 3;            
        }
    }

    float hitFactor (Vector2 ballPos, Vector2 paddlePos, float paddleWidth)
    {
        return (ballPos.x - paddlePos.x) / paddleWidth;
    }

    void OnCollisionEnter2D(Collision2D other)
    {                   
        if (other.transform.CompareTag("brick"))
        {
            Brick brickScript = other.gameObject.GetComponent<Brick>();

            if (brickScript.hitsToBreak > 1)
            {
                brickScript.BreakBrick();
            }

            else
            {   
                int randChance = Random.Range (1, 101);
                int randChance02 = Random.Range (1, 3);

                if (numberOfReducePowerup >= maxNumberOfReducePowerup)
                {
                    randChance02 = 1;
                }

                if (numberOfEnlargePowerup >= maxNumberOfEnlargePowerup)
                {
                    randChance02 = 2;
                }

                if (randChance % 33 == 0 && !extraLifeLimit)
                {
                    Instantiate (powerupExtraLife, other.transform.position, other.transform.rotation);
                    extraLifeLimit = true;
                }

                if (randChance % 20 == 0 && randChance02 == 1 && numberOfEnlargePowerup < maxNumberOfEnlargePowerup)
                {
                    Instantiate (powerupEnlarge, other.transform.position, other.transform.rotation);
                }

                if (randChance % 20 == 0  && randChance02 == 2 && numberOfReducePowerup < maxNumberOfReducePowerup)
                {
                    Instantiate (powerupReduce, other.transform.position, other.transform.rotation);
                }

                if (randChance % 24 == 0 && !slowLimit)
                {
                    Instantiate (powerupSlow, other.transform.position, other.transform.rotation);
                    slowLimit = true;
                }

                if (randChance % 1 == 0 && !catchLimit)
                {
                    Instantiate (powerCatch, other.transform.position, other.transform.rotation);
                    catchLimit = true;
                }


                if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Blue Brick")
                {
                    Transform newExplosion = Instantiate (blueExplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                else if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Green Brick")
                {
                    Transform newExplosion = Instantiate (greenExplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                else if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Grey Brick Broken 2")
                {
                    Transform newExplosion = Instantiate (greyExplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                else if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Orange Brick")
                {
                    Transform newExplosion = Instantiate (orangeExplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                else if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Pink Brick")
                {
                    Transform newExplosion = Instantiate (pinkEplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                else if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Purple Brick")
                {
                    Transform newExplosion = Instantiate (purpleExplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                else if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Red Brick")
                {
                    Transform newExplosion = Instantiate (redExplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                else if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Tirkuaz Brick")
                {
                    Transform newExplosion = Instantiate (tirkuazExplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                else if (other.gameObject.GetComponent<SpriteRenderer>().sprite.name == "Yellow Brick")
                {
                    Transform newExplosion = Instantiate (yellowExplosion, other.transform.position, other.transform.rotation);
                    Destroy (newExplosion.gameObject, 1f);
                }

                gm.UpdateScore (brickScript.points);
                gm.UpdateNumberOfBricks();
                Destroy (other.gameObject);
            }   
        }


        if (other.gameObject.CompareTag("paddle") && transform.position.y >= -3.80)
        {
            if (paddle.inCatch)
            {
                inPlay = false;
                catchCount++;
            }     
            
            else
            {
                float x = hitFactor (transform.position, other.transform.position, other.collider.bounds.size.x);
                Vector2 dir = new Vector2 (x, 0.25f).normalized;
                GetComponent<Rigidbody2D>().velocity = (dir) * speed * Time.deltaTime;
            }   
        }
    }
}
    