using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Right_Click : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public UnityEvent rightClick;
	public UnityEvent rightUp;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			rightClick.Invoke ();
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
			rightUp.Invoke ();
	}
}