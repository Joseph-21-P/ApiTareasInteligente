using ApiTareasInteligente.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace ApiTareasInteligente.Controllers
{
    [Route("api/ml")]
    [ApiController]
    public class MlController : ControllerBase
    {
        [HttpPost("sentimiento")]
        public IActionResult AnalizarSentimiento([FromBody] SentimientoRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Comentario))
            {
                return BadRequest("El comentario no puede estar vacío.");
            }

            var mlContext = new MLContext();

            var datosEntrenamiento = new List<SentimientoData>
            {
                new() { Comentario = "La tarea fue completada correctamente y el sistema funciona bien", EsPositivo = true },
                new() { Comentario = "Excelente trabajo, todo muy rápido y limpio", EsPositivo = true },
                new() { Comentario = "Me encantó el resultado final, muy útil", EsPositivo = true },
                new() { Comentario = "La entrega fue perfecta y a tiempo", EsPositivo = true },
                
                new() { Comentario = "El sistema tiene muchos errores y no funciona", EsPositivo = false },
                new() { Comentario = "Pésimo resultado muy deficiente, estoy decepcionado", EsPositivo = false },
                new() { Comentario = "No me gusta como quedó la tarea", EsPositivo = false },
                new() { Comentario = "Es un desastre, la aplicación se cierra sola", EsPositivo = false }
            };

            var trainingDataView = mlContext.Data.LoadFromEnumerable(datosEntrenamiento);

            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimientoData.Comentario))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

            var model = pipeline.Fit(trainingDataView);

            var predictionEngine = mlContext.Model.CreatePredictionEngine<SentimientoData, SentimientoPrediction>(model);

            var input = new SentimientoData { Comentario = request.Comentario };
            var result = predictionEngine.Predict(input);

            return Ok(new
            {
                comentario = request.Comentario,
                sentimiento = result.Prediction ? "Positivo" : "Negativo"
            });
        }
    }
}