using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SocketManager 
{
    public static void Send(Socket socket, byte[] buffer, int offset, int size, int timeout)
    {
        int startTickCount = Environment.TickCount;
        int sent = 0;  // nombre de bytes déjà envoyés
        do
        {
            if (Environment.TickCount > startTickCount + timeout)
                throw new Exception("Erreur.");
            try
            {
                sent += socket.Send(buffer, offset + sent, size - sent, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.WouldBlock ||
                    ex.SocketErrorCode == SocketError.IOPending ||
                    ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                {
                    Thread.Sleep(30); //attente si le buffer du socket est rempli
                }
                else
                    throw ex;
            }
        } while (sent < size);
    }

}
