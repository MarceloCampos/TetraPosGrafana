using System.Collections.Concurrent;

public static class Globals
{
    public struct PosDataReceived
    {
        public long Timestamp;
        // public string SSI;
        public int SSI;
        public double Lat;
        public double Long;
        public double Vel;
        public double Dir;
        public int Precision;
        public ulong MessageId;
    }

    private static ConcurrentQueue<PosDataReceived> MessageQueue;

    public static bool MessageEnqueue(PosDataReceived pd)
    {
        int c = MessageQueue.Count;

        MessageQueue.Enqueue(pd);

        return MessageQueue.Count > c;
    }
    public static PosDataReceived MessageDequeue()
    {
        if(MessageQueue.TryDequeue(out PosDataReceived result))
        {
            return result;
        }

        return new PosDataReceived {SSI = 0};     // Ooooops errou ??!? então sinaliza através do SSI Empty, melhor tratamento ?
    }

    public static int GetMessageQueueCount()
    {
        if(MessageQueue == null)
            return 0;
            
        return MessageQueue.Count;
    }

    public static void MessageQueueInit()
    {
        MessageQueue = new ConcurrentQueue<PosDataReceived>();
    }
}