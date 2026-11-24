using System;
using System.Threading;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
public class MainScript : MonoBehaviour
{

    //作者：netAI-Lzn 2024/11/22

    private System.Random rand = new();
    public GameObject aircraft;//声明飞行器
    private Transform aircraftTransform;
    private BoxCollider aircraftCollider;//声明飞行器的碰撞箱组件
    private bool status=true;//false:不可用，正在保存

    public int aircraftHeight;//飞行器生成高度
    public int aircraftRange;//飞行器生成与相机xy坐标差异范围
    public int spawnSpeed;//生成速度(帧)
    public int sizeMutiplier;//生成图像大小倍数器
    public int spawnNumber;//生成图像数量

    public Camera cam;//声明摄像机
    private Plane[] camViewPlanes;//声明摄像机的视锥

    private int photoNumber = 1;
    private string outputDirectory;
    // Start is called before the first frame update
    void Start()
    {
        string timestamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff");
        outputDirectory = Path.Combine(Application.persistentDataPath, "GeneratedImages", timestamp);
        Directory.CreateDirectory(outputDirectory);
        Debug.Log($"Saving generated images to {outputDirectory}");
        camViewPlanes = GeometryUtility.CalculateFrustumPlanes(cam);
        aircraftCollider =  aircraft.GetComponent<BoxCollider>();
        aircraftTransform =  aircraft.GetComponent<Transform>();
        randomPos(aircraftTransform,aircraftHeight,aircraftRange);

    }

    // Update is called once per frame
    void Update()
    {
        if (photoNumber > spawnNumber)
        {
            enabled = false;
            Application.Quit();
            return;
        }

        if (Time.frameCount % spawnSpeed == 0)
        {
            if(aircraftPosValid(camViewPlanes, aircraftCollider.bounds)==true && status == true){
                string filename = $"test{photoNumber}高度{aircraftHeight}距离{Math.Round(calculateDistance(aircraftTransform))}.png";
                string filePath = Path.Combine(outputDirectory, filename);
                ScreenCapture.CaptureScreenshot(filePath,sizeMutiplier);
                photoNumber++;
                status = false;
            }
            else{
                randomPos(aircraftTransform,aircraftHeight,aircraftRange);
                status = true;
            }

        }
    }

    bool aircraftPosValid(Plane[] planes , Bounds collider){
        //本函数用处：计算飞行器是否在画面内
        //原理:计算飞行器的碰撞箱是否处于摄像机视锥之内
        if (GeometryUtility.TestPlanesAABB(planes, collider))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void randomPos(Transform trans, int height, int rangeSquare){
        //此函数用于随机飞行器的位置。height为高度，rangesquare为相机方圆（以相机为几何中心做的边长为rangesquare的正方形）
        trans.position = new Vector3(rand.Next(rangeSquare), height, rand.Next(rangeSquare));
        trans.rotation = Quaternion.Euler(rand.Next(360), rand.Next(360), trans.eulerAngles.z);
    }

    double calculateDistance(Transform trans){
        double x=trans.position.x;
        double y=trans.position.y;
        double z=trans.position.z;
        x = Math.Pow(x,2.0);
        y = Math.Pow(y,2.0);
        z = Math.Pow(z,2.0);
        return Math.Sqrt(x+y+z);
    }
}
