//Parth Talwar                                                                                                                                                                                                                  ID: 2220145

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Security.Authentication.ExtendedProtection;
using System.Globalization;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    //Public Fields
    public AudioSource backgroundMusic;
    public Button SkipButton;
    public string Level;
    public TextMeshProUGUI textComponent;
    [TextArea(3, 10)]
    public string[] sentences;
    public float dialogueSpeed;

    //Private Fields
    private int index;
    private bool isTyping = false;

    // Start is called before the first frame update
    void Start()
    {
        backgroundMusic.Play();
        textComponent.text = string.Empty;
        StartDialogue();

        //Skip Button code
        Button SkipBtn = SkipButton.GetComponent<Button>();
        SkipBtn.onClick.AddListener(TaskOnClickSkip);
    }

    void TaskOnClickSkip()
    {
        Debug.Log("Skipping to next scene...");
        SceneManager.LoadScene(Level);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
			if (isTyping)
			{
				//If the text is still typing, complete it immediately.
				CompleteText();
			}
			else if (index < sentences.Length - 1)
			{
				//If there are more sentences, show the next one.
				NextSentence();
			}
			else
			{
				//All sentences are shown, load the level.
				Debug.Log("Going to Level...");
				SceneManager.LoadScene(Level);
			}
		}
    }

    //Task to start the dialogue
    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeSentence());
    }

    //Task to type out the sentences in order
    IEnumerator TypeSentence()
    {
        isTyping = true;
        foreach (char c in sentences[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }
        isTyping = false;
    }

    //Task to load the next sentence or load the next level depending on whether or not all dialogue has been completed
    void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeSentence());
        }
		else
		{
			Debug.Log("Going to Level...");
			SceneManager.LoadScene(Level);
		}
	}

    //Task to stop dialogue box after all texts have gone through
    void CompleteText()
    {
        StopAllCoroutines();
        textComponent.text = sentences[index];
        isTyping = false;
    }
}
