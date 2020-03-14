using System;
using System.Net;
using System.Net.Sockets;

public class SocketReceiver
{

    public static int Main(string[] args)
    {
        Listener();
        return 0;
    }


    public static void Listener()
    {
        TcpListener server = null;
        try
        {
            int port = 11000; //port tcp du serveur
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            // Buffer -> lit les données du SocketManager
            byte[] bytes = new byte[1024];
            string data = null;

            // Boucle d'écoute
            while (true)
            {
                Console.Write("En attente d'une connexion");
                
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connecté");
                data = null;
                bool dataAvailable = false;
                // Get a stream object for reading
                NetworkStream stream = client.GetStream();

                int i;
                while (true)
                {
                    if (!dataAvailable) //checker si les données sont dispo
                    {
                        dataAvailable = stream.DataAvailable;
                        if (server.Pending())
                        {
                            Console.WriteLine("Nouveau client");
                            break;
                        }
                    }

                    if (dataAvailable)
                    {
                        // Récupere toutes les données du client 
                        i = stream.Read(bytes, 0, bytes.Length);
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        string hotspotSSID_name = GetHotspotSSID(); //retourne le SSID du hotspot du hub
                        if (String.Compare(hotspotSSID_name, data) == 0)
                        {
                            EmulateNFC(data); //lance un programme d'émulation tag NFC, récupère les données de la BDD associée au nom du SSID du hub et place les données dans le tag
                            Console.WriteLine("SSID égal, émulation NFC lancée : {0}", data);
                        }
                        Console.WriteLine("SSID non égal, pas d'émulation NFC: {0}", data);
                        dataAvailable = false;
                    }

                    if (server.Pending())
                    {
                        Console.WriteLine("Nouveau client");
                        break;
                    }
                }

                Console.WriteLine("Client fermé");
                // Fermer connection
                client.Close();
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            // Arrêt d'écoute des clients
            Console.WriteLine("Arrêt d'écoute de clients.");
            server.Stop();
        }

    }

    private static string GetHotspotSSID()
    {
        //non implémenté => retourne le nom du SSID du hub
    }

    private static void EmulateNFC(string data)
    {
        //non implémenté, lance un script python utilisant le module nfcpy pour émuler un tag nfc contenant la data transmise en argument dans cette fonction
    }
}