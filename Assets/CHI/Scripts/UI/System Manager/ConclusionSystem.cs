using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ConclusionSystem : MonoBehaviour
{

    [Header("Image Object")]
    [SerializeField] private Image objWinner;
    [SerializeField] private Image objLoser;

    [Header("Conclusion Text")]
    [SerializeField] TextMeshProUGUI conclusionTextObject;

    [Header("Loading Bar")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI progressText;

    

    public void SetWinnerSprite(Sprite winner)
    {
        objWinner.sprite = winner;
    }

    public void SetLoserSprite(Sprite loser)
    {
        objLoser.sprite = loser;
    }

    public void SetConclusionText(string text)
    {
        conclusionTextObject.text = text;
    }

    public Slider GetSlider => slider;

    public TextMeshProUGUI GetProgressText => progressText;

}
