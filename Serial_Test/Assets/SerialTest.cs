using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Text;
using System;
using System.IO;

/*
 * BalanceBall 시리얼 통신 test 스크립트
 */

public class SerialTest : MonoBehaviour
{
    public GameObject Cube;

    private SerialPort sp;
    public string message;
    
    byte[] by = new byte[1];
    string[] tempstr;
    int[] data;

    double Gyro_Angle_x = 0, Gyro_Angle_y = 0, Gyro_Angle_z = 0;
    double tmp_angle_x=0, tmp_angle_y=0, tmp_angle_z=0;

    void Start() //포트 연결
    {
        by[0] = 0x02;
        string[] ports = SerialPort.GetPortNames();
        foreach (string p in ports)
        {
            sp = new SerialPort(p, 115200, Parity.None, 8, StopBits.One); // 초기화

            try
            {
                sp.Open(); // 프로그램 시작시 포트 열기

                sp.Write(by, 0, 1); // 0x02 값을 받으면 데이터 출력 시작 --> by 배열 이용
            }
            catch (TimeoutException e) //예외처리
            {
                Debug.Log("timeout");
                continue;
            }
            catch (IOException ex) //예외처리
            {
                Debug.Log("io exception");
                continue;
            }
            Debug.Log("send message");
            Debug.Log(p);
            if (sp.ReadLine().Equals("")) continue;
            else break;
        }
        Debug.Log(sp.ReadLine());

    }

    private void FixedUpdate()
    {
        if (sp.IsOpen) //시리얼 포트가 열려있다면
        {
            string temp = sp.ReadLine(); //값 받아오는 거 (OK)            
            tempstr = temp.Split(','); //자르는 거 (OK)
            data = Array.ConvertAll(tempstr, int.Parse); //int로 변환

            //for (int i = 0; i < data.Length; i++) Debug.Log(i + " = " + data[i]);

            //data[n] 값으로 버튼 클릭 여부를 받아옴
            if (data[0] == 1)
                InputData.isLeftButtonCliked = true; 
            else
                InputData.isLeftButtonCliked = false;

            if (data[1] == 1)
                InputData.isRightButtonCliked = true;
            else
                InputData.isRightButtonCliked = false;

            //calibration
            InputData.Rx = data[2] / 100f;
            InputData.Ry = data[3] / 100f;
            InputData.Rz = data[4] / 100f;

            Cube.transform.rotation = Quaternion.Euler(-InputData.Rx, -InputData.Rz, InputData.Ry); //데이터 값에 맞춰서 Cube 회전

            #region 주석 (기존 -> 하드웨어팀에게 넘겼음)
            //Debug.Log("Left = "+ InputData.isLeftButtonCliked + ", Right = " + InputData.isRightButtonCliked + ", Rx = " + InputData.Rx + ", Ry = " + InputData.Ry + ", Rz = " + InputData.Rz);
            //InputData.Ax = data[2];
            //InputData.Ay = data[3];
            //InputData.Az = data[4];

            //InputData.Gx = data[5];
            //InputData.Gy = data[6];
            //InputData.Gz = data[7];

            //Calc_Rotation();
            #endregion

        }
    }

    private void OnApplicationQuit()
    {
        sp.Close(); // 프로그램 종료시 포트 닫기
    }

    #region 주석(raw data 가공, 상보 필터를 통해 보정 -> 하드웨어팀에게 넘겼음)
    //void Calc_Rotation()
    //{
    //    int ax = data[2], ay = data[3], az = data[4];
    //    int gx = data[5], gy = data[6], gz = data[7];

    //    double dt;
    //    double Accel_Angle_x, Accel_Angle_y;

    //    const double RADIANS_TO_DEGREES = 180 / Math.PI;
    //    const double ALPHA = 0.85;

    //    dt = Time.deltaTime;

    //    double yz = Math.Sqrt(Math.Pow(ay, 2) + Math.Pow(az,2));
    //    Accel_Angle_y = Math.Atan(-ax / yz) * RADIANS_TO_DEGREES * 90f / 77f;


    //    double xz = Math.Sqrt(Math.Pow(ax, 2) + Math.Pow(az, 2));
    //    Accel_Angle_x = Math.Atan(ay / xz) * RADIANS_TO_DEGREES * 90f / 77f;


    //    Gyro_Angle_x += (gx-100) * dt / 100;
    //    //Gyro_Angle_y += gy * dt / 100;
    //    //Gyro_Angle_z += gz * dt / 100;

    //    tmp_angle_x = InputData.Rx + (gx - 100) * dt /100;
    //    tmp_angle_y = InputData.Ry + (gy - 15) * dt / 100;
    //    tmp_angle_z = InputData.Rz + (gz - 47.9) * dt / 100;

    //    InputData.Rx = ALPHA * tmp_angle_x + (1.0 - ALPHA) * Accel_Angle_x;
    //    InputData.Ry = ALPHA * tmp_angle_y + (1.0 - ALPHA) * Accel_Angle_y;
    //    InputData.Rz = tmp_angle_z;

    //    Debug.Log("Rx = "+ InputData.Rx + ", Ry = " + InputData.Ry + " , Rz = " + InputData.Rz) ;
    //}
    #endregion
}



public static class InputData //받아올 Data
{
    public static int Ax, Ay, Az;
    public static int Gx, Gy, Gz;

    public static float Rx, Ry, Rz;
    public static bool isLeftButtonCliked, isRightButtonCliked;
}