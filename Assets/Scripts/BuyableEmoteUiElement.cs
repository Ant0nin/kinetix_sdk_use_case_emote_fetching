using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
using Kinetix;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

public class BuyableEmoteUiElement : MonoBehaviour
{
    public EmoteData _emoteData; // contains UUID + price
    private string _emoteName = null;
    private string _emotePngImageUrl = null;
    private bool emoteTextureLoaded = false;
    private bool _mustRefreshUiData = false;
    private UnityWebRequest _textureRequest = null;

    public Image _imgEmote;
    public Button _btnBuy;
    public TMP_Text _txtEmoteLabel;
    public TMP_Text _txtEmoteCost;


    void Start()
    {
        Assert.IsNotNull(_emoteData);
        Assert.IsNotNull(_imgEmote);
        Assert.IsNotNull(_btnBuy);
        Assert.IsNotNull(_txtEmoteLabel);
        Assert.IsNotNull(_txtEmoteCost);

        _txtEmoteCost.SetText(_emoteData.price.ToString()+" $");

        Task.Run(FetchAndDisplayEmoteDataAsync);
    }

    private void Update()
    {
        if(_mustRefreshUiData)
        {
            _mustRefreshUiData = false;

            _txtEmoteLabel.SetText(_emoteName);

            _textureRequest = UnityWebRequestTexture.GetTexture(_emotePngImageUrl);
            _textureRequest.SendWebRequest();
            if (_textureRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(_textureRequest.error);
                _textureRequest = null;
            }
        }

        if(!emoteTextureLoaded && (_textureRequest != null) && _textureRequest.isDone)
        {
            Texture2D texture = ((DownloadHandlerTexture)_textureRequest.downloadHandler).texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());
            _imgEmote.sprite = sprite;
            emoteTextureLoaded = true;
        }
    }

    private async void FetchAndDisplayEmoteDataAsync()
    {
        JObject emote = await KinetixRestApiClient.Instance.GetEmoteAsync(_emoteData.uuid);
        if(emote != null)
        {
            _emoteName = (string)emote["name"];

            _emotePngImageUrl = null;
            foreach(var fileData in emote["files"]) {
                if((string)fileData["extension"] == "png")
                {
                    _emotePngImageUrl = (string)fileData["url"];
                    break;
                }
            }
            Assert.IsNotNull(_emotePngImageUrl);

            _mustRefreshUiData = true;
        }
        else {
            Debug.LogError("Unable to fetch JSON data (Kinetix Rest API) from following emote UUID: " + _emoteData.uuid);
        }
    }

    public void BuyEmote()
    {
        _txtEmoteCost.SetText("owned!");
        _btnBuy.enabled = false;
        KinetixCore.Account.AssociateEmotesToUser(_emoteData.uuid);
    }
}
