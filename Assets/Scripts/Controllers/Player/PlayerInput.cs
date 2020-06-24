using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public string verticalAxisName = "Vertical";        
	public string horizontalAxisName = "Horizontal";    
	public string actionKey = "Action";    
	public string swapCameraKey = "SwapCam";                 
	public string talkieKey = "Talkie";
	public string pauseKey = "Pause";


	[HideInInspector] public bool shouldAction = false;
	[HideInInspector] public bool actionStopped = false;
	[HideInInspector] public bool isActing = false;
	[HideInInspector] public bool shouldSwapCam = true;
	[HideInInspector] public bool shouldTalkie = false;
	[HideInInspector] public bool hasPressedPause = false;



	void Update()
	{

		shouldAction = Input.GetButtonDown(actionKey);
		actionStopped = Input.GetButtonUp(actionKey);
		isActing = Input.GetButton(actionKey);

		shouldSwapCam = Input.GetButtonDown(swapCameraKey);
		shouldTalkie = Input.GetButtonDown(talkieKey);

		hasPressedPause = Input.GetButtonDown(pauseKey);

	}
}
