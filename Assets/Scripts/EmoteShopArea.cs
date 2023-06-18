using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class EmoteShopArea : MonoBehaviour
{
    public GameObject _emoteShopUi;
    public CustomCharacterController _playercharacterController;

    void Start()
    {
        Assert.IsNotNull(_emoteShopUi);
        _emoteShopUi.SetActive(false);
        Assert.IsNotNull(_playercharacterController);
    }

    public void OpenShopUi() {
        _emoteShopUi.SetActive(true);
    }

    public void CloseShopUi() {
        _emoteShopUi.SetActive(false);
    }

    public bool IsShopUiOpen() {
        return _emoteShopUi.activeSelf;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") {
            _playercharacterController.OnEnterEmoteShopArea(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            _playercharacterController.OnExitEmoteShopArea(this);
        }
    }
}
