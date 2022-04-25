using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
// using System.Windows.Forms;

public class FreeRecall : MonoBehaviour
{

  //Participant ID (removed from Public)
  string participantID;
  //Enter what day it is for running this participant (removed from Public)
  int day;

  //How many words do we want to step up WITHIN TRIAL if correct? (was orignially public)
  int wordStepUp;
  //How many words do we want to step down WITHIN TRIAL if not correct? (removed from public)
   int wordStepDown = 1;

   //Deprecated
  // //How many trials before you increase to next amount of words? (removed from public)
  //  int numTrials = 4;
  // //How many words do we want to step up BETWEEN TRIALS after numTrials is reached? (was originally public)
  // int trialWordStepUp;

  //Length of the word list that people see (i.e. 3 at a time, 6 at a time, etc.) (removed from public)
   int lengthOfWordList = 3;
  //Reads the text file for all of the words and stores in an array
  string[] reader;
  //unused words from reader (starts as copy)
  List<String> readerUnused = new List<String>();
  //used words from reader (starts out empty, but after day 1 is the first list participants see)
  List<String> readerUsed = new List<String>();
  //used words for week 2. This is different from readerUsed because readerUsed is their total used word list from the whole 2 weeks.
  List<String> readerWeek2 = new List<String>();
  //random number for shuffling reader list for participant
  private System.Random _random = new System.Random();
  //Array that stores the answers from the participant each time they write them. (Final)
  string[] userAnswers;
  //Array that stores answers from participant BEFORE removal of repeats
  string[] preRemovalUserAnswers;

  //Deprecated
  // //Array that stores answers from participant BEFORE white space is removed (removes ALL white space from string)
  // string[] preWhiteSpace;

  //Value of number of words correct
  int numWordCorrect;
  //What day is it? First day is day 0 (removed from public)
   int wordListValue = 0;

   //Deprecated
  // //determine number of trials on this length of list (Do we want this stored as a saved value?)
  // int numTrialsCount = 0;

  //count how many times a participant gets a certain list wrong
  int numSessionsWrong=0;
  //determine the number of sessions that people will do when they get a list wrong before it decides to move them down
  int numSessions;
  //determine the number of times that someone is in a session with a decreased value
  int numTimeSessionsWrong = 0;
  //Button to submit words from Recall (removed from public)
   Button wordSubmit;
  //GameObject of wordSubmit button (removed from public)
   GameObject wordSubmitButton;
  //Trial number counts the number of times they go through all of the functions. Will reset at the beginning of each day/time you run this program.
  int trials = 1;
  //number to collect the rating of how well participants do at using the autobigraphical method. Self-rated 1-4, will be input via inputField
  int methodRating = 0;
  //input field game object for collecting rating of using autobigraphical method (removed from public)
   GameObject inputFieldMethod;
  //input field variable for self-report method use (removed from public)
   InputField inputMethod;
  //button to submit method ratings (removed from public)
   Button methodSubmit;
  //button gameobject for submitting method ratings (removed from public)
   GameObject methodSubmitButton;
  //boolean to know if submitted method value is not empty
  bool methodFilledBool = false;

  //random num1 for math Tasks
  int num1;
  //random num2 for math tasks
  int num2;
  //String to say if correct answer or not
  string mathCorrect;
  //Convert their input string to integer
  int mathAnswer;
  //button to submit the math tasks (removed from public)
   Button mathSubmit;
  //button gameobject for submitting math tasks (removed from public)
   GameObject mathSubmitButton;
  //boolean to know if button is clicked
  bool mathSubmitBool = false;
  //boolean to know if submitted math value is not empty
  bool mathFilledBool = false;


  //Size of the instruction font (removed from public)
   int instructionFontSize = 28;
  //Size of the font for the words that participants learn and the math (removed from public)
   int wordFontSize = 44;

  //input field game object to edit it being on and off (removed from public)
   GameObject inputField;
  //input field variable to change its unique features (removed from public)
   InputField input;
  //text field to change the text box (removed from public)
  public Text text;
  //input field game object for math problems to turn on and off (removed from public)
   GameObject inputFieldMath;
  //input field variable for the math problems (removed from public)
   InputField inputMath;

  //Text field game GameObject
  GameObject textGO;

  //string for storing the directory to store participant unique shuffled list of words (updated to shuffled list total)
  // string directoryShuffleList;

  //string for the directory to store the participant data (entered math problems and words)
  string directoryData;
  //string for directory to store participant unique shuffled list total
  string directoryShuffledListTotal;
  //string for directory to store participant unique shuffled list unused
  string directoryShuffledListUnused;
  //string for directory to store participant unique shuffled list used
  string directoryShuffledListUsed;
  //string for directory to store participant unique shuffled list used for WEEK 2
  string directoryShuffledListUsed2;
  //string for the file that holds the whole toronto word pool
  string torontoWordPool;

  //boolean to tell if participant is about to step up in words and thus show the necessary message
  bool steppingUp = false;

  //boolean to know if we have already generated a word file and will overwrite if we continue
  bool dejaVu = false;
  //string to store last line of data file and determine if the trial is bigger than 1
  string dejaVuString;
  //string array to store dejaVuString components
  string[] dejaVuArray;

  //boolean to tell if previous word answers were correct
  bool correct = false;

