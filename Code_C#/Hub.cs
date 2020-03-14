using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HubDataType
{
    image, audio
}; //enumération du type de données, pour s'assurer qu'on associe pas des components (audio comme sprite) inutilement

public class Hub : MonoBehaviour
{
    public HubDataType dataType
    {
        get
        {
            return dataType;
        }
        protected set //on ne veut pas set le type du hub en dehors de celui-ci mais on souhaite la récupérer en dehors pour ajouter les bons fichiers
        {
            dataType = value;
        }
    }
    public AudioManager audioData;
    public ImageManager imageData;
    public int hub_id; //l'id du hub
    public Module enigme; //enigme du hub
    public bool isActive; // le hub est-elle active ? (désactivée lorsque énigme terminée et recompense donnée)
    public string hubWiFiName; //nom du wi-fi du hub

    public Hub(int hub_id, Module enigme, string HubType, string path, string wifiName)
    {
        this.isActive = true;
        this.hub_id = hub_id;
        this.enigme = enigme;
        this.hubWiFiName = wifiName;
        switch (HubType)
        {
            case "audio":
                dataType = HubDataType.audio;
                audioData.chargeurSon.LoadWithUrl(path);
                break;
            case "image":
                dataType = HubDataType.image;
                imageData.chargeurImage.LoadWithUrl(path);
                break;
        }
        
    }
}
