using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[System.Serializable]
public class AudioController : MonoBehaviour
{
    public List<string> audioClipNames = new List<string>();
    public List<AudioClip> audioClips = new List<AudioClip>();
    public List<AudioSource> audioSources = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPlayingSFX(string Name)
    {
        AudioSource audioSource = audioSources[audioClipNames.IndexOf(Name)];

        return audioSource.isPlaying;
            
    }

    public void PlaySFX(string Name, bool randomizePitchVol = true, bool loop = false)
    {
        AudioSource audioSource = audioSources[audioClipNames.IndexOf(Name)];
        audioSource.clip = audioClips[audioClipNames.IndexOf(Name)];
        if(randomizePitchVol)
        {
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            audioSource.volume = Random.Range(0.8f, 1.1f);
            
        }
        else
        {
            audioSource.pitch = 1f;
            audioSource.volume = 1f;
        }
        if (loop)
            audioSource.loop = true;
        audioSource.Play();
    }

    public void StopSFX(string Name)
    {
        AudioSource audioSource = audioSources[audioClipNames.IndexOf(Name)];
        audioSource.Stop();
    }

    public void PlayMusic(string Name)
    {
        AudioSource audioSource = audioSources[audioClipNames.IndexOf(Name)];
        audioSource.clip = audioClips[audioClipNames.IndexOf(Name)];
        audioSource.loop = true;
        audioSource.Play();
        
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(AudioController))]
public class AudioControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AudioController controller = (AudioController)target;
        for(int i = 0; i < controller.audioClips.Count; i++)
        {
            GUILayout.BeginHorizontal();
            controller.audioClipNames[i] = GUILayout.TextField(controller.audioClipNames[i]);


            EditorGUI.BeginChangeCheck();
            controller.audioClips[i] = (AudioClip)EditorGUILayout.ObjectField(controller.audioClips[i], typeof(AudioClip), false);
            if(EditorGUI.EndChangeCheck())
            {
                controller.audioSources[i].clip = controller.audioClips[i];
            }

            GUILayout.EndHorizontal();
            if(GUILayout.Button("Remove Clip"))
            {
                controller.audioClips.RemoveAt(i);
                controller.audioClipNames.RemoveAt(i);
                Destroy(controller.audioSources[i].gameObject);
                controller.audioSources.RemoveAt(i);
            }
        }
        if(GUILayout.Button("Add Clip")) // When we add a clip to the list, create a new gameObject childed to the controller
        {
            controller.audioClips.Add(null);
            controller.audioClipNames.Add("New Audio Clip " + controller.audioClipNames.Count);
            GameObject newSourceObj = new GameObject("New Audio Clip " + controller.audioClipNames.Count, typeof(AudioSource));
            newSourceObj.transform.SetParent(controller.transform);
            AudioSource newSource = newSourceObj.GetComponent<AudioSource>();
            newSource.playOnAwake = false;
            controller.audioSources.Add(newSource);
        }

        if(GUI.changed)
        {
            // Need to set dirty!
            Debug.Log("Setting dirty!");
            EditorUtility.SetDirty(controller);
            EditorSceneManager.MarkSceneDirty(controller.gameObject.scene);
        }
        
    }
}

#endif
