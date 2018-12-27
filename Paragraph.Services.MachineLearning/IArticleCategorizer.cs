using System;
using System.Collections.Generic;
using System.Text;

namespace Paragraph.Services.MachineLearning
{
    public interface IArticleCategorizer
    {
        string Categorize(string modelFile, string articleContent);
    }
}
