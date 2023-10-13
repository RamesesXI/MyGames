using UnityEngine;
using System.Linq;
using System.Collections;

public class Submission : MonoBehaviour
{
    public static int complexityLevel = 3;
    [HideInInspector]
    public bool winner = false;

    private string[] safecode = new string[complexityLevel];
    public int row = 0;

    private bool canRestart = false;

    public Results resultScript;
    private ButtonManager buttonMangerScript;

    void Start ()
    {
        InitSafecode();

        buttonMangerScript = GetComponent<ButtonManager>();
    }

    public void CompareCodes(string[] code)
    {
        string digits = "";
        int ticks = 0;
        int discs = 0;

        for (int i = 0; i < complexityLevel; i++)
        {
            digits += code[i];

            if (safecode.Contains(code[i]))
            {
                if (safecode[i] == code[i])
                    ticks++;
                else                
                    discs++;             
            }
        }

        resultScript.ShowResult(row, digits, discs, ticks);
        row++;

        if (!canRestart)
        {
            buttonMangerScript.RestartAvailable();
        }

        if (ticks == complexityLevel)
        {
            winner = true;
            StartCoroutine(youWon());
        }
    }

    private IEnumerator youWon()
    {
        yield return new WaitForSeconds(2.2f);

        buttonMangerScript.youWonPopup();
    }

    private void InitSafecode()
    {
        string solution = ""; //extra
        string[] checker = new string[safecode.Length];

        for (int i = 0; i < safecode.Length; i++)
        {
            safecode[i] = Random.Range(0, 10).ToString();

            safecode.CopyTo(checker, 0);
            checker[i] = null;

            while (checker.Contains(safecode[i]))
            {
                safecode[i] = Random.Range(0, 10).ToString();
            }

            solution += safecode[i]; //extra
        }

        Debug.Log(solution); //extra
    }
}