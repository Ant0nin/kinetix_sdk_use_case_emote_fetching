using Kinetix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteCloudApi : MonoBehaviour
{
    public string _userId;

    // Start is called before the first frame update
    void Start()
    {
        KinetixCore.Account.ConnectAccount(_userId);
    }
}
