using Microsoft.ML.Data;
using System.Text.Json.Serialization;

namespace ApiTareasInteligente.Models
{

    public class SentimientoRequest
    {
        [JsonPropertyName("comentario")]
        public string Comentario { get; set; } = string.Empty;
    }


    public class SentimientoData
    {
        public string Comentario { get; set; } = string.Empty;
        
        [ColumnName("Label")] 
        public bool EsPositivo { get; set; } 
    }


    public class SentimientoPrediction
    {
        [ColumnName("PredictedLabel")] 
        public bool Prediction { get; set; }
    }
}