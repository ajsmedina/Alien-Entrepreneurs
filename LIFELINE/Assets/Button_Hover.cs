using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Button_Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
	public UnityEvent hover;
	public UnityEvent exithover;

	public void OnPointerEnter(PointerEventData eventData)
	{
		hover.Invoke ();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		exithover.Invoke ();
	}

	public void OnSelect (BaseEventData data) 
	{
		hover.Invoke ();
	}

	public void OnDeselect (BaseEventData data) 
	{
		exithover.Invoke ();
	}
}