  //integer to keep currect location until it changes when correct
  int currentLocation = 0;
  //integer to keep the last word in the list that is read from the text file. Will be template for first location of new list.
  int lastWord = 0;



    // Start is called before the first frame update
    //All code starts here, currently not using update 03/22/2022
    void Start()
    {
      //Stop any coroutines running
      StopAllCoroutines();

      //Call participantID and day number in from the start scene
      participantID = PlayerPrefs.GetString("participantID");
      day = PlayerPrefs.GetInt("day");

      //Assign all components in script so that build functions
      wordSubmitButton = GameObject.Find("ButtonWords");
      wordSubmit = wordSubmitButton.GetComponent<Button>();
      mathSubmitButton = GameObject.Find("ButtonMath");
      mathSubmit = mathSubmitButton.GetComponent<Button>();
      methodSubmitButton = GameObject.Find("ButtonMethod");
      methodSubmit = methodSubmitButton.GetComponent<Button>();
      inputFieldMethod = GameObject.Find("InputFieldMethod");
      inputMethod = inputFieldMethod.GetComponent<InputField>();
      inputField = GameObject.Find("InputFieldWords");
      input = inputField.GetComponent<InputField>();
      inputFieldMath = GameObject.Find("InputFieldMath");
      inputMath = inputFieldMath.GetComponent<InputField>();
      // textGO = GameObject.Find("Text");
      // text = textGO.GetComponent<Text>();

      //Set the text size and turn off all of the necessary game objects
      text.fontSize = instructionFontSize;
      text.text = "Hello, World!";
      inputField.SetActive(false);
      inputFieldMath.SetActive(false);
      mathSubmitButton.SetActive(false);
      wordSubmitButton.SetActive(false);
      inputFieldMethod.SetActive(false);
      methodSubmitButton.SetActive(false);

      //Create Data folder if not already made (back slashes and periods make it go up a directory)
      string dataFolder = Application.dataPath + "\\..\\/Data";
      Directory.CreateDirectory(dataFolder);

      // //define directory for the shuffled list of the participant (NOT USED AS DATA, gets read out to main project folder for reference)
      // directoryShuffleList = Application.dataPath + "\\..\\Participant_" + participantID + "_Date_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm") +
      // "_ParticipantShuffledList.txt";


      //define directory for shuffled list total (USED FOR DATA)
      directoryShuffledListTotal = Application.dataPath + "\\..\\/Data/Participant_" + participantID + "_ParticipantShuffledListTotal.txt";
      //define directory for participant used list (USED FOR DATA)
      directoryShuffledListUsed = Application.dataPath + "\\..\\/Data/Participant_" + participantID + "_ParticipantShuffledListUsed.txt";
      //define directory for participant used list for WEEK 2 (USED FOR EXPERIMENTER PURPOSES ONLY, NO NEED TO INCLUDE IN DATA)
      directoryShuffledListUsed2 = Application.dataPath + "\\..\\/Data/Participant_" + participantID + "_ParticipantShuffledListUsedWeek2.txt";
      //define directory for participant unused list (USED FOR DATA)
      directoryShuffledListUnused = Application.dataPath + "\\..\\/Data/Participant_" + participantID + "_ParticipantShuffledListUnused.txt";
      //define the directory for storing data (OUTPUTS DATA)
      directoryData = Application.dataPath + "\\..\\/Data/Participant_" + participantID + "_ParticipantData.txt";
      //define the path of the toronto word pool
      torontoWordPool = Application.dataPath + "\\..\\Test.txt";

      Debug.Log("ParticipantID: " + participantID);

      if(day == 1 && trials == 1)
      {
        if(File.Exists(directoryShuffledListTotal))
        {
          dejaVu = true;
        }
      }


      //Call the array of the word list from the stored text File
      callArray();

      //create data directory if it does not exist, else output that it exists
      if(File.Exists(directoryData))
      {
        Debug.Log("The data file exists, will write new lines to it.");
        // using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
        // {
        //   file.WriteLine("ProgramStart");
        //   // file.WriteLine("Correct?: " + mathCorrect);
        // }
      }else
      {
        using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
        {
          file.WriteLine("day;id;trial;timestamp;list_length;methodRating;mp1;mp1_resp;mp1_correct;mp2;mp2_resp;mp2_correct;mp3;mp3_resp;mp3_correct;mp4;mp4_resp;mp4_correct;mp5;mp5_resp;mp5_correct;words;words_response_raw;words_response_list;words_correct;session");

          // file.WriteLine("Participant ID: " + participantID);
        }
      }




      //Sets the length of the word list at start so that if someone does not get all values, it does not keep that minimized value across the whole study.
      if(day == 1 && !dejaVu)
      {
        PlayerPrefs.SetInt("Length of Word List",lengthOfWordList);
      }else if(day < 6){
        lengthOfWordList = readerUsed.Count();
        PlayerPrefs.SetInt("Length of Word List",lengthOfWordList);
      }else if(day == 6 && !dejaVu)
      {
        lengthOfWordList = 3;
        PlayerPrefs.SetInt("Length of Word List",lengthOfWordList);
      }else{
        dejaVuString = File.ReadLines(directoryData).Last();
        dejaVuArray = dejaVuString.Split(new[]{';'}, StringSplitOptions.RemoveEmptyEntries);
        int dejaVuArrayLengthOfWordList = int.Parse(dejaVuArray[4]);
        lengthOfWordList = dejaVuArrayLengthOfWordList;
        PlayerPrefs.SetInt("Length of Word List",lengthOfWordList);
        dejaVu = false;
      }

      // if(day == 1 && trials == 1)
      // {
      //   if(File.Exists(directoryShuffledListTotal))
      //   {
      //     StopAllCoroutines();
      //     StartCoroutine(failTask());
      //   }
      //   StartCoroutine(waitStartTask());
      // }else{
        StartCoroutine(waitStartTask());
      // }


    }

