using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;
using System.IO;
using UnityEngine.Networking;

public class TfLiteManagerHolistic : MonoBehaviour
{
	public static TfLiteManagerHolistic Instance;

	[SerializeField, FilePopup("*.tflite")] string modelName;

	[HideInInspector]
	public float?[,,] input;

	[HideInInspector]
	public float[] outputs = new float[250];

	public int maxFrames;

	[HideInInspector]
	public bool isCapturingMediaPipeData = false;

	[HideInInspector]
	public int sessionNumber = 0;

	[HideInInspector]
	public int recordingFrameNumber = 0;

	public static string[] LABELS = { "dad", "elephant", "red", "where", "yellow" };

	private Interpreter interpreter;
	private float timer = 0f;

	public Queue<float?[,]> allData = new Queue<float?[,]>();

	[HideInInspector]
	public bool isWaitingForResponse = false;
	[HideInInspector]
	public bool isResponseReady = false;



	// Start is called before the first frame update
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		var options = new InterpreterOptions()
		{
			threads = 1,
		};
		interpreter = new Interpreter(FileUtil.LoadFile(modelName), options);
		if(interpreter.GetInputTensorInfo(0).shape[0] > 0)
			maxFrames = interpreter.GetInputTensorInfo(0).shape[1];
		else if(maxFrames == 0)
        {
			maxFrames = 30;
        }
	}

	public void AddDataToList(float?[,] singleFrameData)
	{
		allData.Enqueue(singleFrameData);
		if (allData.Count > maxFrames)
		{
			allData.Dequeue();
		}
	}

	public void StartRecording()
	{
		//Clear Data
		input = new float?[maxFrames, 543, 3];
		allData = new Queue<float?[,]>();
		isCapturingMediaPipeData = true;
		sessionNumber++;
		recordingFrameNumber = 0;
	}

	public string StopRecording()
	{
		isCapturingMediaPipeData = false;
		timer = 0;

		//StartCoroutine(ReadFile());
		return RunModel();
	}

	private IEnumerator ReadFile()
	{
		yield return new WaitForEndOfFrame();
		string path = Application.persistentDataPath + "/" + sessionNumber + "_landmarks.txt"; //dir to be changed accordingly
		StreamWriter sWriter = new StreamWriter(path, true);
		sWriter.Write("}");
		sWriter.Close();
	}

	private string RunModel()
	{
		outputs = new float[250];

		//For now we aren't padding data
		//if (allData.Count < maxFrames)
		//{
		//	var middleData = allData.[allData.Count / 2];
		//	int middleDataIndex = allData.Count / 2;
		//	int framesToAdd = maxFrames - allData.Count;
		//	for (int i = 0; i < framesToAdd; i++)
		//	{
		//		allData.Insert(middleDataIndex, middleData);
		//	}
		//}

		int dataRecordedSize = allData.Count;
		input = new float?[dataRecordedSize, 543, 3];

		for (int frameNumber = 0; frameNumber < dataRecordedSize; frameNumber++)
		{
			var currentFrameData = allData.Dequeue();

			for (int mediapipevalue = 0; mediapipevalue < 543; mediapipevalue++)
			{
				//This can be optimized later
				input[frameNumber, mediapipevalue, 0] = currentFrameData[mediapipevalue, 0];
				input[frameNumber, mediapipevalue, 1] = currentFrameData[mediapipevalue, 1];
				input[frameNumber, mediapipevalue, 2] = currentFrameData[mediapipevalue, 2];
			}
		}

		var options = new InterpreterOptions()
		{
			threads = 1,
		};
		interpreter = new Interpreter(FileUtil.LoadFile(modelName), options);

		var info = interpreter.GetInputTensorInfo(0);

		// Allocate input buffer
		interpreter.AllocateTensors();

		interpreter.SetInputTensorData(0, input);

		// Blackbox!!
		interpreter.Invoke();

		// Debug.Log("Output index " + interpreter.GetOutputTensorIndex(20));

		// Get data
		interpreter.GetOutputTensorData(0, outputs);

		//label1: 
		float max = 0f;
		string answer = "";
		for (int i = 0; i < outputs.Length; i++)
		{
			if (outputs[i] > max)
			{
				max = outputs[i];
				answer = "" + i;

			}
		}

		Debug.Log("Max Probability " + max);
		Debug.Log("results!!!!!!!!!!!!!!!!!! " + answer);

		return answer;
	}

	private void Update()
	{
		if (TfLiteManagerHolistic.Instance.isCapturingMediaPipeData)
		{
			timer += Time.deltaTime;
		}
	}
}
