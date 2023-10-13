using UnityEngine;
using UnityEngine.UI;

public class Results : MonoBehaviour
{
    public Text[] digits = new Text[14];
    public GameObject[] panel = new GameObject[14];
    private Image[][] signs = new Image[14][];

    public Sprite discSprite;
    public Sprite tickSprite;

    private int lastRow;

    void Start()
    {
        float screenAspect = (float)Screen.width / Screen.height;

        if (Mathf.Approximately(screenAspect, 4.0f / 3.0f))
            lastRow = 12;
        else if (Mathf.Approximately(screenAspect, 16.0f / 10.0f))
            lastRow = 9;
        else
            lastRow = 8;
        

        for (int i = 0; i < 14; i++)
        {
            signs[i] = new Image[6];
            signs[i] = panel[i].GetComponentsInChildren<Image>(true);
        }
    }

    public void ShowResult(int row, string digitString, int discs, int ticks)
    {
        if (row <= lastRow)
        {
            digits[row].text = digitString;
            digits[row].enabled = true;

            for (int i = 1; i <= discs; i++)
            {
                signs[row][i].sprite = discSprite;
                signs[row][i].enabled = true;
            }

            for (int i = discs + 1; i <= discs + ticks; i++)
            {
                signs[row][i].sprite = tickSprite;
                signs[row][i].enabled = true;
            }
        }
        else
        {
            //Scrolling up digits
            for (int i = 0; i < lastRow; i++)
            {
                digits[i].text = digits[i + 1].text;
            }

            //Scrolling up signs
            for (int i = 0; i < lastRow; i++)
            {
                for (int j = 1; j <= 6; j++)
                {
                    signs[i][j].enabled = false;
                    signs[i][j].sprite = null;

                    if (signs[i + 1][j].sprite)
                    {
                        signs[i][j].sprite = signs[i + 1][j].sprite;
                        signs[i][j].enabled = true;
                    }
                }
            }

            //Setting lastRow Input
            digits[lastRow].text = digitString;

            for (int j = 1; j <= 6; j++)
            {
                signs[lastRow][j].enabled = false;
                signs[lastRow][j].sprite = null;
            }

            for (int i = 1; i <= discs; i++)
            {
                signs[lastRow][i].sprite = discSprite;
                signs[lastRow][i].enabled = true;
            }

            for (int i = discs + 1; i <= discs + ticks; i++)
            {
                signs[lastRow][i].sprite = tickSprite;
                signs[lastRow][i].enabled = true;
            }
            //!
        }
    }
}
