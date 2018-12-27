using System;
using Microsoft.ML;
using Microsoft.ML.Runtime.Data;
using Microsoft.ML.Core.Data;
using System.IO;

namespace Paragraph.Services.MachineLearning
{
    public class ArticleCategorizer : IArticleCategorizer
    {
            public string Categorize(string modelFile, string articleContent)
            {

                var mlContext = new MLContext(seed: 0);

                ITransformer trainedModel;

                using (var stream = new FileStream(modelFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                {

                    trainedModel = mlContext.Model.Load(stream);

                }

                var predFunction = trainedModel.MakePredictionFunction<ArticleModel, ArticleModelPrediction>(mlContext);

                var prediction = predFunction.Predict(new ArticleModel { Content = articleContent });

                return prediction.Category;            
        }
    }
}
