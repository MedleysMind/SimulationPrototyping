using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour {
    public static GameObject AnimalPanel;
    public GameObject AnimalPanelReference;
    // Start is called before the first frame update
    void Start () {
        AnimalPanel = AnimalPanelReference;
    }

    // Update is called once per frame
    void Update () {

    }
}