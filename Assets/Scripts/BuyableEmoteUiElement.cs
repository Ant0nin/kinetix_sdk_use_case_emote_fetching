using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Assertions;
using Kinetix;

public class BuyableEmoteUiElement : MonoBehaviour
{
    public EmoteData _emoteData;
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

        _txtEmoteLabel.SetText(_emoteData.label);
        _txtEmoteCost.SetText(_emoteData.cost.ToString()+" $");
    }

    public void BuyEmote()
    {
        _txtEmoteCost.SetText("owned!");
        _btnBuy.enabled = false;
        KinetixCore.Account.AssociateEmotesToUser(_emoteData.uuid);
    }
}