    IEnumerator failTask()
    {
      // Debug.Log("We are getting here");

      text.text = "ERROR: SHUFFLED LIST ALREADY EXISTS, PROGRAM WILL RESTART.\n\nPress the return key to advance.";

      // Debug.Log("Are we getting here");

      yield return new WaitUntil(() => Input.GetKeyDown("return"));

      // Debug.Log("Are we getting here 2");
      Application.Quit();
      // Debug.Log("Are we getting here 3");
    }

    // Update is called once per frame
    void Update()
    {
      mathSubmit.onClick.AddListener(ParameterOnClick);
      wordSubmit.onClick.AddListener(ParameterOnClick);
      methodSubmit.onClick.AddListener(ParameterOnClick);

    }
    private void callArray()
    {
      /*This is where you would enter the Array. The array can be a size of strings divisible
      by the length you want your word list to be. For instance, if your word list length is 3
      words, and you thought that 14 staircases of stepping up to the next value was good, then
      your array would be 42 words long.

      The above description is outdated as of 03/28/2022:
      We use a function to decide the step values and will use the full length of the word list. The funciton decides
      steps by (length of wordlist + 12)/length of wordlist

      Sessions that a participant can do before it steps them down a word are decided by the function 3/4 * length of wordlist
      */

      if(day < 6)
      {
        if(File.Exists(directoryShuffledListTotal))
        {
          Debug.Log("This file of total words exists, we will make the word list the used list");
          //Determine what the list is to start with by making the reader be the used list
          reader = System.IO.File.ReadAllLines(directoryShuffledListUsed);
          foreach(string word in reader)
          {
            //readerUsed is the within script storage of words (same with readerUnused and readerWeek2)
            readerUsed.Add(word);
          }

          //Should not need this in week 1
          // //This will take the last word of the file and make it the new starting location for the new lists (week 2)
          // lastWord = readerUsed.Count;

          //clear array before writing to it again.
          Array.Clear(reader,0,reader.Length);
          reader = System.IO.File.ReadAllLines(directoryShuffledListUnused);
          foreach(string word in reader)
          {
            readerUnused.Add(word);
          }
        }else
        {
          Debug.Log("Creating directory");
          //Call function to randomize the array per person and then store that list in a new text File
          // reader = System.IO.File.ReadAllLines("Assets/Test.txt");
          reader = System.IO.File.ReadAllLines(torontoWordPool);
          shuffleArray(reader);
          using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListTotal, true))
          {
            // file.WriteLine("Participant Unique Shuffled Word List");
            // file.WriteLine("Participant ID: " + participantID);
            foreach(string word in reader)
            {
              file.WriteLine(word);
              readerUnused.Add(word);
            }
          }
        }
        using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUnused, true))
        {
          // file.WriteLine("Participant Unique Shuffled Word List");
          // file.WriteLine("Participant ID: " + participantID);
          foreach(string word in readerUnused)
          {
            file.WriteLine(word);
          }
        }

      //Here is where we will write week 2 selection of the array
      }else{
        if(File.Exists(directoryShuffledListUsed2))
        {
          Debug.Log("This file of week 2 words exists, we will make the word list the used list");
          //Determine what the list is to start with by making the reader be the used list
          reader = System.IO.File.ReadAllLines(directoryShuffledListUsed2);
          foreach(string word in reader)
          {
            //readerUsed is the within script storage of words (same with readerUnused and readerWeek2)
            readerUsed.Add(word);
            readerWeek2.Add(word);
          }
          //This will take the last word of the file and make it the new starting location for the new lists (week 2)
          lastWord = readerUsed.Count();

          dejaVu = true;
          //clear array before writing to it again.
          Array.Clear(reader,0,reader.Length);

        }else
        {
          Debug.Log("Will create week 2 directory once words seen.");
          // //Call function to randomize the array per person and then store that list in a new text File
          // // reader = System.IO.File.ReadAllLines("Assets/Test.txt");
          // reader = System.IO.File.ReadAllLines(torontoWordPool);
          // shuffleArray(reader);
          // using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListTotal, true))
          // {
          //   // file.WriteLine("Participant Unique Shuffled Word List");
          //   // file.WriteLine("Participant ID: " + participantID);
          //   foreach(string word in reader)
          //   {
          //     file.WriteLine(word);
          //     readerUnused.Add(word);
          //   }
          // }

        }

        reader = System.IO.File.ReadAllLines(directoryShuffledListUnused);
        foreach(string word in reader)
        {
          readerUnused.Add(word);
        }
      }
    }
    private void shuffleArray(string[] array)
    {
      //This will be the array that each participant will get
      //The array will be created by shuffling the list from the parent text file
      //We will then store the shuffled array as that participant's unique list

      for(int n = 0;n < array.Length;n++)
      {
        int r = UnityEngine.Random.Range(0,array.Length);
        string t = array[r];
        array[r] = array[n];
        array[n] = t;
      }
    }

    IEnumerator waitStartTask()
    {
      Debug.Log("in wait mode before start task");
      text.text = "Welcome to the Free Recall Task.\n\nOnce you advance, you will see a number of words. You will be be asked to remember these words and recall them.\n\nPress the space bar when you are ready to advance.";

      if(dejaVu)
      {
          //dejaVu is meant to check the last trial number and make the new trial number based on the old one.
          dejaVuString = File.ReadLines(directoryData).Last();
          dejaVuArray = dejaVuString.Split(new[]{';'}, StringSplitOptions.RemoveEmptyEntries);
          int dejaVuArrayTrial = int.Parse(dejaVuArray[2]);
          //I am doing this in case the program fails during the first day but after the participant has already run for a while. In that case, we would want to continue with the words that they have already seen.
          if(dejaVuArrayTrial == 1)
          {
            //If they have only run one trial, just restart. To restart, you must delete the old files in the Data folder.
            StartCoroutine(failTask());
          }else{
            //Make trials the new trial number + 1
            trials = dejaVuArrayTrial + 1;
          }

      }


      yield return new WaitUntil(() => Input.GetKeyDown("space"));

      if(File.Exists(directoryData))
      {
        // Debug.Log("The data file exists, will write new lines to it.");
        using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
        {
          file.WriteLine("ProgramStart");
          // file.WriteLine("Correct?: " + mathCorrect);
        }
      }
      StartCoroutine(freeRecallTask());
    }
    IEnumerator doAgain()
    {
      text.fontSize = instructionFontSize;
      if(steppingUp)
      {
        text.text = "Great job!\n\nYou will now do the task again with more words.\n\nWhen you are ready, press the space bar.";
      }else{
        text.text = "You will now do the task again.\n\nWhen you are ready, press the space bar.";
      }

      yield return new WaitUntil(() => Input.GetKeyDown("space"));
      //increment trials by 1 as this is now the next time they are going through all of the functions.
      trials++;
      //Turn off steppingUp boolean to prepare for next time
      steppingUp = false;
      StartCoroutine(freeRecallTask());
    }

    //Main task for free recall
    IEnumerator freeRecallTask()
    {
      // Debug.Log("trials number: " + trials);
      // Debug.Log("Day number: " + day);

      // Have script that pops up to say "Word list will begin in 3..2..1" with a 1 second pause between each number and a blank screen with 1 second pause before the word list starts.
      int k = 3;
      for(int i = 0; i < 3; i++)
      {
        text.text = "Word list will begin in " + k;
        yield return new WaitForSeconds(1f);
        if(k == 1)
        {
          text.text = "";
          yield return new WaitForSeconds(1f);
        }
        k--;
      }

      text.fontSize = wordFontSize;
      // for(int i = 0; i <3; i++)
      // {
      //   Debug.Log(readerUnused[i]);
      // }


      if(day < 6)
      {
        if(day == 1 && trials == 1)
        {
          for(int i = wordListValue; i < lengthOfWordList; i++)
          {
            // Debug.Log("word list value: " wordListValue);
            // Debug.Log("Length of word list: " + lengthOfWordList);
            // Debug.Log("In here");
            // Debug.Log("i value: " + i);
              text.text = readerUnused[i];
                //pause 3 seconds with words on screen
              yield return new WaitForSeconds(3f);
              //blank screen for 3 seconds
              text.text = "";
              yield return new WaitForSeconds(3f);

          }
          for(int i = wordListValue; i < lengthOfWordList; i++)
          {
            readerUsed.Add(readerUnused[i]);
          }
          readerUnused.RemoveRange(wordListValue,lengthOfWordList);
          using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUsed, true))
          {
            foreach(string word in readerUsed)
            {
              file.WriteLine(word);
            }
          }
          //write over unused word list with the new list
          File.Delete(directoryShuffledListUnused);
          using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUnused, true))
          {
            foreach(string word in readerUnused)
            {
              file.WriteLine(word);
            }
          }

        }else if(day > 1 && trials == 1){

          for(int i = wordListValue; i < readerUsed.Count(); i++)
          {
              text.text = readerUsed[i];
                //pause 3 seconds with words on screen
              yield return new WaitForSeconds(3f);
              //blank screen for 3 seconds
              text.text = "";
              yield return new WaitForSeconds(3f);
          }
        }else{
          for(int i = wordListValue; i < lengthOfWordList; i++)
          {
              text.text = readerUsed[i];
                //pause 3 seconds with words on screen
              yield return new WaitForSeconds(3f);
              //blank screen for 3 seconds
              text.text = "";
              yield return new WaitForSeconds(3f);
          }
        }
        //Need to edit here and figure out the second week stuff...
      }else if(day == 6 && trials == 1){

        for(int i = wordListValue; i < lengthOfWordList; i++)
        {
            text.text = readerUnused[i];
              //pause 3 seconds with words on screen
            yield return new WaitForSeconds(3f);
            //blank screen for 3 seconds
            text.text = "";
            yield return new WaitForSeconds(3f);

        }
        for(int i = wordListValue; i < lengthOfWordList; i++)
        {
          readerUsed.Add(readerUnused[i]);
          readerWeek2.Add(readerUnused[i]);
        }
        readerUnused.RemoveRange(wordListValue,lengthOfWordList);
        using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUsed, true))
        {
          for(int i = wordListValue; i < lengthOfWordList;i++)
          {
            file.WriteLine(readerWeek2[i]);
          }
        }
        //Also will start the week 2 used directory here
        using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUsed2, true))
        {
          for(int i = wordListValue; i < lengthOfWordList;i++)
          {
            file.WriteLine(readerWeek2[i]);
          }
        }
        //write over unused word list with the new list
        File.Delete(directoryShuffledListUnused);
        using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUnused, true))
        {
          foreach(string word in readerUnused)
          {
            file.WriteLine(word);
          }
        }
      }else if(day > 6 && trials == 1)
      {
        // dejaVuString = File.ReadLines(directoryData).Last();
        // dejaVuArray = dejaVuString.Split(new[]{';'}, StringSplitOptions.RemoveEmptyEntries);
        int dejaVuArrayLengthOfWordList = int.Parse(dejaVuArray[4]);
        int temp = dejaVuArray.Length - 2;
        int dejaVuArrayLastComplete = int.Parse(dejaVuArray[temp]);
        lengthOfWordList = dejaVuArrayLengthOfWordList;
        if(dejaVuArrayLengthOfWordList == dejaVuArrayLastComplete)
        {
          //Their last session from the previous day was correct and we want to increment up by the necessary amount and pull from the unused list
          Debug.Log("Previous day was correct, incrementing");
          double doubleWordStepUp = (lengthOfWordList+12d)/lengthOfWordList;
          doubleWordStepUp = Math.Round(doubleWordStepUp, MidpointRounding.AwayFromZero);
          wordStepUp = (int)doubleWordStepUp;
          lengthOfWordList=wordStepUp+lengthOfWordList;
          PlayerPrefs.SetInt("Length of Word List",lengthOfWordList);

          //reading to participant from unused list of words
          for(int i = (readerUsed.Count() - lengthOfWordList); i < readerUsed.Count();i++)
          {
              text.text = readerUsed[i];
                //pause 3 seconds with words on screen
              yield return new WaitForSeconds(3f);
              //blank screen for 3 seconds
              text.text = "";
              yield return new WaitForSeconds(3f);
          }
          // for(int i = (readerUsed.Count() - lengthOfWordList); i < readerWeek2.Count();i++)
          // {
          //   readerWeek2.Add(readerUsed[i]);
          // }
        }else{
          //Their last session from the previous day was NOT correct and we want to pull from the used list
          int y = readerUsed.Count();
          int i = (y-lengthOfWordList);
          for(int x = i; x < y; x++)
          {
            //ReaderWeek2 should be new every day and contain the words that the participant will see that unique day
            readerWeek2.Add(readerUsed[x]);
          }
          int p = 0;
          for(p = (readerWeek2.Count() - lengthOfWordList);p < readerWeek2.Count(); p++)
          {
              text.text = readerWeek2[p];
                //pause 3 seconds with words on screen
              yield return new WaitForSeconds(3f);
              //blank screen for 3 seconds
              text.text = "";
              yield return new WaitForSeconds(3f);

          }
        }
      //Removed && !correct from below
      }else if(day >= 6 && trials > 1)
      {
        //We want to repeat last list from readerWeek2
        Debug.Log("word list value: " + wordListValue);
        Debug.Log("length of word list: " + lengthOfWordList);
        for(int i = (readerWeek2.Count-lengthOfWordList); i < readerWeek2.Count; i++)
        {
            text.text = readerWeek2[i];
              //pause 3 seconds with words on screen
            yield return new WaitForSeconds(3f);
            //blank screen for 3 seconds
            text.text = "";
            yield return new WaitForSeconds(3f);

        }
      }
      correct = false;
      //Ask for the user's rating of how well they used the autobiographical method
      inputFieldMethod.SetActive(true);
      methodSubmitButton.SetActive(true);
      text.text = "How well did you use the method?\n\nEnter an answer 1-4.\n\n1 being you did not use the method well and 4 being you used the method well.";
      yield return new WaitUntil(() => mathSubmitBool && methodFilledBool);
      inputMethod.text = inputMethod.text.Trim();
      methodRating = int.Parse(inputMethod.text);
      inputMethod.text = "";
      text.text = "";
      inputFieldMethod.SetActive(false);
      methodSubmitButton.SetActive(false);

      mathSubmitBool = false;
      text.fontSize = instructionFontSize;
      text.text = "During the next tasks, you will be asked to do some math. Please enter the correct answer.\n\nWhen you are ready, press enter.";
      yield return new WaitUntil(() => Input.GetKeyDown("return"));
      //Change font of math to be bigger (same size as word list words) and set input field to active
      text.fontSize = wordFontSize;
      inputFieldMath.SetActive(true);
      mathSubmitButton.SetActive(true);

      //start generating random numbers and present questions to participant
      num1 = UnityEngine.Random.Range(1,11);
      num2 = UnityEngine.Random.Range(1,11);
      text.text = num1 + " x " + num2 + " =";
      // yield return new WaitUntil(() => Input.GetKeyDown("1") || Input.GetKeyDown("0"));
      yield return new WaitUntil(() => mathSubmitBool && mathFilledBool);
      //Will need to put code here to check if their answer is correct
      inputMath.text = inputMath.text.Trim();
      // StartCoroutine(mathCheckFilled());
      mathAnswer = int.Parse(inputMath.text);
      if((num1*num2) == mathAnswer)
      {
        mathCorrect = "Yes";
      }else{
        mathCorrect = "No";
      }
      //Erase their previous answer
      inputMath.text = "";
      using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
      {
        file.Write(day + ";" + participantID + ";" + trials + ";" + System.DateTime.Now + ";" + lengthOfWordList + ";" + methodRating + ";");
        file.Write(num1 + "x" + num2 + ";" + mathAnswer + ";" + mathCorrect + ";");
        // file.WriteLine("Correct?: " + mathCorrect);
      }
      yield return new WaitForSeconds(.1f);


      mathSubmitBool = false;
      num1 = UnityEngine.Random.Range(1,11);
      num2 = UnityEngine.Random.Range(1,11);
      text.text = num1 + " + " + num2 + " =";
      // yield return new WaitUntil(() => Input.GetKeyDown("1") || Input.GetKeyDown("0"));
      yield return new WaitUntil(() => mathSubmitBool && mathFilledBool);
      inputMath.text = inputMath.text.Trim();
      mathAnswer = int.Parse(inputMath.text);
      if((num1+num2) == mathAnswer)
      {
        mathCorrect = "Yes";
      }else{
        mathCorrect = "No";
      }
      //Erase their previous answer
      inputMath.text = "";
      using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
      {
        // file.WriteLine("Math question: " + num1 + "+" + num2);
        // file.WriteLine("Participant Answer: " + mathAnswer);
        // file.WriteLine("Correct?: " + mathCorrect);
        file.Write(num1 + "+" + num2 + ";" + mathAnswer + ";" + mathCorrect + ";");
      }
      yield return new WaitForSeconds(.2f);


      mathSubmitBool = false;
      num1 = UnityEngine.Random.Range(900,1001);
      num2 = UnityEngine.Random.Range(1,11);
      text.text = num1 + " - " + num2 + " =";
      // yield return new WaitUntil(() => Input.GetKeyDown("1") || Input.GetKeyDown("0"));
      yield return new WaitUntil(() => mathSubmitBool && mathFilledBool);
      inputMath.text = inputMath.text.Trim();
      mathAnswer = int.Parse(inputMath.text);
      if((num1-num2) == mathAnswer)
      {
        mathCorrect = "Yes";
      }else{
        mathCorrect = "No";
      }
      //Erase their previous answer
      inputMath.text = "";
      using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
      {
        // file.WriteLine("Math question: " + num1 + "-" + num2);
        // file.WriteLine("Participant Answer: " + mathAnswer);
        // file.WriteLine("Correct?: " + mathCorrect);
        file.Write(num1 + "-" + num2 + ";" + mathAnswer + ";" + mathCorrect + ";");
      }
      yield return new WaitForSeconds(.2f);


      mathSubmitBool = false;
      num1 = UnityEngine.Random.Range(1,11);
      num2 = UnityEngine.Random.Range(1,11);
      text.text = num1 + " x " + num2 + " =";
      // yield return new WaitUntil(() => Input.GetKeyDown("1") || Input.GetKeyDown("0"));
      yield return new WaitUntil(() => mathSubmitBool && mathFilledBool);
      inputMath.text = inputMath.text.Trim();
      mathAnswer = int.Parse(inputMath.text);
      if((num1*num2) == mathAnswer)
      {
        mathCorrect = "Yes";
      }else{
        mathCorrect = "No";
      }
      //Erase their previous answer
      inputMath.text = "";
      using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
      {
        // file.WriteLine("Math question: " + num1 + "x" + num2);
        // file.WriteLine("Participant Answer: " + mathAnswer);
        // file.WriteLine("Correct?: " + mathCorrect);
        file.Write(num1 + "x" + num2 + ";" + mathAnswer + ";" + mathCorrect + ";");
      }
      yield return new WaitForSeconds(.2f);


      mathSubmitBool = false;
      num1 = UnityEngine.Random.Range(50,100);
      num2 = UnityEngine.Random.Range(1,11);
      text.text = num1 + " + " + num2 + " =";
      // yield return new WaitUntil(() => Input.GetKeyDown("1") || Input.GetKeyDown("0"));
      yield return new WaitUntil(() => mathSubmitBool && mathFilledBool);
      inputMath.text = inputMath.text.Trim();
      mathAnswer = int.Parse(inputMath.text);
      if((num1+num2) == mathAnswer)
      {
        mathCorrect = "Yes";
      }else{
        mathCorrect = "No";
      }
      //Erase their previous answer
      inputMath.text = "";
      using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
      {
        // file.WriteLine("Math question: " + num1 + "+" + num2);
        // file.WriteLine("Participant Answer: " + mathAnswer);
        // file.WriteLine("Correct?: " + mathCorrect);
        file.Write(num1 + "+" + num2 + ";" + mathAnswer + ";" + mathCorrect + ";");
      }
      yield return new WaitForSeconds(.2f);


      //Turn off math input field and math submit button and reset math submit boolean to false
      inputFieldMath.SetActive(false);
      mathSubmitButton.SetActive(false);
      mathSubmitBool = false;


      //Change size of font back to instruction size, about to start asking for recall
      text.fontSize = instructionFontSize;
      text.text = "On the following screen, you will enter as many words as you can remember from the list you were shown.\n\nSeparate the words by commas.\n\nIf you are unsure about spelling, you may ask the experimenter.\n\nWhen you are ready to move on, press enter.";
      yield return new WaitUntil(() => Input.GetKeyDown("return"));
      text.text = "";
      inputField.SetActive(true);
      wordSubmitButton.SetActive(true);

      yield return new WaitForSeconds(.2f);
      yield return new WaitUntil(() => mathSubmitBool);
      compare();
      text.text = "You got " + numWordCorrect + " correct.";
      yield return new WaitForSeconds(3f);
      decideWordStepUpOrStepDown();
      mathSubmitBool = false;
      // decideTrialsStepUpOrStepDown();


    }
    // IEnumerator mathCheckFilled()
    // {
    //   while(inputMath.text == "")
    //   {
    //
    //   }
    // }
    private void ParameterOnClick()
    {
      // Debug.Log("Math submitted");
      mathSubmitBool = true;
      inputMath.text = Regex.Replace(inputMath.text, @"[^0-9]+", "");
      if(inputMath.text.Trim() != "")
      {
        mathFilledBool = true;
      }else{
        mathFilledBool = false;
      }
      try{
        if(inputFieldMethod.activeInHierarchy)
        {
          inputMethod.text = Regex.Replace(inputMethod.text, @"[^0-9]+", "");
          inputMethod.text = inputMethod.text.Trim();
          methodRating = int.Parse(inputMethod.text);
          if(inputMethod.text.Trim() != "" && methodRating < 5 && methodRating > 0)
          {
            methodFilledBool = true;
          }else{
            methodFilledBool = false;
            inputMethod.text = "";
          }
        }
      }catch(Exception e){
        //Do nothing
      }


    }
    private void compare()
    {
      numWordCorrect = 0;

      //Write to same line of directoryData with the words from the list that participants should have learned and their raw responses
      if(day < 6)
      {
        using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
        {
          file.Write("\"");
          // foreach(string word in readerUsed)
          // {
          //   file.Write(word + ",");
          // }
          for(int i = wordListValue; i < lengthOfWordList; i++)
          {
            file.Write(readerUsed[i] + ",");
          }
          file.Write("\";\"" + input.text + "\"");
        }
      }else{
        using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
        {
          file.Write("\"");
          // foreach(string word in readerUsed)
          // {
          //   file.Write(word + ",");
          // }
          for(int i = (readerWeek2.Count() - lengthOfWordList); i < readerWeek2.Count(); i++)
          {
            file.Write(readerWeek2[i] + ",");
          }
          file.Write("\";\"" + input.text + "\"");
        }
      }


      preRemovalUserAnswers = input.text.Split(new[]{',',' ',';'}, StringSplitOptions.RemoveEmptyEntries);
      // for(int a = 0; a < preRemovalUserAnswers.Length; a++)
      // {
      //   preWhiteSpace[a] = String.Concat(preRemovalUserAnswers.Where(c => !Char.IsWhiteSpace(c)));
      // }


      input.text = "";
      // Debug.Log("UserAnswers looks like this: " + userAnswers[0] + userAnswers[1]);
      inputField.SetActive(false);
      wordSubmitButton.SetActive(false);
      userAnswers = preRemovalUserAnswers.Distinct().ToArray();
      for(int p = 0; p < userAnswers.Length; p++)
      {
        userAnswers[p] = userAnswers[p].ToLower();
      }
      if(day < 6)
      {
        for(int x = 0; x < userAnswers.Length; x++)
        {
          for(int y = wordListValue; y < lengthOfWordList; y++)
          {
            if(userAnswers[x]==readerUsed[y])
            {
              numWordCorrect++;
            }
          }
        }
      }else{
        for(int x = 0; x < userAnswers.Length; x++)
        {
          for(int y = (readerWeek2.Count() - lengthOfWordList); y < readerWeek2.Count(); y++)
          {
            if(userAnswers[x]==readerWeek2[y])
            {
              // Debug.Log("This is correct: " + userAnswers[x]);
              numWordCorrect++;
            }
          }
        }
      }


      using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
      {
        file.Write(";\"");
        foreach(string word in userAnswers)
        {
          file.Write(word + ",");
        }
        file.Write("\";" + numWordCorrect + ";");
      }
    }
    private void decideWordStepUpOrStepDown()
    {
      //Do we want to allow people to go to full addition of step up if they get all right? even if they were at a lower amount of things to memorize?

      if(numWordCorrect == (lengthOfWordList-wordListValue))
      {
        Debug.Log("correct number of words");
        correct = true;
        //This (below) is when someone gets the list correct AND they are not on a decreased word list
        if(numTimeSessionsWrong == 0)
        {
          numSessionsWrong = 0;
          //Added this in rather than make the wordStepUp public. The function here should add a number of words to the lists length = (list length +12)/list length
          //make a double that can be used in case of a decimal value. Then convert back to integer after rounded (rounds =>.5 up and <.5 down)

          double doubleWordStepUp = (lengthOfWordList+12d)/lengthOfWordList;
          // Debug.Log("This is the length of the word list: " + lengthOfWordList);
          // Debug.Log("This is the double word step up: " + doubleWordStepUp);
          doubleWordStepUp = Math.Round(doubleWordStepUp, MidpointRounding.AwayFromZero);
          // Debug.Log("This is the double word step up after rounding: " + doubleWordStepUp);
          wordStepUp = (int)doubleWordStepUp;
          // Debug.Log("This is theword step up after rounding and converted to integer: " + wordStepUp);
          //Change value of wordListValue to add in the necessary stepup. (if get 3 right out of 3, step up 5 to 8)
          wordListValue = wordListValue+wordStepUp;
          lengthOfWordList=PlayerPrefs.GetInt("Length of Word List");
          int lastWordTemp = lastWord;
          lastWord = lastWord + lengthOfWordList;
          lengthOfWordList=wordListValue+lengthOfWordList;
          // numTrialsCount++;

          //Adding this to make first week just increment whenever someone gets a list of set length correct AND they are NOT on a decreased length list
          wordListValue = 0;
          // lengthOfWordList=PlayerPrefs.GetInt("Length of Word List");
          // lengthOfWordList = lengthOfWordList + trialWordStepUp;
          PlayerPrefs.SetInt("Length of Word List",lengthOfWordList);
          // wordStepUp=wordStepUp+trialWordStepUp;
          if(day < 6)
          {
            for(int x = 0; x < wordStepUp; x++)
            {
              readerUsed.Add(readerUnused[x]);
            }
            readerUnused.RemoveRange(0,wordStepUp);
            //write over unused word list with the new list
            File.Delete(directoryShuffledListUnused);
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUnused, true))
            {
              foreach(string word in readerUnused)
              {
                file.WriteLine(word);
              }
            }

            using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUsed, true))
            {
              // file.WriteLine("Participant Unique Shuffled Word List");
              // file.WriteLine("Participant ID: " + participantID);

              //Could change to readerUsed.Count() and subtract wordStepUp
              for(int i = (readerUsed.Count() - wordStepUp); i < readerUsed.Count(); i++)
              {
                file.WriteLine(readerUsed[i]);
              }
            }
          //Week 2 stuff
          }else{
            for(int x = 0; x < lengthOfWordList; x++)
            {
              readerWeek2.Add(readerUnused[x]);
            }
            readerUnused.RemoveRange(0,lengthOfWordList);
            //write over unused word list with the new list
            File.Delete(directoryShuffledListUnused);
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUnused, true))
            {
              foreach(string word in readerUnused)
              {
                file.WriteLine(word);
              }
            }

            using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUsed, true))
            {
              // file.WriteLine("Participant Unique Shuffled Word List");
              // file.WriteLine("Participant ID: " + participantID);
              Debug.Log("current location: " + lastWordTemp);
              Debug.Log("second i value: " + (wordStepUp+lastWordTemp));
              for(int i = (readerWeek2.Count-lengthOfWordList); i < readerWeek2.Count; i++)
              {
                file.WriteLine(readerWeek2[i]);
              }
            }
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryShuffledListUsed2, true))
            {
              for(int i = (readerWeek2.Count-lengthOfWordList); i < readerWeek2.Count; i++)
              {
                file.WriteLine(readerWeek2[i]);
              }
            }
          }


          steppingUp = true;
        //This (below) is for increasing a decreased word list up 1 until they reach the original word list length
        }else{
          wordListValue = wordListValue+wordStepDown;
          lengthOfWordList=PlayerPrefs.GetInt("Length of Word List");
          lengthOfWordList=wordListValue+lengthOfWordList;
          wordListValue = wordListValue-wordStepDown;
          numTimeSessionsWrong--;
          PlayerPrefs.SetInt("Length of Word List",lengthOfWordList);
        }
      //This below is for when a participant gets the list wrong
      }else{
        Debug.Log("incorrect number of words");
        lengthOfWordList=PlayerPrefs.GetInt("Length of Word List");
        //Add one whenever someone gets a list wrong
        numSessionsWrong++;
        //Determining the number of sessions before step down (different for week 1 and 2)
        if(day < 6)
        {
          //determine how many sessions they should do before we take a word off. Done by taking 3/4 of list length
          double doubleNumSessions=lengthOfWordList*.75;
          doubleNumSessions = Math.Round(doubleNumSessions, MidpointRounding.AwayFromZero);
          numSessions = (int)doubleNumSessions;
        }else{
          //If in week 2, we want the amount of sessions before we cut down a word = length of word list
          numSessions = lengthOfWordList;
        }
        //Deciding to step down
        if(numSessionsWrong == numSessions)
        {
          Debug.Log("subtracting 1");
          if(wordListValue == 0 || day >= 6){
            // lengthOfWordList=PlayerPrefs.GetInt("Length of Word List")
            lengthOfWordList=lengthOfWordList-wordStepDown;
          }else{
            // lengthOfWordList=PlayerPrefs.GetInt("Length of Word List");
            lengthOfWordList = wordListValue+lengthOfWordList-wordStepDown;
          }
          numTimeSessionsWrong++;
          PlayerPrefs.SetInt("Length of Word List",lengthOfWordList);
        }

      }
      using(System.IO.StreamWriter file = new System.IO.StreamWriter(directoryData, true))
      {
        file.WriteLine(numSessionsWrong);
      }
      if(numSessionsWrong == numSessions)
      {
        numSessionsWrong = 0;
      }
      StartCoroutine(doAgain());
    }



}
