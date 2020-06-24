using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerController : MonoBehaviour
{
    [Space(10)]
    [Header("Scripts & Components :")]
    [Space(10)]

    public static FirstPersonController fpc;
    PlayerInput input;
    public Animator mainAnimD, mainAnimG;

    //Rigidbody rb;
    public static Transform t;
    Collider triggerTouched;
    Camera mainCam;

    //Permet de savoir si le joueur est dans une safezone ou pas
    public static bool isSafe;
    public static bool isOnTablet;
    public static bool isOnTalkie;
    public static bool isRunning;


    //Permet de savoir si le joueur a ramassé la tablette ou pas
    public static bool isActionAllowed;
    public static bool hasTablet;
    public static bool hasTalkie;

    [Space(10)]
    [Header("Items :")]
    [Space(10)]

    //public GameObject talkie;
    public GameObject playerCam;
    bool triggerTouchedIsVisible;


    [Space(10)]
    [Header("Audio : ")]
    [Space(10)]

    [SerializeField] LayerMask whatIsMoquette;
    [SerializeField] AudioClip audTalkieOn;
    [SerializeField] AudioClip[] footstepsMoquette;





    #region Mono

    void Awake()
    {
        t = transform;
        fpc = GetComponent<FirstPersonController>();
        input = GetComponent<PlayerInput>();
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (input.shouldSwapCam && hasTablet && ReseauManager.curActiveReseau && !PauseMenu.instance.isPaused)
        {
            ActivateCam();
        }


        //On sépare le escape de l'input de la tablette pour pouvoir l'utiliser à la fois pour le menu pause et pour quitter la tablette
        if (isOnTablet)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ActivateCam();
            }
        }
        else
        {
            
            if (input.hasPressedPause)
            {
                PauseMenu.instance.Pause();

            }
        }

        //if (input.shouldTalkie && !DialogueManager.isPlaying)
        //{
        //    ActivateTalkieDetector(!talkie.activeSelf);
        //}

        if (triggerTouched && !PauseMenu.instance.isPaused)
        {
            /*On*/TriggerStay(triggerTouched);
        }
    }


    private void FixedUpdate()
    {
        Physics.Raycast(mainCam.ViewportToWorldPoint(Vector3.one / 2f) - mainCam.transform.forward * .2f, mainCam.transform.forward, out RaycastHit hit, 5f);

        if (hit.collider)
        {
            if(hit.collider.CompareTag("Item/Renderer") && triggerTouched)
            {
                triggerTouchedIsVisible = hit.collider.transform.parent.Equals(triggerTouched.transform);
            }
            else
            {
                triggerTouchedIsVisible = false;
            }
        }
        else
        {
            triggerTouchedIsVisible = false;
        }

        //Pour changer les bruits de pas si on marche sur de la moquette
        Physics.Raycast(t.position, Vector3.down, out hit, 1.5f, whatIsMoquette);

        if (hit.collider)
        {
            fpc.ChangeFootSteps(footstepsMoquette);
        }
        else
        {
            fpc.ChangeFootSteps();
        }

    }

    #endregion



    #region Cams et talkie

    //Appelée une fois le jeu lancé ou quand le menu pause est actif
    public static void SetCursorVisibility(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }


    //On lui met un paramètre pour pouvoir l'appeler ailleurs
    public void ActivateTalkieDetector(bool active)
    {
        //talkie.SetActive(active);

        mainAnimG.Play(active ? "talkie in" : "talkie out");

        if (active)
            AudioManager.instance.Play(audTalkieOn);
    }


    //Appelée par le clic droit
    public void ActivateCam()
    {
        mainAnimD.Play(isOnTablet ? "tablette out" : "tablette in");
    }

    //Appelée pendant les anims de la tablette
    public void ActivateCamFunc()
    {
        if (playerCam)
        {
            //playerCam.SetActive(!playerCam.activeSelf);
            playerCam.GetComponent<Camera>().enabled = !playerCam.GetComponent<Camera>().enabled;
        }

        //if (securityCams)
        //{
        //    securityCams.SetActive(!securityCams.activeSelf);
        //}


        fpc.enabled = !fpc.enabled;
        isOnTablet = !fpc.enabled;
        ReseauManager.instance.DisableOtherReseaux();
        ReseauManager.instance.ActiverCamerasDuReseau(ReseauManager.curActiveReseau, isOnTablet);

    }



    //Appelée par le checkpoint manager
    public void ResetScreen()
    {
        //Si le controller n'est pas activé, alors la tablette l'est et on doit la désactiver
        if (!fpc.enabled)
        {
            ActivateCam();
        }
    }


    public void SetHasTablet()
    {
        hasTablet = true;
    }

    public void SetHasTalkie()
    {
        hasTalkie = true;
    }
    public void SetActionAllowed(bool allowed)
    {
        isActionAllowed = allowed;
    }

    #endregion


    #region Triggers

    private void /*On*/TriggerStay(Collider c)
    {


        if (c.TryGetComponent(out InteractableTrigger it))
        {
            //triggerTouched = c;
            FeedbackCanvas.instance.interactIcon.enabled = !it.HasBeenInteracted() && triggerTouchedIsVisible && !isOnTablet && !isOnTalkie && isActionAllowed;

        }
        if (c.TryGetComponent(out CollectibleTrigger ct))
        {
            //triggerTouched = c;
            FeedbackCanvas.instance.interactIcon.enabled = !ct.collected && triggerTouchedIsVisible && !isOnTablet && !isOnTalkie && isActionAllowed;

        }
        FeedbackCanvas.instance.interactIcon.enabled = FeedbackCanvas.instance.interactIcon.enabled && !PauseMenu.instance.isPaused;
        bool iconVisible = FeedbackCanvas.instance.interactIcon.enabled;


        if (isOnTablet)
            return;



        if (input.isActing) 
        {
            if (c.CompareTag("Item/Camera"))
            {
                if (c.TryGetComponent(out InteractableTrigger trigger) && iconVisible)
                {
                    trigger.onInteractedEvent?.Invoke();
                }
            }
        }
        if (input.shouldAction)
        {
            if (c.CompareTag("Item/Camera"))
            {
                if (c.TryGetComponent(out InteractableTrigger trigger) && iconVisible)
                {
                    if(!trigger.HasBeenInteracted())
                        trigger.onMouseDownEvent?.Invoke();
                }
            }
            if (c.CompareTag("Item/Collectible"))
            {
                if (c.TryGetComponent(out CollectibleTrigger trigger) && iconVisible)
                {
                    trigger.Pickup();
                }
            }
        }
        if (input.actionStopped)
        {
            if (c.CompareTag("Item/Camera"))
            {
                if (c.TryGetComponent(out InteractableTrigger trigger))
                {
                    if(!trigger.HasBeenInteracted())
                    trigger.onMouseUpEvent?.Invoke();
                }
            }
        }

        
    }


    private void OnTriggerEnter(Collider c)
    {
        //Pour éviter que le trigger de détection de l'ennemi ne bloque les boîtiers et autres triggers
        if (c.CompareTag("Entity/Enemy"))
            return;

        triggerTouched = c;
        if (c.gameObject.layer == LayerMask.NameToLayer("Terrain/Safezone"))
        {
            isSafe = true;
        }

        //print($"trigger touched : {c.name}");
    }


    private void OnTriggerExit(Collider c)
    {
        //Pour éviter que le trigger de détection de l'ennemi ne bloque les boîtiers et autres triggers
        if (c.CompareTag("Entity/Enemy"))
            return;



        if (c.gameObject.layer == LayerMask.NameToLayer("Terrain/Safezone"))
        {
            isSafe = false;
        }
        //print($"trigger quit : {c.name}");

        if (c.CompareTag("Item/Camera"))
        {
            if (c.TryGetComponent(out InteractableTrigger trigger))
            {
                trigger.onMouseUpEvent?.Invoke();
            }
        }
        triggerTouched = null;
        FeedbackCanvas.instance.interactIcon.enabled = false;
    }



    //Appelée par le DoorTrigger pour faire reculer le joueur pour éviter qu'il ne se prenne la porte dans la tronche
    public void MoveAwayFromDoor(Transform door)
    {
        float angle = Vector3.Angle(door.forward, t.forward);
        bool isFacingDoor = angle > 135f;

        if (isFacingDoor)
            StartCoroutine(MoveAwayFromDoorCo(door));
    }

    private IEnumerator MoveAwayFromDoorCo(Transform door)
    {
        CharacterController cc = GetComponent<CharacterController>();
        cc.enabled = false;

        Vector3 startPos = t.position;
        Vector3 endPos = startPos - t.forward * .5f;

        float timer = 0f;
        float moveSpeed = 3f;

        while(timer < 1f)
        {
            timer += Time.deltaTime * moveSpeed;
            t.position = Vector3.Lerp(startPos, endPos, timer);
            yield return null;
        }
        while (timer > 0f)
        {
            timer -= Time.deltaTime * moveSpeed;
            t.position = Vector3.Lerp(startPos, endPos, timer);
            yield return null;
        }

        cc.enabled = true;
    }


    #endregion
}
