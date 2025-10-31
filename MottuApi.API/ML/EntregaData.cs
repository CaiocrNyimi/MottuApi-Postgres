using Microsoft.ML.Data;

namespace MottuApi.ML
{
    public class EntregaData
    {
        [LoadColumn(0)] public string Modelo { get; set; } = string.Empty;
        [LoadColumn(1)] public float DistanciaKm { get; set; }
        [LoadColumn(2)] public float TempoEntregaMin { get; set; }
    }

    public class EntregaPrediction
    {
        [ColumnName("Score")]
        public float TempoEstimadoMin { get; set; }
    }
}