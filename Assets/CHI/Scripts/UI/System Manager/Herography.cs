using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Herography : MonoBehaviour
{

    [Header("Ref Object")]
    [SerializeField] private Image avatar;
    [SerializeField] private TextMeshProUGUI alias;
    [SerializeField] private TextMeshProUGUI information;

    public void SetAvatar(Sprite avatar)
    {
        this.avatar.sprite = avatar;
    }

    public void SetAlias(string alias)
    {
        this.alias.text = alias;
    }

    public void SetStory(string information)
    {
        this.information.text = information;
    }
}
