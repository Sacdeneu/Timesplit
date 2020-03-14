using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChargerImageWWW : MonoBehaviour
{

    public string url;
    public Renderer imageRenderer;
    public Texture2D img_Tex;
    public int width = 200; //images de 200x200 (carrées, format photo)
    public int height = 200;
    public string uploadImageURL = "localhost:80/TimeSplit/InsertImage.php"; //upload image URL (un script pour MJ, un pour joueur)


    private void Awake()
    {
        imageRenderer = gameObject.AddComponent<Renderer>();
        img_Tex = new Texture2D(width, height, TextureFormat.RGB24, false);
    }

    public void LoadWithUrl(string imagePath)
    {
        imagePath = url;
        StartCoroutine(LoadImage());
    }

    public void UploadImage()
    {
        StartCoroutine(UploadPNG());
    }


    private IEnumerator LoadImage()
    {
        WWW wwwLoader = GetImageFromFile(url); 
        yield return wwwLoader;        

        Debug.Log("Loaded");
        imageRenderer.material.color = Color.white;
        imageRenderer.material.mainTexture = wwwLoader.texture;
        img_Tex = wwwLoader.texture;
    }

    private WWW GetImageFromFile(string path)
    {
        WWW request = new WWW(path);
        return request;
    }

    IEnumerator UploadPNG()
    {
        string name = "hub_image.png"; //nom du fichier stocké en BDD
        Texture2D temp_Img = img_Tex;
        temp_Img.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        temp_Img.Apply();

        // Encodage, texture => PNG
        byte[] bytes = temp_Img.EncodeToPNG();
        Destroy(temp_Img);

        // Creation WWWForm
        WWWForm form = new WWWForm();
        form.AddBinaryData("image", bytes, name, "image/png");

        // Upload via Script PHP
        using (var w = UnityWebRequest.Post(uploadImageURL, form))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                print(w.error);
            }
            else
            {
                print("Image correctement uploadée");
            }
        }
    }


}
