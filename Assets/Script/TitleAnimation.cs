using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    private string titleString = "Organic Food Network";
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var length = titleString.Length;
        while (true)
        {
            for (int i = 0; i <= length; i++)
            {
                title.text = titleString.Substring(0, i);
                yield return new WaitForSeconds(0.2f);
            }
            for (int i = length; i >= 0; i--)
            {
                title.text = titleString.Substring(0,i);
                yield return new WaitForSeconds(0.2f);
            }

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
