using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region Variables

    [SerializeField] float pauseBetweenRepliquesDelay = .3f;
    [SerializeField] GameObject enemiesParent;


    Dialogue currentDialogue;
    Objective currentObjective;
    AudioClip previousDialogueClip; //Pour couper le dialogue en cours si un nouveau doit être joué
    PlayerController pc;
    int curDialogueIndex;


    public static DialogueManager instance;
    public static bool isPlaying;
    Coroutine nextRepliqueCo;
    NavMeshPlayerDetector[] enemies;

    [Space(10)]
    [Header("First Person Controller : ")]
    [Space(10)]

    //Pour ralentir le joueur pendant les dialogues
    public float slowWalkSpeed = .5f;
    public float slowRunSpeed = 1f;
    float _startWalkSpeed, _startRunSpeed;//Pour ralentir le joueur pendant les dialogues

    #endregion


    //pour éviter de passer la première réplique
    float _firstRepliqueTimer = 0f;



    #region Mono

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
        _startWalkSpeed = PlayerController.fpc.m_WalkSpeed;
        _startRunSpeed = PlayerController.fpc.m_RunSpeed;

        enemies = enemiesParent.GetComponentsInChildren<NavMeshPlayerDetector>();

    }

    //Pour passer les répliques manuellement.
    //On a des doublages maintenant, donc on en a plus besoin


    //private void Update()
    //{
    //    if(_firstRepliqueTimer < .1f)
    //    {
    //        _firstRepliqueTimer += Time.deltaTime;
    //    }

    //    //Pour passer manuellement les répliques du dialogue (ne s'active que si le DialogueManager a bien un dialogue en cours)
    //    if (Input.GetMouseButtonDown(0) && isPlaying && _firstRepliqueTimer > .1f)
    //    {
    //        curDialogueIndex++;
    //        NextReplique();
    //    }
    //}


    //Pour éviter que les ennemis n'attaquent le joueur pendant les dialogues
    public void StopEnemies(bool stop)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(!stop);
        }
    }

    #endregion






    #region Dialogue

    public void PlayDialogue(Dialogue newDialogue, Objective newObjective)
    {
        StopAllCoroutines();

        _firstRepliqueTimer = 0f;
        curDialogueIndex = 0;
        currentDialogue = newDialogue;
        currentObjective = newObjective;


        if (!currentDialogue)
        {
            DisplayNewObjective(newObjective);
            //FeedbackCanvas.instance.PrintDialogue("");
            FeedbackCanvas.instance.dialogueText.gameObject.SetActive(false);

            PlayerController.fpc.m_WalkSpeed = _startWalkSpeed;
            PlayerController.fpc.m_RunSpeed = _startRunSpeed;
            PlayerController.isOnTalkie = false;
        }
        else
        {
            StartDialogue();
        }
    }

    private void StartDialogue()
    {
        //On désactive les ennemis pour que le joueur soit tranquille pendant les cutscenes
        //StopEnemies(true);


        //On coupe le dialogue précédent et on joue l'audio du dialogue en cours
        if (previousDialogueClip)
            AudioManager.instance.Stop(previousDialogueClip);

        if(currentDialogue.dialogueClip)
            AudioManager.instance.Play(currentDialogue.dialogueClip);

        previousDialogueClip = currentDialogue.dialogueClip;


        isPlaying = true;
        if (currentDialogue.useTalkie)
        {
            pc.ActivateTalkieDetector(true);
            PlayerController.isOnTalkie = true;
        }
        PlayerController.fpc.m_WalkSpeed = slowWalkSpeed;
        PlayerController.fpc.m_RunSpeed = slowRunSpeed;

        FeedbackCanvas.instance.currentObjectiveText.enabled = false;
        FeedbackCanvas.instance.newObjectiveText.enabled = false;
        FeedbackCanvas.instance.cartoucheDialogue.enabled = true;

        NextReplique();
    }

    private void NextReplique()
    {


        if (curDialogueIndex == currentDialogue.repliques.Length || currentDialogue.repliques.Length == 0)
        {
            //Le dialogue est terminé, on réactive les ennemis
            //StopEnemies(false);


            isPlaying = false;
            //FeedbackCanvas.instance.dialogueText.gameObject.SetActive(false); //Pour cacher "le clic pour continuer"
            FeedbackCanvas.instance.dialogueText.enabled = false;

            if (currentDialogue.useTalkie) pc.ActivateTalkieDetector(false);
            PlayerController.isOnTalkie = false;
            PlayerController.fpc.m_WalkSpeed = _startWalkSpeed;
            PlayerController.fpc.m_RunSpeed = _startRunSpeed;

            if (currentObjective)
            {
                DisplayNewObjective(currentObjective);
            }
            else
            {
                FeedbackCanvas.instance.cartoucheDialogue.enabled = false;
            }


        }
        else
        {
            //FeedbackCanvas.instance.dialogueText.gameObject.SetActive(true); //Pour afficher "le clic pour continuer"
            FeedbackCanvas.instance.dialogueText.enabled = true;

            if (nextRepliqueCo != null)
                StopCoroutine(nextRepliqueCo);

            nextRepliqueCo = StartCoroutine(DisplayNextRepliqueCo());
        }
    }


    private IEnumerator DisplayNextRepliqueCo()
    {

        //On récupère la réplique en cours
        Replique r = currentDialogue.repliques[curDialogueIndex];

        //Pour retirer les retours à la ligne
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < r.text.Length; i++)
        {
            char c = r.text[i];
            if (!char.IsControl(c))
                sb.Append(c);
        }

        //On affiche le texte
        FeedbackCanvas.instance.PrintDialogue($"{r.characterName.ToUpper()} :\n{sb.ToString()}");
        yield return new WaitForSeconds(r.textOnScreenDuration);
        FeedbackCanvas.instance.dialogueText.enabled = false;
        yield return new WaitForSeconds(pauseBetweenRepliquesDelay);

        curDialogueIndex++;
        NextReplique();

        yield return null;
    }

    private void DisplayNewObjective(Objective newObjective)
    {
        FeedbackCanvas.instance.PrintNewObjective(newObjective ? newObjective.bottomtext : "", 
                                                  newObjective ? newObjective.topLeftText : "", 
                                                  newObjective ? newObjective.displayOnScreenDuration : 3f);

        PauseMenu.instance.PrintNewObjective(newObjective ? newObjective.bottomtext : "");
    }

    #endregion
}
