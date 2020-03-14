using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class SQLDataInsert
{
    private static string CreateArboURLMJ = "localhost:80/TimeSplit/InsertHubMJ.php"; //mettre le chemin du fichier PHP sur le Raspberry du hub maître
    private static string CreateArboURLJoueur = "localhost:80/TimeSplit/InsertHubJoueur.php"; //idem que pour CreateArboURLMJ

    public static void CreerArborescenceJoueur(TreeNode<Hub> arboJoueur)
    {
        string playerType = "Joueur";
        TreeVisitor<Hub> my_visitor = my_visitor_impl;
        arboJoueur.Traverse(playerType, arboJoueur, my_visitor);
    }


    //Le visiteur récupère un hub à chaque itération (du haut en bas de l'arbre) et crée dans la BDD le contenu associé au hub
    public static void CreerArborescenceMJ(TreeNode<Hub> arboMJ)
    {
        string playerType = "MJ";
        TreeVisitor<Hub> my_visitor = my_visitor_impl;
        arboMJ.Traverse(playerType, arboMJ, my_visitor);
    }

    //Classe visiteur, copie à chaque itération un hub et ses données, et l'ajoute à la BDD
    static void my_visitor_impl(string playerType, Hub hub_visitor, int nodeLevel, LinkedList<TreeNode<Hub>> hub_children)
    {
        CreerHub(playerType, hub_visitor.hub_id.ToString(), nodeLevel.ToString(), hub_children.ToString(), hub_visitor.enigme.ToString(), hub_visitor.hubWiFiName.ToString(), hub_visitor);
    }

    public static IEnumerator CreerHub(string playerType, string hubID, string hubLevel, string hubChildren, string hubEnigme, string hubWifiName, Hub hubaCreer)
    {
        //Ajout des premiers champs
        WWWForm form = new WWWForm();
        form.AddField("hubIDPost", hubID);
        form.AddField("hubLevelPost", hubLevel);
        form.AddField("hubChildrenPost", hubChildren);
        form.AddField("hubEnigmePost", hubEnigme);
        form.AddField("hubWiFiNamePost", hubWifiName);

        //Ajout de la récompense (s'il y en a une)
        switch (hubaCreer.dataType)
        {
            case HubDataType.image:
                hubaCreer.imageData.chargeurImage.UploadImage(); //l'image a déjà été chargée lors de l'instanciation du hub, on upload donc directement
                break;
            case HubDataType.audio:
                hubaCreer.audioData.chargeurSon.UploadSon();
                break;
            default:
                break;
        }

        string CreateARBOURL = "";
        switch (playerType)
        {
            case "MJ":
                CreateARBOURL = CreateArboURLMJ;
                break;
            case "Joueur":
                CreateARBOURL = CreateArboURLJoueur;
                break;
            default:
                break;
        }
        var download = UnityWebRequest.Post(CreateARBOURL, form);

        // On attend que le DL soit terminé
        yield return download.SendWebRequest();

        if (download.isNetworkError || download.isHttpError)
        {
            Debug.Log("Erreur d'upload: " + download.error);
        }
        else
        {
            Debug.Log(download.downloadHandler.text);
        }

        //CREATION SOCKET ET COMMUNICATION TCP AVEC LES HUBS
        TcpClient tcpClient = new TcpClient("localhost", 11000);
        Socket socket = tcpClient.Client;
        string SSID_Hub_msg = hubWifiName;
        try
        {
            SocketManager.Send(socket, Encoding.UTF8.GetBytes(SSID_Hub_msg), 0, SSID_Hub_msg.Length, 5000);
        }
        catch (Exception ex) {
            Debug.Log(ex);
        }
    }
}
