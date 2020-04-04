using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consequence : MonoBehaviour
{

    [SerializeField] private Destiny destiny = Destiny.miserable;

    private enum Destiny
    {
        miserable,
        original
    }

    private void Awake()
    {
        PlayerPrefs.SetString("Consequence", destiny.ToString());
    }
}
