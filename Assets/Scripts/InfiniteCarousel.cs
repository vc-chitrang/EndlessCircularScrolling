using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class InfiniteCarousel : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private List<Transform> objectList = new List<Transform>();
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _previousButton;
    [SerializeField] private TextMeshProUGUI _status;
    private List<Transform> concatenatedList = new List<Transform>();

    private int _index = 0;

    public int Index
    {
        get
        {
            return _index;
        }
        set
        {
            _index = value;

            // Wrap around logic
            if (_index < 0)
            {
                _index = objectList.Count - 1;
            }
            else if (_index >= objectList.Count)
            {
                _index = 0;
            }
            if(objectList.Count == 0){
                return;
            }
            // Update the status text
            string message = objectList[_index].ToString();
            _status.text = $"{message}";            
        }
    }

    void Start()
    {
        DivideAndPrintArray();

        Index = 0; // Initialize to the first item
        _nextButton.onClick.RemoveAllListeners();
        _previousButton.onClick.RemoveAllListeners();

        _nextButton.onClick.AddListener(() =>
        {
            Index++;
            MoveElementsForward(concatenatedList);
            RearrangeList();
            //Debug.Log("After moving forward: " + string.Join(", ", concatenatedList));
        });

        _previousButton.onClick.AddListener(() =>
        {
            Index--;
            MoveElementsBackward(concatenatedList);
            RearrangeList();
            //Debug.Log("After moving backward: " + string.Join(", ", concatenatedList));
        });
    }

    public void DivideAndPrintArray()
    {
        int count = objectList.Count;
        int midIndex = count / 2;

        List<Transform> firstHalf = objectList.GetRange(0, midIndex);
        List<Transform> secondHalf = objectList.GetRange(midIndex, count - midIndex);

        concatenatedList = new List<Transform>(secondHalf);
        concatenatedList.AddRange(firstHalf);
        objectList = concatenatedList;
        
        RearrangeList();

        //Debug.Log("BEFORE Concatenated List: " + string.Join(", ", concatenatedList));
    }

    private void RearrangeList()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            Transform item = objectList[i];
            item.SetSiblingIndex(i);
        }
    }

    static void MoveElementsForward(List<Transform> list)
    {
        if (list.Count == 0) return;

        Transform lastElement = list[list.Count - 1]; // Store the last element

        for (int i = list.Count - 1; i > 0; i--)
        {
            list[i] = list[i - 1]; // Shift elements
        }

        list[0] = lastElement; // Place the last element at the front
    }

    static void MoveElementsBackward(List<Transform> list)
    {
        if (list.Count == 0) return;

        Transform firstElement = list[0]; // Store the first element

        for (int i = 0; i < list.Count - 1; i++)
        {
            list[i] = list[i + 1]; // Shift elements
        }

        list[list.Count - 1] = firstElement; // Place the first element at the end
    }

}
