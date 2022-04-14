using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;
using UnityEngine.UI;
// using System.Threading;
// using System.Threading.Tasks;
using System;
// using System.Linq;
// using System.IO;
using UnityEngine.SceneManagement;


public class StartScript : MonoBehaviour
{

    string participantID;
    string day;
    int dayNum;

    public InputField participantIDInputField;
    GameObject participantIDGO;
    public InputField dayInputField;
    GameObject dayGO;
    public Button startButton;
    GameObject startButtonGO;
    public Text text;
    GameObject textGO;

    bool keyCheck = false;
    AsyncOperation asyncOperation;


    // Start is called before the first frame update
    void Start()
    {
      text.fontSize = 26;
      participantIDGO = GameObject.Find("ParticipantID");
      dayGO = GameObject.Find("Day");
      startButtonGO = GameObject.Find("StartButton");
      textGO = GameObject.Find("mainText");

      textGO.SetActive(false);

      //Begin to load the Scene you specify
      asyncOperation = SceneManager.LoadSceneAsync("FreeRecallScene");
      //Don't let the Scene activate until you allow it to
      asyncOperation.allowSceneActivation = false;

    }

    // Update is called once per frame
    void Update()
    {
      startButton.onClick.AddListener(startTask);
      if(Input.GetKeyDown("y")){
        keyCheck = true;
      }

      //FIX THIS

      // else if(Input.GetKeyDown("n")){
      //   //Go back to entering info
      //   participantIDGO.SetActive(true);
      //   dayGO.SetActive(true);
      //   startButtonGO.SetActive(true);
      //   textGO.SetActive(false);
      // }
    }

    private void startTask()
    {
      participantID = participantIDInputField.text.Trim();
      day = dayInputField.text.Trim();
      if(participantID == "" || day == "")
      {
        StartCoroutine(failedInput());
      }else if(!IsAllDigits(day)){
        StartCoroutine(failedInput());
      }else{
        dayNum = int.Parse(day);
        StartCoroutine(showInput());
      }
    }
    IEnumerator failedInput()
    {

      participantIDGO.SetActive(false);
      dayGO.SetActive(false);
      startButtonGO.SetActive(false);
      textGO.SetActive(true);
      text.text = "You did not enter a correct participant ID or day number.\n\nEnsure you enter both values correctly.";
      yield return new WaitForSeconds(4f);
      participantIDGO.SetActive(true);
      dayGO.SetActive(true);
      startButtonGO.SetActive(true);
      textGO.SetActive(false);

    }
    IEnumerator showInput()
    {
      participantIDGO.SetActive(false);
      dayGO.SetActive(false);
      startButtonGO.SetActive(false);
      textGO.SetActive(true);
      text.text = "These are your entered values:\n\nParticipant ID: " + participantID + "\n\nDay: " + day + "\n\nIs this correct? Enter y or n";
      // yield return new WaitUntil(() => Input.GetKeyDown("y") || Input.GetKeyDown("n"));
      yield return new WaitUntil(() => keyCheck);
      if(keyCheck){
        //Progress to task
        Debug.Log("This is working");
        PlayerPrefs.SetString("participantID", participantID);
        PlayerPrefs.SetInt("day", dayNum);
        yield return new WaitForSeconds(1.0f);
        // UnityEngine.SceneManagement.SceneManager.LoadScene("FreeRecallScene",LoadSceneMode.Single);
        asyncOperation.allowSceneActivation = true;
      }else if(Input.GetKeyDown("n")){
        //Go back to entering info
        participantIDGO.SetActive(true);
        dayGO.SetActive(true);
        startButtonGO.SetActive(true);
        textGO.SetActive(false);
      }


    }
    bool IsAllDigits(string s)
    {
      foreach (char c in s)
      {
          if (!char.IsDigit(c))
              return false;
      }
      return true;
    }
}
