using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    public Image flagImage;
    public Text title, actualName1, actualName2, moneyText;
    public Button[] choices = new Button[4];
    public Button homeButton;
    public GameObject MainMenu, PlayMenu, ChoiceMenu, CorrectMenu, WrongMenu, logo, flag, toggleObject, PopUp;

    public Toggle LanguageToggle;

    private class Country
    {
        public Sprite flag;
        public string ARname;
        public string ENname;

        public Country(Sprite flag, string ARname, string ENname)
        {
            this.flag = flag;
            this.ARname = ARname;
            this.ENname = ENname;
        }
    }

    private List<Country> originalCountries = new List<Country>();
    private List<Country> countries;
    private GameObject currentMenu;
    private int initialCount, counter = 0, correctNum;
    private List<string> AR_choicesList = new List<string>();
    private List<string> EN_choicesList = new List<string>();
    string AR_correctChoice;
    string EN_correctChoice;
    private int money = 40;
    private int currentScore = 0;
    private int highScore = 0;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("High Score"))
            LoadProgress();

        StreamReader ARreader = new StreamReader(Application.dataPath + "/AR_Names.txt");
        StreamReader ENreader = new StreamReader(Application.dataPath + "/En_Names.txt");

        List<Sprite> flags = new List<Sprite>(Resources.LoadAll<Sprite>(""));
        List<string> ARnames = new List<string>(ARreader.ReadToEnd().Split('،'));
        List<string> ENnames = new List<string>(ENreader.ReadToEnd().Split(','));
        ARreader.Close();
        ENreader.Close();
        ARnames.Reverse();

        for (int i = 0; i < flags.Count; i++)
            originalCountries.Add(new Country(flags[i], ARnames[i], ENnames[i]));

        initialCount = originalCountries.Count;
        countries = new List<Country>(originalCountries);
        currentMenu = MainMenu;

        currentScore = 0;
    }

    // Buttons
    public void Home()
    {
        //MenuSwap(MainMenu);
        toggleObject.SetActive(false);
        RefreshTitle("ﺔﻴﺴﻴﺋﺮﻟﺍ ﺔﻤﺋﺎﻘﻟﺍ");

        if (flag.activeInHierarchy)
        {
            flag.SetActive(false);
            logo.SetActive(true);
        }
    }

    public void Popup()
    {
        PopUp.SetActive(!PopUp.activeInHierarchy);
    }

    public void Play()
    {
        RefreshTitle("ﺐﻌﻟﺍ"); //il3ab

        MenuSwap(PlayMenu, MainMenu);
    }

    public void Names()
    {
        toggleObject.SetActive(true);

        //if (counter > 0)
        //{
        //    //Do Nothing
        //}
        //else
            SwitchFlag();

        flag.SetActive(true);
        logo.SetActive(false);
       
        RefreshScore();

        MenuSwap(ChoiceMenu, PlayMenu);
    }

    public void Choice(int n)
    {
        StartCoroutine(CheckAnswer(n));
    }

    public void Continue()
    {
        SaveProgress();

        SwitchFlag();

        homeButton.interactable = true;

        MenuSwap(ChoiceMenu, CorrectMenu);
    }

    public void PaidContinue()
    {
        if (money >= 7)
        {
            money -= 7;
            moneyText.text = money.ToString();
        }
        else
            return;

        currentScore++;
        RefreshScore();
        SaveProgress();

        SwitchFlag();

        homeButton.interactable = true;

        MenuSwap(ChoiceMenu, WrongMenu);
    }

    public void NewGame()
    {
        SaveProgress();

        countries = new List<Country>(originalCountries);
        counter = 0;

        currentScore = 0;
        RefreshScore();

        SwitchFlag();

        homeButton.interactable = true;

        MenuSwap(ChoiceMenu, WrongMenu);
    }

    public void ToggleLanguage()
    {
        LanguageToggle.GetComponentInChildren<Text>().text = LanguageToggle.isOn ? "AR" : "EN";

        if (ChoiceMenu.activeInHierarchy)
        {
            if (LanguageToggle.isOn)
                for (int j = 0; j < choices.Length; j++)
                    choices[j].GetComponentInChildren<Text>().text = AR_choicesList[j].Trim();
            else
                for (int j = 0; j < choices.Length; j++)
                    choices[j].GetComponentInChildren<Text>().text = EN_choicesList[j].Trim();
        }
        else if (CorrectMenu.activeInHierarchy)
            actualName1.text = LanguageToggle.isOn ? AR_correctChoice : EN_correctChoice;
        else if (WrongMenu.activeInHierarchy)
            actualName2.text = LanguageToggle.isOn ? AR_correctChoice : EN_correctChoice;
    }
    // !Buttons

    private void MenuSwap(GameObject newMenu, GameObject oldMenu)
    {
        newMenu.SetActive(true);
        oldMenu.SetActive(false);
    }

    private void RefreshTitle(string titleName)
    {
        title.text = titleName;
    }

    private void RefreshScore()
    {
        if (currentScore > highScore)
            highScore = currentScore;

        title.text = currentScore + " / " + highScore;
    }

    private void SwitchFlag()
    {
        //Check if Game Over
        //if (++counter >= initialCount - 2)
        //{
        //    Home();
        //    return;
        //}

        int k = Random.Range(0, countries.Count);
        flagImage.sprite = countries[k].flag;
        AR_correctChoice = countries[k].ARname;
        EN_correctChoice = countries[k].ENname;
        countries.RemoveAt(k);
        Popup();

        List<Country> countriesCopy = new List<Country>(countries);
        AR_choicesList.Clear();
        EN_choicesList.Clear();

        for (int i = 0; i < choices.Length - 1; i++)
        {
            int h = Random.Range(0, countriesCopy.Count);
            AR_choicesList.Add(countriesCopy[h].ARname);
            EN_choicesList.Add(countriesCopy[h].ENname);
            countriesCopy.RemoveAt(h);
        }

        correctNum = Random.Range(0, AR_choicesList.Count + 1);
        AR_choicesList.Insert(correctNum, AR_correctChoice);
        EN_choicesList.Insert(correctNum, EN_correctChoice);  

        if (LanguageToggle.isOn)
            for (int j = 0; j < choices.Length; j++)
                choices[j].GetComponentInChildren<Text>().text = AR_choicesList[j].Trim();
        else
            for (int j = 0; j < choices.Length; j++)
                choices[j].GetComponentInChildren<Text>().text = EN_choicesList[j].Trim();

        Color flatOrange = new Color32(243, 156, 18, 255);

        foreach (Button button in choices)
            button.image.color = flatOrange;
    }

    private IEnumerator CheckAnswer(int n)
    {
        foreach (Button choice in choices)
            choice.interactable = false;

        homeButton.interactable = false;

        if (n == correctNum)
        {
            choices[n].image.color = new Color32(46, 204, 113, 255);
            yield return new WaitForSeconds(0.8f);

            actualName1.text = LanguageToggle.isOn ? AR_correctChoice : EN_correctChoice;
            MenuSwap(CorrectMenu, ChoiceMenu);

            currentScore++;
            RefreshScore();
        }
        else
        {
            choices[n].image.color = new Color32(231, 76, 60, 255);
            yield return new WaitForSeconds(0.8f);

            actualName2.text = LanguageToggle.isOn ? AR_correctChoice : EN_correctChoice;
            MenuSwap(WrongMenu, ChoiceMenu);
        }

        foreach (Button choice in choices)
            choice.interactable = true;
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("High Score", highScore);
        PlayerPrefs.SetInt("Money", money);

        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        PlayerPrefs.GetInt("High Score", highScore);

        if (PlayerPrefs.HasKey("Money"))
            PlayerPrefs.GetInt("Money", money);
    }
}