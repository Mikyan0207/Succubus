using System.Threading.Tasks;
using GoogleTranslateFreeApi;
using Succubus.Services.Interfaces;

namespace Succubus.Services
{
    public class TranslationService : IService
    {
        private static readonly GoogleTranslator Translator = new GoogleTranslator();

        public async Task<TranslationResult> TranslateAsync(string text, string to)
        {
            var result = await Translator
                .TranslateLiteAsync(text, Language.Auto, GoogleTranslator.GetLanguageByName(to))
                .ConfigureAwait(false);
            
            return result;
        }
    }
}