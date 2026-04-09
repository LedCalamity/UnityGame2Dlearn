using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropdownResolution : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown_res;
    Resolution[] resos;
    void Start()
    {
        dropdown_res.onValueChanged.AddListener(ConvertResolution);
        resos = Screen.resolutions; //get available resolutions
        List<string> options = new List<string>();
        int cur_pos = 0;
        for (int i = 0; i < resos.Length; i++) 
        {
            string option = resos[i].width + "x" + resos[i].height;
            options.Add(option);
            if (resos[i].width == Screen.currentResolution.width && resos[i].height == Screen.currentResolution.height)
            {
                cur_pos = i;
            }
        }
        dropdown_res.AddOptions(options); //add options and show current resolution as default
        dropdown_res.value = cur_pos;
        dropdown_res.RefreshShownValue();
    }

    void ConvertResolution(int pos)
    {
        Screen.SetResolution(resos[pos].width, resos[pos].height,Screen.fullScreen);
    }
}
