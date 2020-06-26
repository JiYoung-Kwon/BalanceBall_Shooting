using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject Gun;
    public GameObject Bullet;
    public GameObject Aim;
    public Rigidbody GunRB;
    public Text Score;
    public Text timeText;
    public int ScorePoint;
    public int plusScore;
    public Vector3 force;
    public bool isShoot;
    private float time;

    private void Awake()
    {
        time = 5f;
    }

    // Start is called before the first frame update
    void Start()
    {
        GunRB = Gun.GetComponent<Rigidbody>();
        ScorePoint = 0;
        plusScore = 0;
        isShoot = false;
        force = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
    }

    // Update is called once per frame
    void Update()
    {    
        FixShooting();
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        if (Input.GetMouseButtonDown(1) && Camera.main.fieldOfView != 20) //줌인
        {
            Vector3 position = new Vector3(-55f, 5.1f, 6.5f);
            Vector3 rotation = new Vector3(-2f, 180f, 0f);
            Debug.Log("Right Mouse Button 1");
            Camera.main.fieldOfView = 20;
            Gun.transform.LeanMoveLocal(position, 0.3f);           
        }
        else if (Input.GetMouseButtonDown(1) || isShoot == true) //총 쐈을때
        {
            Vector3 position = new Vector3(-56.72f, 4.86f, 5.08f);
            Vector3 rotation = new Vector3(-6.599f, 172.567f, 0.266f);
            Debug.Log("Right Mouse Button 2");
            Camera.main.fieldOfView = 50;
            Gun.transform.LeanMoveLocal(position, 0.3f);

            force = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
            Debug.Log(force.x +" "+ force.y +" "+ force.z);
            isShoot = false;
            time = 5f;
        }

        if (Camera.main.fieldOfView == 20)
        {
            if (time > 0)
                time -= Time.deltaTime;
            else
            {
                isShoot = true;
                Shooting();
            }
            if (Input.GetMouseButtonDown(0))
            {
                ShootBullet();

                Gun.gameObject.GetComponent<Animator>().SetBool("IsShoot", true);
                yield return new WaitForSeconds(0.2f);
                Gun.gameObject.GetComponent<Animator>().SetBool("IsShoot", false);
                yield return new WaitForSeconds(0.4f);
                isShoot = true;
            }
            GunRB.AddForce(force);

            Vector3 pos = Camera.main.WorldToViewportPoint(Gun.transform.position);

            if (pos.x < 0f) pos.x = 0f;

            if (pos.x > 1f) pos.x = 1f;

            //if (pos.y < 0f) pos.y = 0f;

            //if (pos.y > 1f) pos.y = 1f;

            Gun.transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
        else
        {
            GunRB.velocity = Vector3.zero;
            GunRB.angularVelocity = Vector3.zero;
        }

        timeText.text = "시간 : " + Mathf.Ceil(time).ToString();
        Score.text = "점수 : " + ScorePoint + " (+" + plusScore + ")";
    }

    void FixShooting()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            GunRB.AddForce(0f, 0.2f, 0f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            GunRB.AddForce(0f, -0.2f, 0f);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GunRB.AddForce(0.2f, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GunRB.AddForce(-0.2f, 0f, 0f);
        }
    }

    void ShootBullet()
    {
        RaycastHit hit;
        if (Physics.Raycast(Bullet.transform.position, Bullet.transform.forward, out hit))
        {
            Debug.Log(hit.point);
            float ShootPoint = Vector3.Distance(Aim.transform.position, hit.point);
            if (ShootPoint < 0.022f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("10점 입니다.");
                ScorePoint += 10;
                plusScore = 10;
            }
            else if (ShootPoint > 0.022f && ShootPoint < 0.04f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("9점 입니다.");
                ScorePoint += 9;
                plusScore = 9;
            }
            else if (ShootPoint > 0.04f && ShootPoint < 0.06f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("8점 입니다.");
                ScorePoint += 8;
                plusScore = 8;
            }
            else if (ShootPoint > 0.06f && ShootPoint < 0.08f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("7점 입니다.");
                ScorePoint += 7;
                plusScore = 7;
            }
            else if (ShootPoint > 0.08f && ShootPoint < 0.1f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("6점 입니다.");
                ScorePoint += 6;
                plusScore = 6;
            }
            else if (ShootPoint > 0.1f && ShootPoint < 0.12f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("5점 입니다.");
                ScorePoint += 5;
                plusScore = 5;
            }
            else if (ShootPoint > 0.12f && ShootPoint < 0.14f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("4점 입니다.");
                ScorePoint += 4;
                plusScore = 4;
            }
            else if (ShootPoint > 0.14f && ShootPoint < 0.16f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("3점 입니다.");
                ScorePoint += 3;
                plusScore = 3;
            }
            else if (ShootPoint > 0.16f && ShootPoint < 0.18f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("2점 입니다.");
                ScorePoint += 2;
                plusScore = 2;
            }
            else if (ShootPoint > 0.18f && ShootPoint < 0.2f)
            {
                Debug.Log(ShootPoint);
                Debug.Log("1점 입니다.");
                ScorePoint += 1;
                plusScore = 1;
            }
            else
            {
                Debug.Log(ShootPoint);
                Debug.Log("못마춰써");
                plusScore = 0;
            }
            //force = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f);
            //Debug.Log(force);

        }
    }
}
