using System.Collections;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

public class CloudSaveScript : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void SaveData(string saveKey, string saveValue)
    {
        var data = new Dictionary<string, object> { { saveKey, saveValue } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }

}
