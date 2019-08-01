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
            this.payload.Add("country_translation", Translations);
        }

        private IEnumerable<CurrentLanguageTranslations> Translations
        {
            get
            {
                return Directory.GetFiles(Constants.TranslationsFileDirectoryPath, Constants.CurrentTranslationsFileNamePattern)
                       .Select(m => File.ReadAllText(m))
                       .Select(m => JsonConvert.DeserializeObject<CurrentLanguageTranslations>(m))
                       .ToList();
            }
        }
    }
}