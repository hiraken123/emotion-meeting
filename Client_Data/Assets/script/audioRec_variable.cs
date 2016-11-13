using UnityEngine;
using System.Collections;

public class audioRec_variable : MonoBehaviour {

	int count = 0;
	float recTime = 5.0f;
	float second = 0;
	bool isRecording = false;

	public void OnStartRec(){
		isRecording = true;
		count = 0;
		OnRec ();
	}

	public void OnEndRec(){
		isRecording = false;
		OnStopRec();
	}

	//-----------------------------------

	public void OnRec(){
		this.gameObject.GetComponent<AudioSource>().clip = Microphone.Start (Microphone.devices [0], false, 6, 44100);

	}

	public void OnStopRec(){
		Microphone.End (Microphone.devices [0]);
		SavWav.Save(""+count, GetComponent<AudioSource>().clip);
	}

	void Update(){

		if (!isRecording)
			return;

		second += Time.deltaTime;
		if (second >= recTime) {
			count++;
			second = 0;
			OnStopRec();
			OnRec();
		}
	}


}
