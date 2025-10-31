using Microsoft.ML;
using Microsoft.ML.Transforms;
using MottuApi.Dtos;
using MottuApi.ML;
using MottuApi.Services.Interfaces;

namespace MottuApi.Services.Implementations
{
    public class EntregaMlService : IEntregaMlService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;

        public EntregaMlService()
        {
            _mlContext = new MLContext();

            var trainingData = _mlContext.Data.LoadFromEnumerable(EntregaSamples.Dados);

            var pipeline = _mlContext.Transforms
                .Categorical.OneHotEncoding("Modelo", outputKind: OneHotEncodingEstimator.OutputKind.Bag)
                .Append(_mlContext.Transforms.Concatenate("Features", "Modelo", "DistanciaKm"))
                .Append(_mlContext.Regression.Trainers.FastTree(labelColumnName: "TempoEntregaMin"));

            _model = pipeline.Fit(trainingData);
        }

        public float PreverTempoEntrega(EntregaRequestDto request)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<EntregaData, EntregaPrediction>(_model);

            var entrega = new EntregaData
            {
                Modelo = request.Modelo,
                DistanciaKm = request.DistanciaKm
            };

            var prediction = predictionEngine.Predict(entrega);
            return prediction.TempoEstimadoMin;
        }

        public EntregaResponseDto PreverTempoEntregaDto(EntregaRequestDto request)
        {
            var tempo = PreverTempoEntrega(request);
            return new EntregaResponseDto
            {
                TempoEstimadoMin = (float)Math.Round(tempo, 2)
            };
        }
    }
}