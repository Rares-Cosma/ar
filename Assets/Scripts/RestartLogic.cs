using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class RestartLogic : MonoBehaviour
{
    Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        StartCoroutine(Quit());
    }

    IEnumerator Quit()
    {
        yield return new WaitForSeconds(0.1f);
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
