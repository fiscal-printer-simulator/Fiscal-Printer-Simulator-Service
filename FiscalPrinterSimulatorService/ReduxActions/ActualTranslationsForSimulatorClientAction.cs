using FiscalPrinterSimulatorService.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FiscalPrinterSimulatorService.ReduxActions
{
    public class ActualTranslationsForSimulatorClientAction : BaseReduxAction
    {
        public ActualTranslationsForSimulatorClientAction()
        {
            this.type = ReduxActionType.ACTUAL_TRANSLATION_FOR_SIMULATOR_CLIENT.ToString();

            var mergedTranslationsFilePath = Path.Combine(Constants.TranslationsFileDirectoryPath, Constants.MergedTranslationsFileName);
            IEnumerable<CurrentLanguageTranslations> translations = new List<CurrentLanguageTranslations>();
            if (File.Exists(mergedTranslationsFilePath))
            {

                var translationFiles = Directory.GetFiles(Constants.TranslationsFileDirectoryPath, Constants.CurrentTranslationsFileNamePattern);
                if (translationFiles.Any())
                {
                    translations = translationFiles
                        .Select(m => File.ReadAllText(m))
                        .Select(m => JsonConvert.DeserializeObject<CurrentLanguageTranslations>(m))
                        .ToList();
                }
                this.payload.Add("country_translation", translations);

            }
        }
    }
}