using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace AccessibilityChecker.Services
{
    public class AccessibilityDeclarationGenerator
    {
        public byte[] Generate(string url, AccessibilityResult result)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Content().Column(col =>
                    {
                        col.Item().Text("Dichiarazione di Accessibilità").FontSize(20).Bold();
                        col.Item().Text($"URL analizzata: {url}");
                        col.Item().Text($"Titolo presente: {result.HasTitle}");
                        col.Item().Text($"Attributo lang presente: {result.HasLang}");
                        col.Item().Text($"Immagini senza alt: {result.ImagesWithoutAlt}");
                        col.Item().Text($"Campi input senza label: {result.InputsWithoutLabel}");
                    });
                });
            });

            using var stream = new MemoryStream();
            document.GeneratePdf(stream);
            return stream.ToArray();
        }
    }
}
