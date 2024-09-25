//Parth Talwar                                                                                                                                                                                                                   ID: 2220145

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Security.Authentication.ExtendedProtection;
using System.Globalization;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    //Fields
    public Button BeginButton;
    public Button InfoButton;
    public Button ControlButton;
    public Button InfoXButton;
    public Button ControlXButton;
    public Button ExitButton;
    public string GameScene;
    public AudioSource backgroundMusic;
    public GameObject InfoScreenPad;
    public GameObject ControlScreenPad;
    public GameObject InfoXBtn;
    public GameObject ControlXbtn;
    public GameObject BlackBackgroundFULL;

    // Start is called before the first frame update
    void Start()
    {
        InfoScreenPad.gameObject.SetActive(false);
        InfoXBtn.gameObject.SetActive(false);
        ControlScreenPad.gameObject.SetActive(false);
        ControlXbtn.gameObject.SetActive(false);
        BlackBackgroundFULL.gameObject.SetActive(false);

        Button Begbtn = BeginButton.GetComponent<Button>();
        Begbtn.onClick.AddListener(TaskOnClickBeg);

        Button Infobtn = InfoButton.GetComponent<Button>();
        Infobtn.onClick.AddListener(TaskOnClickInfo);

        Button XInfobtn = InfoXButton.GetComponent<Button>();
        XInfobtn.onClick.AddListener(TaskOnClickInfoX);

        Button Controlbtn = ControlButton.GetComponent<Button>();
        Controlbtn.onClick.AddListener(TaskOnClickControls);

        Button XControlbtn = ControlXButton.GetComponent<Button>();
        XControlbtn.onClick.AddListener(TaskOnClickControlX);

        Button Exitbtn = ExitButton.GetComponent<Button>();
        Exitbtn.onClick.AddListener(TaskOnClickExit);

        backgroundMusic.Play();
    }

    //Task to begin game (switch scenes to GameScene)
    void TaskOnClickBeg()
    {
        Debug.Log("You have clicked Start Button!");
        SceneManager.LoadScene(GameScene);
    }

    //Task to view Infos
    void TaskOnClickInfo()
    {
        Debug.Log("You have clicked Info Button!");
        InfoScreenPad.gameObject.SetActive(true);
        InfoXBtn.gameObject.SetActive(true);
        BlackBackgroundFULL.gameObject.SetActive(true);
    }

    //Task to go back to title screen (from Infos menu)
    void TaskOnClickInfoX()
    {
        Debug.Log("Going back to menu...");
        InfoScreenPad.gameObject.SetActive(false);
        InfoXBtn.gameObject.SetActive(false);
        BlackBackgroundFULL.gameObject.SetActive(false);
    }

    //Task to view controls
    void TaskOnClickControls()
    {
        Debug.Log("You have clicked Controls Button!");
        ControlScreenPad.gameObject.SetActive(true);
        ControlXbtn.gameObject.SetActive(true);
        BlackBackgroundFULL.gameObject.SetActive(true);
    }

    //Task to go back to title screen (from controls menu)
    void TaskOnClickControlX()
    {
        Debug.Log("Going back to menu...");
        ControlScreenPad.gameObject.SetActive(false);
        ControlXButton.gameObject.SetActive(false);
        BlackBackgroundFULL.gameObject.SetActive(false);
    }

    //Task to exit game
    void TaskOnClickExit()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
