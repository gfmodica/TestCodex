using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace AccessibilityChecker.Services
{
    public record AccessibilityResult(
        bool HasTitle,
        bool HasLang,
        int ImagesWithoutAlt,
        int InputsWithoutLabel
    );

    public class AccessibilityAnalyzer
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<AccessibilityResult> AnalyzeAsync(string url)
        {
            var html = await _httpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            bool hasTitle = doc.DocumentNode.SelectSingleNode("//title") != null;
            bool hasLang = doc.DocumentNode.SelectSingleNode("//html[@lang]") != null;

            int imagesWithoutAlt = doc.DocumentNode
                .SelectNodes("//img")?
                .Count(img => img.GetAttributeValue("alt", string.Empty).Trim() == string.Empty) ?? 0;

            int inputsWithoutLabel = doc.DocumentNode
                .SelectNodes("//input[not(@type='hidden')]")?
                .Count(input => {
                    var id = input.GetAttributeValue("id", string.Empty);
                    if (!string.IsNullOrEmpty(id))
                    {
                        var label = doc.DocumentNode.SelectSingleNode($"//label[@for='{id}']");
                        if (label != null) return false;
                    }
                    // check ancestor label
                    if (input.Ancestors("label").Any()) return false;
                    return true;
                }) ?? 0;

            return new AccessibilityResult(hasTitle, hasLang, imagesWithoutAlt, inputsWithoutLabel);
        }
    }
}
