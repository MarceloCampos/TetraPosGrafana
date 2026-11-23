using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using TetraPosGrafana;
public class MessageDeQueuing
{
    public async Task ExecutaPollDequeueLoop()
    {
        bool isToExit = false;

        while(!isToExit) 
        {
            while (Globals.GetMessageQueueCount() > 0)
            {
                var posD = Globals.MessageDequeue();    

                if(posD.SSI != 0)
                {
                    bool resultado = await SendToInflux.UploadMetricAsync(posD).WaitAsync(TimeSpan.FromSeconds(3)); 

                    if(Program.opcoes.DebugOut_PosData > 0)
                        Console.WriteLine($"{DateTime.Now}\t{posD.SSI}\t{posD.Lat}\t{posD.Long}\t{posD.Vel}\t{(resultado == true? "OK": "Erro")}\t{posD.MessageId}\t{Globals.GetMessageQueueCount()}");                                                             
                }
                else    
                {
                    Console.WriteLine($"{DateTime.Now}\tERRO TryDequeue");
                }                   
                Yield();
            }
            Yield();
        }          
    }
    private static void Yield()
    {
        Thread.Sleep(10);
    }   
}

