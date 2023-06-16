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

    void Update()
    {
        //if(_bPlayerInArea)
        //{
        //    if(_emoteShopUi.activeSelf) 
        //    {
        //        if(Input.GetKey(KeyCode.Escape)) {
        //            OpenShopUi();
        //            _txtPressKey.enabled = true;
        //        }
        //    }
        //    else {
        //        if(Input.GetKey(KeyCode.E)) {
        //            CloseShopUi();
        //            _txtPressKey.enabled = false;
        //        }
        //    }
        //}
        //else // player not in area
        //{
        //    if(_emoteShopUi.activeSelf) {
        //        _emoteShopUi.SetActive(false);
        //        _txtPressKey.enabled = false;
        //    }
        //}
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
