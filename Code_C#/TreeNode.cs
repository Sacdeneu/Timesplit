using System.Collections.Generic;

public delegate void TreeVisitor<T>(string playerType, T nodeData, int nodeLevel, LinkedList<TreeNode<T>> children); //delegué appelé sans récursivité pour traverser l'arbre
public class TreeNode<T>
{
    private T data; //donnée de l'arbre
    private int level; //level de l'arbre (racine = 0)
    private LinkedList<TreeNode<T>> children; //enfant de l'arbre

    //CONSTRUCTEUR -> ajoute donnée et crée un enfant -> a utiliser à l'initialisation (racine)
    public TreeNode(T data)
    {
        this.data = data;
        this.level = 0;
        children = new LinkedList<TreeNode<T>>();
    }

    //CONSTRUCTEUR -> ajoute donnée et crée un enfant (lui ajoute +1 a son level) -> a utiliser depuis le parent
    public TreeNode(T data, int level)
    {
        this.data = data;
        this.level = level+1;
        children = new LinkedList<TreeNode<T>>();
    }

    //Ajoute un noeud enfant au début de la linked list du parent
    public void AddChild(T data, TreeNode<T> parent)
    {
        children.AddFirst(new TreeNode<T>(data, parent.level));
    }

    //retourne le i-ème noeud enfant
    public TreeNode<T> GetChild(int i)
    {
        foreach (TreeNode<T> n in children)
            if (--i == 0)
                return n;
        return null;
    }

    //méthode récursive : traverse l'arbre de haut en bas
    public void Traverse(string playerType, TreeNode<T> node, TreeVisitor<T> visitor)
    {
        visitor(playerType, node.data, node.level, node.children);
        foreach (TreeNode<T> noeud in node.children)
            Traverse(playerType, noeud, visitor);
    }
}