using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePanel : MonoBehaviour
{
    //I do it fast, so I use crutches.
    [SerializeField] private GameObject _firstPanel;
    [SerializeField] private GameObject _secondPanel;
    [SerializeField] private GameObject _thirdPanel;

    public void Start()
    {
        _firstPanel.SetActive(true);
        _secondPanel.SetActive(false);
        _thirdPanel.SetActive(false);
    }

    public void ActiveSecondPanel()
    {
        _firstPanel.SetActive(false);
        _secondPanel.SetActive(true);
        _thirdPanel.SetActive(false);
    }

    public void ActiveThirdPanel()
    {
        _firstPanel.SetActive(false);
        _secondPanel.SetActive(false);
        _thirdPanel.SetActive(true);
    }
}
