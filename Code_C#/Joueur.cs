using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeJoueur
{
    MJ, Joueur
}; //type de joueur (MJ ou Joueur)

public class Joueur : MonoBehaviour
{
    private string nomJoueur; //nom du joueur
    public Inventaire casier; //casier du joueur
    private TypeJoueur typeJoueur;

    public Joueur(string nomJoueur, Inventaire casier, TypeJoueur typeJoueur)
    {
        this.nomJoueur = nomJoueur;
        this.casier = casier;
        this.typeJoueur = typeJoueur;
    }
}
