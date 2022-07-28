using UnityEngine;

public class DoorView : MonoBehaviour
{
	public string buttonConfigId;

	[SerializeField] private Transform _movedObject;

	private Vector3 _startPosition;
	private Vector3 _endPosition;

	void Start()
	{
		_startPosition = _movedObject.transform.position;
		_endPosition = _startPosition + 7 * Vector3.left;
	}
	
	public void UpdateProgress(float progress)
	{
		_movedObject.transform.position = Vector3.Lerp(_startPosition, _endPosition, progress);
	}
}
