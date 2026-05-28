using System;
using System.Collections;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소 - 인스펙터 창에서 연결")]

    public GameObject DialoguePanel;
    public Image characterImage;
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;

    [Header("기본 설정")]
    public Sprite defaultCharacterImage;

    [Header("타이핑 효과 설정")]
    public float typingSpeed = 0.05f;
    public bool skipTypingOnClick = true;

    private DialogueDataSO currentDialogue;
    private int currenLinelndex = 0;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        DialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(HandleNextInput);
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();
        }
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";
        for (int i = 0; i < textToType.Length; i++)
        {
            dialogueText.text += textToType[i];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        isTyping = false;

        if (currentDialogue != null && currenLinelndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currenLinelndex];
        }
    }

    void ShowCurrentLine()
    {
        if (currentDialogue != null && currenLinelndex < currentDialogue.dialogueLines.Count)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
        }

        string currentText = currentDialogue.dialogueLines[currenLinelndex];
        typingCoroutine = StartCoroutine(TypeText(currentText));
    }

    public void ShowNextLine()
    {
        currenLinelndex++;

        if (currenLinelndex >= currentDialogue.dialogueLines.Count)
        {
            EndDialogue();
        }
        else
        {
            ShowCurrentLine();
        }
    }

    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;
        isTyping = false;
        DialoguePanel.SetActive(false);
        currenLinelndex = 0;
    }

    public void HandleNextInput()
    {
        if (isTyping && skipTypingOnClick)
        {
            CompleteTyping();
        }
        else if (!isTyping)
        {
            ShowNextLine();
        }
    }

    public void SkipDialogue()
    {
        EndDialogue();
    }

    public bool IsDialgueActive()
    {
        return isDialogueActive;
    }

    public void StartDialogue(DialogueDataSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;

        currentDialogue = dialogue;
        currenLinelndex = 0;
        isDialogueActive = true;

        DialoguePanel.SetActive(true);
        characterNameText.text = dialogue.characterName;

        if (characterImage != null)
        {
            if (dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;
            }
        }

        ShowCurrentLine();
    }
}
