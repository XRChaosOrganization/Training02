using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingUIComponent : MonoBehaviour
{
    public CanvasGroup canvasgroup;
    public TextMeshProUGUI currentAmount;
    public TextMeshProUGUI maxAmount;

    // Start is called before the first frame update
    public IEnumerator DisplayXpUP(int _currentAmount, int _maxAmount)
    {
        canvasgroup.alpha = 1;
        currentAmount.text = _currentAmount.ToString();
        maxAmount.text = "/" + _maxAmount.ToString();
        yield return new WaitForSeconds(2);
        canvasgroup.alpha = 0;

    }
}
