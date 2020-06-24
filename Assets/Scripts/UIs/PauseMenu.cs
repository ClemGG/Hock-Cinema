using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas, feedbackCanvas;
    [SerializeField] TextMeshProUGUI newObjectiveText;
    [SerializeField] CharacterController characterController;
    [SerializeField] FirstPersonController fpc;
    public bool isPaused = false;

    public static PauseMenu instance;

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
        isPaused = true;
        Pause();
    }

    public void Pause()
    {

        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
        feedbackCanvas.SetActive(!isPaused);
        Time.timeScale = isPaused ? 0f : 1f;

        //On récupère toutes les voix de dialogues et on les pause quand le jeu est en pause
        Son[] sons = AudioManager.instance.GetAllSonsFromTag("Dialogue");

        for (int i = 0; i < sons.Length; i++)
        {
            if (isPaused)
            {
                sons[i].source.Pause();
            }
            else
            {
                sons[i].source.UnPause();
            }
        }
        

        //On affiche le curseur pendant la pause
        PlayerController.SetCursorVisibility(isPaused);

        characterController.enabled = fpc.enabled = !isPaused;    //On empêche la caméra de se tourner si on est en mode pause

    }


    public void ReturnToMenu()
    {
        SceneFader.instance.FadeToScene(0);
    }


    //Appelée par le Dilogue Manager pour garder en mémoire l'objectif actuel afin que le joueur ne l'oublie pas
    public void PrintNewObjective(string text)
    {
        newObjectiveText.text = text;
    }
}
