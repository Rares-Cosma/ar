using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    public RectTransform healthPanel;
    public GameObject deathScreen;
    float panelWidth = 780;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health,0,100);
        float healthPx = (health/100)*780;
        Vector2 newSize = new Vector2(healthPx,healthPanel.sizeDelta.y);
        healthPanel.sizeDelta = Vector2.Lerp(healthPanel.sizeDelta,newSize,.1f);
        if(health == 0)
        {
            deathScreen.SetActive(true);
        }
    }
}
