using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * 사격 콘텐츠 관리 스크립트
 */

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
        time = 5f; //처음 시간 : 5초로 설정
    }

    // Start is called before the first frame update
    void Start()
    {
        GunRB = Gun.GetComponent<Rigidbody>();

        //초기화
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

    IEnumerator Shooting() //사격 효과
    {
        if (Input.GetMouseButtonDown(1) && Camera.main.fieldOfView != 20) //오른쪽 마우스 클릭 시, 줌인
        {
            Vector3 position = new Vector3(-55f, 5.1f, 6.5f);
            Vector3 rotation = new Vector3(-2f, 180f, 0f);
            Debug.Log("Right Mouse Button 1");
            Camera.main.fieldOfView = 20;
            Gun.transform.LeanMoveLocal(position, 0.3f);
        }
        else if (Input.GetMouseButtonDown(1) || isShoot == true) //오른쪽 마우스 다시 클릭 시 || 총 쐈을 시 => 줌 아웃
        {
            Vector3 position = new Vector3(-56.72f, 4.86f, 5.08f);
            Vector3 rotation = new Vector3(-6.599f, 172.567f, 0.266f);
            Debug.Log("Right Mouse Button 2");
            Camera.main.fieldOfView = 50;
            Gun.transform.LeanMoveLocal(position, 0.3f);

            force = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0f); //총을 랜덤 방향으로 이동 시키기 위한 힘
            Debug.Log(force.x + " " + force.y + " " + force.z);
            isShoot = false;
            time = 5f;
        }

        if (Camera.main.fieldOfView == 20) //줌인일 때
        {
            if (time > 0)
                time -= Time.deltaTime; //제한시간 감소
            else
            {
                isShoot = true;
                Shooting();
            }
            if (Input.GetMouseButtonDown(0)) //왼쪽 마우스 클릭 시
            {
                ShootBullet();

                Gun.gameObject.GetComponent<Animator>().SetBool("IsShoot", true);
                yield return new WaitForSeconds(0.2f);
                Gun.gameObject.GetComponent<Animator>().SetBool("IsShoot", false);
                yield return new WaitForSeconds(0.4f);
                isShoot = true;
            }
            GunRB.AddForce(force); //총 랜덤방향으로 이동하게 힘을 가해줌

            Vector3 pos = Camera.main.WorldToViewportPoint(Gun.transform.position);

            if (pos.x < 0f) pos.x = 0f;

            if (pos.x > 1f) pos.x = 1f;

            //if (pos.y < 0f) pos.y = 0f;

            //if (pos.y > 1f) pos.y = 1f;

            Gun.transform.position = Camera.main.ViewportToWorldPoint(pos); //총 이동
        }
        else //줌 아웃
        {
            GunRB.velocity = Vector3.zero; //속도 0
            GunRB.angularVelocity = Vector3.zero; //각속도 0
        }

        //text 표시
        timeText.text = "시간 : " + Mathf.Ceil(time).ToString();
        Score.text = "점수 : " + ScorePoint + " (+" + plusScore + ")";
    }

    void FixShooting() //총 위치 보정하는 거(밸런스볼로 보정 들어갈 것, 현재는 키보드)
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

    void ShootBullet() //총알을 쏨
    {
        RaycastHit hit; //레이캐스트 사용
        if (Physics.Raycast(Bullet.transform.position, Bullet.transform.forward, out hit))
        {
            Debug.Log(hit.point);
            float ShootPoint = Vector3.Distance(Aim.transform.position, hit.point);

            //점수 계산
            if (ShootPoint < 0.022f) //10점 부근에 맞으면
            {
                Debug.Log(ShootPoint);
                Debug.Log("10점 입니다.");
                ScorePoint += 10;
                plusScore = 10;
            }
            else if (ShootPoint > 0.022f && ShootPoint < 0.04f) //9점
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
            else //못맞췄을 경우
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
