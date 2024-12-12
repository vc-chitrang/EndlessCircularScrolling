using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class InfiniteCarousel:MonoBehaviour {
	[SerializeField] private RectTransform _content;
	[SerializeField] private List<RectTransform> objectList = new List<RectTransform>();
	private List<RectTransform> concatenatedList = new List<RectTransform>();
	[SerializeField] private Button _nextButton;
	[SerializeField] private Button _previousButton;
	[SerializeField] private TextMeshProUGUI _status;
	private Tween _scaleTween;
	private RectTransform _currentSelectedElement;

	void Start() {
		InitializeContentList();
		DivideAndPrintArray();

		_nextButton.onClick.RemoveAllListeners();
		_previousButton.onClick.RemoveAllListeners();

		_nextButton.onClick.AddListener(() => {
			MoveElementsBackward(concatenatedList);
			RearrangeList();
			HighlightSelectedElement();
			UpdateUI();
			//Debug.Log("After moving forward: " + string.Join(", ", concatenatedList));
		});

		_previousButton.onClick.AddListener(() => {
			MoveElementsForward(concatenatedList);
			RearrangeList();
			HighlightSelectedElement();
			UpdateUI();
			//Debug.Log("After moving backward: " + string.Join(", ", concatenatedList));
		});

		HighlightSelectedElement();
		UpdateUI();
	}

	private void UpdateUI() {
		_status.text = $"{_currentSelectedElement.name}";
	}

	private void InitializeContentList() {
		objectList.Clear();
		int childCount = _content.childCount;
		for (int i = 0;i < childCount;i++) {
			objectList.Add(_content.GetChild(i) as RectTransform);
		}
	}

	public void DivideAndPrintArray() {
		int count = objectList.Count;
		int midIndex = count / 2;

		List<RectTransform> firstHalf = objectList.GetRange(0,midIndex);
		List<RectTransform> secondHalf = objectList.GetRange(midIndex,count - midIndex);

		concatenatedList = new List<RectTransform>(secondHalf);
		concatenatedList.AddRange(firstHalf);
		objectList = concatenatedList;

		RearrangeList();

		//Debug.Log("BEFORE Concatenated List: " + string.Join(", ", concatenatedList));
	}

	private void RearrangeList() {
		for (int i = 0;i < objectList.Count;i++) {
			Transform item = objectList[i];
			item.SetSiblingIndex(i);
		}
	}

	static void MoveElementsForward(List<RectTransform> list) {
		if (list.Count == 0)
			return;

		RectTransform lastElement = list[list.Count - 1]; // Store the last element

		for (int i = list.Count - 1;i > 0;i--) {
			list[i] = list[i - 1]; // Shift elements
		}

		list[0] = lastElement; // Place the last element at the front
	}

	static void MoveElementsBackward(List<RectTransform> list) {
		if (list.Count == 0)
			return;

		RectTransform firstElement = list[0]; // Store the first element

		for (int i = 0;i < list.Count - 1;i++) {
			list[i] = list[i + 1]; // Shift elements
		}

		list[list.Count - 1] = firstElement; // Place the first element at the end
	}

	private void HighlightSelectedElement() { 
		int count = objectList.Count;
		int midIndex = count / 2;
		if (objectList.Count == 0) {
			return;
		}

		ResetScale();
		_currentSelectedElement = objectList[midIndex];
		Scale(_currentSelectedElement,Vector3.one * 1.1f); 
	}

	private void ResetScale() {
		foreach (var item in objectList) {
			Scale(item,Vector3.one);
		}
	}

	internal void Scale(RectTransform rect, Vector3 scale) {
		// Complete and clear any active scale tween
		if (_scaleTween != null && _scaleTween.IsActive() && !_scaleTween.IsComplete()) {
			_scaleTween.Complete();
		}
	
		// Scale up to 1.1x
		_scaleTween = rect.DOScale(scale,0.2f)
								 .SetEase(Ease.OutQuad);
	}
}
