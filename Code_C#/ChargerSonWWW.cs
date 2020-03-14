using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChargerSonWWW : MonoBehaviour
{
    public string url;
    public string audioName;
    public AudioClip audioClip;
    public AudioSource source;
    public string uploadSonURL = "localhost:80/TimeSplit/InsertSon.php"; //upload son URL (un script pour MJ, un pour joueur)

    public void UploadSon()
    {
        StartCoroutine(UploadWAV());
    }


    private void Awake()
    {
        source = gameObject.AddComponent<AudioSource>();
    }

    public void LoadWithUrl(string audioPath)
    {
        audioPath = url;
        StartCoroutine(LoadAudio());
    }

    IEnumerator UploadWAV()
    {
        string name = "hub_son.wav"; //nom du fichier stocké en BDD
        AudioClip temp_Snd = audioClip;

        // Encodage, WAV => BYTE[]
        byte[] bytes = WavUtility.FromAudioClip(temp_Snd); //utilisation de la librairie WavUtility pour convertir audioclip en byte array
        Destroy(temp_Snd);

        // Creation WWWForm
        WWWForm form = new WWWForm();
        form.AddBinaryData("son", bytes, name, "audio/x-wav");

        // Upload via Script PHP
        using (var w = UnityWebRequest.Post(uploadSonURL, form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
            }
            else
            {
                print("Musique correctement uploadée");
            }
        }
    }



    private IEnumerator LoadAudio()
    {
        WWW request = GetAudioFromFile(url);
        yield return request;

        audioClip = request.GetAudioClip();
        audioClip.name = audioName;
    }

    private void PlayAudioFile()
    {
        source.clip = audioClip;
        source.Play();
    }



    private WWW GetAudioFromFile(string path)
    {
        WWW request = new WWW(path);
        return request;
    }
    

}
