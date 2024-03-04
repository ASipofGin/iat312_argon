using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPCBehaviour : MonoBehaviour
{

    public GameObject dialoguePanel;
    public Text dialogueText;
    public Text npcName;
    public string[] dialogue;
    public new string name;
    private int index;

    public float textSpeed;
    public bool playerProximity;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerProximity)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                zeroText();
            }
            else
            {
                
                dialoguePanel.SetActive(true);
                npcName.text = name;
                StartCoroutine(TextEffect());
            }
        }
    }

    public void zeroText()
    {
        if (dialoguePanel != null){
            dialogueText.text = "";
            index = 0;
            dialoguePanel.SetActive(false);
        }

    }

    IEnumerator TextEffect()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(TextEffect());
        }
        else
        {
            zeroText();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerProximity = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerProximity = false;
            zeroText();
        }
    }

}
