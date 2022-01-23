using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowSocials : MonoBehaviour
{
    [SerializeField]
    private string _url;
    private void Awake() => GetComponent<Button>().onClick.AddListener(ShowSocialsWeb);
    private void ShowSocialsWeb() => Application.OpenURL(_url);
}
