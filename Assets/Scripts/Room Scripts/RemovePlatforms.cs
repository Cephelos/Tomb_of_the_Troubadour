using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovePlatforms : MonoBehaviour
{
    public List<SpriteRenderer> platforms = new List<SpriteRenderer>();
    public List<SpriteRenderer> activePlatforms = new List<SpriteRenderer>();
    public float fadeTime; // Time in seconds until platforms fade out
    public Color platformColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        platforms.AddRange(transform.GetComponentsInChildren<SpriteRenderer>());
        activePlatforms.AddRange(platforms);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPlatforms() // Resets all platforms to full opacity and sets them active
    {
        foreach(SpriteRenderer platform in platforms)
        {
            platform.gameObject.SetActive(true);
            if(!activePlatforms.Contains(platform))
                activePlatforms.Add(platform);
            platform.color = platformColor;
        }
    }

    public void RemoveRandomPlatform(float intensity) // If platforms exist, removes one
    {
        Debug.Log("Removing a random platform!");
        if(activePlatforms.Count > 0)
        {
            StartCoroutine(RemovePlatform(activePlatforms[Random.Range(0, activePlatforms.Count - 1)]));
        }
    }

    public IEnumerator RemovePlatform(SpriteRenderer platform) // Fades out a platform, disabling it when fully transparent
    {

        float timeRemaining = fadeTime;
        Color finalColor = Color.clear;
        while (timeRemaining > 0)
        {
            float timeRatio = (fadeTime - timeRemaining)/ fadeTime;


            if (platform != null)
            {
                platform.color = new Color(Mathf.Lerp(platformColor.r, finalColor.r, timeRatio), Mathf.Lerp(platformColor.g, finalColor.g, timeRatio), Mathf.Lerp(platformColor.b, finalColor.b, timeRatio), Mathf.Lerp(platformColor.a, finalColor.a, timeRatio));

            }
            else
                yield break;
            yield return new WaitForEndOfFrame();
            timeRemaining -= Time.deltaTime;
        }
        Debug.Log("Platform removed!");
        platform.gameObject.SetActive(false);
        activePlatforms.Remove(platform);

    }
}
