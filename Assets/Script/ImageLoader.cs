using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageLoader : MonoBehaviour
{
    public string url;
    public Renderer thisRenderer;

    // automatically called when game started
    void Start()
    {
        //StartCoroutine(LoadFromLikeCoroutine()); // execute the section independently

        // the following will be called even before the load finished
        thisRenderer.material.color = Color.red;

        
    }

    //스트리밍 시작 
    public void StartStreaming()
    {
        string ip = GameManager.instance.Razig_IP.text;

        url = "http://" + ip + ":8091/?action=snapshot";

        StartCoroutine(LoadFromLikeCoroutine());
    }

    //스트리밍 정지 
    public void StopStreaming()
    {
        StopCoroutine(LoadFromLikeCoroutine());
    }

    // this section will be run independently
    private IEnumerator LoadFromLikeCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            //Debug.Log("Loading ....");
            WWW wwwLoader = new WWW(url);   // create WWW object pointing to the url
            yield return wwwLoader;         // start loading whatever in that url ( delay happens here )

            //Debug.Log("Loaded");
            thisRenderer.material.color = Color.white;              // set white
            thisRenderer.material.mainTexture = wwwLoader.texture;  // set loaded image
        }
    }
}
