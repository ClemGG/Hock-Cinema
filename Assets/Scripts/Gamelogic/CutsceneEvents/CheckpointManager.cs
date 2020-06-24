using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] UnityEvent onTeleportedToCheckpointEvent, onFirstDeathEvent;
    [SerializeField] Transform[] checkpoints;
    Vector3 lastCheckpoint;
    public static CheckpointManager instance;


    int curNbDeaths = 0;


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
        if(checkpoints != null)
        SetCheckpoint(checkpoints[0]);
    }


    public void SetCheckpoint(Transform t)
    {
        lastCheckpoint = t.position;
    }

    public void TeleportToLastCheckpoint(CharacterController player)
    {
        StartCoroutine(TeleportToLastCheckpointCo(player));
    }

    private IEnumerator TeleportToLastCheckpointCo(CharacterController player)
    {
        player.enabled = false;
        yield return StartCoroutine(SceneFader.instance.FadeOutCo());
        player.transform.position = lastCheckpoint;
        onTeleportedToCheckpointEvent?.Invoke();
        yield return new WaitForSeconds(1f);
        player.enabled = true;
        yield return StartCoroutine(SceneFader.instance.FadeInCo());

        curNbDeaths++;
        if(curNbDeaths == 1)
        {
            onFirstDeathEvent?.Invoke();
        }

    }
}
