using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayingCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private Button useButton;

    private int cardStrength;
    //private VisitorType cardType;

    public int Strength => cardStrength;


    private void OnEnable()
    {
        useButton.onClick.AddListener(OnButtonClick);
        useButton.enabled = false;
    }

    private void OnDisable()
    {
        useButton.onClick.RemoveListener(OnButtonClick);
    }

    //public void Init(VisitorType type, int str)
    //{
    //    cardStrength = str;
    //    cardType = type;
    //    UpdateText();
    //}
    //private void UpdateText()
    //{
    //    typeText.text = "Type: " + cardType.ToString();
    //    strengthText.text = "Strength: " + cardStrength.ToString();
    //}

    private void OnButtonClick()
    {
        TavernEventsManager.PlayerChoseCard(this);
    }

    public void SetButtonEnebled(bool isButtonEnabled)
    {
        useButton.enabled = isButtonEnabled;
    }

    public void SetStrength(int newValue)
    {
        cardStrength = newValue;
        //UpdateText();
    }
}
