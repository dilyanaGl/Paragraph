﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ML.Runtime.Api;
using Microsoft.ML.Runtime.Data;

namespace Paragraph.Services.MachineLearning
{
    internal class ArticleModelPrediction
    {
        [ColumnName(DefaultColumnNames.PredictedLabel)]
        public string Category { get; set; }
    }
}
