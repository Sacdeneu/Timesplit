using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int nb_hubs; //nombre de hubs placées
    public static GameManager singleton; //singleton pattern pour récuper l'instance du gamemanager dans d'autres scripts
    public Joueur[] ListeJoueurs; //listes des joueurs (MJ et Joueurs)
    public Joueur currentPlayer; //Instance du joueur qui joue sur cet appareil (pour le distinguer des autres et faire des opérations directes sur son casier)
    public TreeNode<Hub> arbreMJ; //arbre du MJ
    public TreeNode<Hub> arbreJoueurs; //arbre des joueurs
    public bool isEnigmaTriggered; //si une énigme est en cours on ne peut pas en lancer d'autre

    private void Awake()
    {
        singleton = this;
    }

    void my_visitor_impl(string datum)
    {
        Debug.Log(datum);
    }

    public void Update()
    {
        CheckArborescence();
        CheckARSignal();
        CheckNFCContact();
    }

    private void CheckARSignal()
    {
        //non implémenté => vérifie si un symbole de réalité augmenté est détecté
    }

    private void CheckArborescence()
    {
        //non implémenté => vérifie s'il y a un changement dans l'arborescence des joueurs et si les conditions de victoire sont réunies
    }

    public void Start()
    {
        isEnigmaTriggered = false;
        //INSTANCIATION PREMIER HUB + ARBORESCENCE
        Hub FirstHub = InitialiserPremierHub();
        arbreMJ = new TreeNode<Hub>(FirstHub);
        arbreJoueurs = new TreeNode<Hub>(FirstHub);
        SQLDataInsert.CreerArborescenceMJ(arbreMJ);
        SQLDataInsert.CreerArborescenceJoueur(arbreJoueurs);
    }


    //méthode de création du premier hub au lancement de la partie, implémentation similaire pour les autres hubs
    private Hub InitialiserPremierHub()
    {
        Hub premHub; //premier hub a créer
        
        GameObject fieldpremHub = GameObject.FindWithTag("premHub"); //on retrouve le gameobject avec les champs a remplir pour le premier hub
        Module premEnigme;
        switch (fieldpremHub.GetComponent<FieldHub>().enigme.ToString())
        {
            case "code4chiffres":
                premEnigme = new ModuleCode4Chiffres();
                break;
            default:
                premEnigme = new ModuleDefault();
                break;
        }

        GameObject simplefilebrowser = GameObject.FindWithTag("simplefilebrowser");
        GameObject Wifibrowser = GameObject.FindWithTag("wifibrowser");
        string premEnigmePath = simplefilebrowser.GetComponent<SimpleFileBrowser>().getPath();
        string premHubWifi = Wifibrowser.GetComponent<WifiBrowser>.getWiFiName();
        premHub = new Hub(nb_hubs, premEnigme, fieldpremHub.GetComponent<FieldHub>().recompense.ToString(), premEnigmePath, premHubWifi);
        ++nb_hubs;

        return premHub;
    }


    private void CheckNFCContact()
    {
        //non implémenté => vérifie s'il y a un contact NFC avec un hub (en utilisant le script Near-Field-Communication-NFC-For-Unity-Android ou IOS At NFC)
    }

}
