using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class NumPad : MonoBehaviour
{
    public Text monitor;
    public Button[] buttons = new Button[12];
    public AudioClip numberAudio, clearAudio, submitAudio, errorAudio;
    public Submission submitScript;

    private string[] code = new string[Submission.complexityLevel];
    private AudioSource audioSource;
    private Animator anim;

    void Start()
    {
        for (int i = 0; i < code.Length; i++)
        {
            code[i] = "_";
        }

        UpdateMonitor();

        audioSource = GetComponent<AudioSource>();
        anim = GetComponentInParent<Animator>();
    }

    public void NumberClick(string num)
    {
        if (code.Contains("_"))
        {
            buttons[int.Parse(num)].interactable = false;
            PlaySound(numberAudio);

            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] == "_")
                {
                    code[i] = num;
                    UpdateMonitor();
                    break;
                }
            }
        }
        else
            PlaySound(errorAudio);
    }

    public void ClearClick()
    {
        if (code[0] != "_")
        {
            PlaySound(clearAudio);

            for (int i = 0; i < code.Length; i++)
            {
                if (code[i] != "_")
                {
                    buttons[int.Parse(code[i])].interactable = true;
                    code[i] = "_";
                }
                else
                    break;
            }

            UpdateMonitor();
        }
        else
            PlaySound(errorAudio);
    }

    public void SubmitClick()
    {
        if (code[code.Length - 1] != "_")
        {
            submitScript.CompareCodes(code);
            PlaySound(submitAudio);

            //Check if user won
            if (submitScript.winner)
            {
                anim.SetBool("winner", true);

                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].interactable = false;
                }
                
                monitor.text = "";
                return;
            }

            for (int i = 0; i < code.Length; i++)
            {
                buttons[int.Parse(code[i])].interactable = true;
                code[i] = "_";
            }

            UpdateMonitor();
        }
        else
            PlaySound(errorAudio);
    }

    private void UpdateMonitor()
    {
        monitor.text = "";

        for (int i = 0; i < code.Length; i++)
        {
            monitor.text += code[i] + " ";
        }

        monitor.text = monitor.text.Substring(0, monitor.text.Length - 1);
    }

    private void PlaySound(AudioClip soundClip)
    {
        audioSource.clip = soundClip;
        audioSource.Play();
    }
}
