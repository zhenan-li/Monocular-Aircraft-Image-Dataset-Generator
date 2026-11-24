using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class parameterInput : MonoBehaviour
{
    public Button startButton;
    public TMP_InputField heightInput;
    public TMP_InputField rangeInput;
    public TMP_InputField spawnSpeed;
    public TMP_InputField spawnNumber;
    public TMP_InputField sizeMutiplier;

    public GameObject cam;//声明相机
    public GameObject canvas;
    private MainScript MainScript;
    private Canvas can;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(OnButtonClick);
        MainScript =  cam.GetComponent<MainScript>();
        can = canvas.GetComponent<Canvas>();
    }

    // Update is called once per frame
    public void OnButtonClick()
    {
        // 读取每个输入框的值
        if (!int.TryParse(heightInput.text, out int height) || height <= 0 ||
            !int.TryParse(rangeInput.text, out int range) || range <= 0 ||
            !int.TryParse(spawnSpeed.text, out int interval) || interval <= 0 ||
            !int.TryParse(spawnNumber.text, out int count) || count <= 0 ||
            !int.TryParse(sizeMutiplier.text, out int multiplier) || multiplier <= 0)
        {
            Debug.LogWarning("Enter a positive integer in each generation field.");
            return;
        }

        MainScript.aircraftHeight = height;
        MainScript.aircraftRange = range;
        MainScript.spawnSpeed = interval;
        MainScript.spawnNumber = count;
        MainScript.sizeMutiplier = multiplier;
        MainScript.enabled = true;
        can.enabled = false;
    }

}
