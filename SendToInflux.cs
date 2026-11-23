using System.Text;
using InfluxData.Net.InfluxDb.Models;
using TetraPosGrafana;
public static class SendToInflux
{   
    private static InfluxLayer influxLayer;

    public static bool RECORD_METRIC_W_TIMESTAMP = false;

    public static bool InitConn()
    {
        influxLayer = new InfluxLayer();
        return true ; // sempre true ...
    }
    public static async Task<bool> UploadMetricAsync(Globals.PosDataReceived ld)
    {   
        InfluxData.Net.InfluxDb.Models.Point pointToWrite = ConvertToPointToWrite(ld);

        if (RECORD_METRIC_W_TIMESTAMP)
            pointToWrite.Timestamp = DateTime.UtcNow;
                
        await influxLayer.WriteAsync(pointToWrite, Program.opcoes.Database);

        return true; 
    }
        

    private static InfluxData.Net.InfluxDb.Models.Point ConvertToPointToWrite(Globals.PosDataReceived ld)
    {
        InfluxData.Net.InfluxDb.Models.Point pointToWrite = new InfluxData.Net.InfluxDb.Models.Point()
        {
            Name = "PositionData", // serie/measurement/table to write into
            Tags = new Dictionary<string, object>()
            {
                { "GatewayId", Program.opcoes.GatewayId },
                { "Remarks", "Gen_by_TetraPosGrafana" },
                { "SSI", ld.SSI }
            },
            Fields = new Dictionary<string, object>()
            {             
                { "lat", ld.Lat },
                { "lon", ld.Long },
                { "vel", ld.Vel },
                { "acc", ld.Precision },
                { "dir", ld.Dir },
                { "ssi", ld.SSI },
                { "timestamp", ld.Timestamp }  // movido p/ condicional abaixo Timestamp = DateTime.UtcNow -> coloca o atual 
            },
            
        };
        return pointToWrite;
    }
}