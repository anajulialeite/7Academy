using EventManager.Web.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace EventManager.Web.Services
{
    /// <summary>Gera o PDF do material de estudo de um curso, no padrão visual da 7Academy.</summary>
    public class StudyMaterialService
    {
        private const string Purple = "#240B36";
        private const string Gold = "#D4AF37";

        public bool HasMaterial(Event ev) => StudyContent.ForEvent(ev.Title) != null;

        public byte[]? Generate(Event ev)
        {
            var topic = StudyContent.ForEvent(ev.Title);
            if (topic == null) return null;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(40);
                    page.DefaultTextStyle(x => x.FontFamily("Lato").FontSize(11).LineHeight(1.35f));

                    page.Header().Column(h =>
                    {
                        h.Item().Text("7ACADEMY  |  MATERIAL DE ESTUDO")
                            .FontFamily("Montserrat").FontSize(9).Bold().FontColor(Gold).LetterSpacing(0.08f);
                        h.Item().PaddingTop(4).LineHorizontal(1.5f).LineColor(Gold);
                    });

                    page.Content().PaddingVertical(12).Column(col =>
                    {
                        col.Spacing(10);

                        // Capa / título
                        col.Item().Background(Purple).Padding(18).Column(c =>
                        {
                            c.Item().Text(ev.Title).FontFamily("Montserrat").FontSize(20).Bold().FontColor(Gold);
                            c.Item().PaddingTop(4).Text(topic.Subtitle).FontSize(12).FontColor(Colors.White);
                            c.Item().PaddingTop(8).Text($"Carga horária: {ev.WorkloadHours} horas  •  Nota mínima na prova: 7,0")
                                .FontSize(10).FontColor(Colors.Grey.Lighten2);
                        });

                        col.Item().Text(topic.Intro);

                        foreach (var section in topic.Sections)
                        {
                            col.Item().PaddingTop(6).Text(section.Title)
                                .FontFamily("Montserrat").FontSize(14).Bold().FontColor(Purple);
                            col.Item().LineHorizontal(0.8f).LineColor(Gold);

                            foreach (var paragraph in section.Text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries))
                            {
                                col.Item().Text(paragraph.Trim()).Justify();
                            }

                            if (!string.IsNullOrWhiteSpace(section.Code))
                            {
                                col.Item().Background("#F4EFF8").BorderLeft(3).BorderColor(Gold)
                                    .Padding(10).Text(section.Code.TrimEnd()).FontSize(9).FontColor("#2B1245");
                            }
                        }

                        // Resumo
                        col.Item().PaddingTop(10).Background("#FBF6E4").Border(1).BorderColor(Gold).Padding(14).Column(c =>
                        {
                            c.Item().Text("Resumo para a prova").FontFamily("Montserrat").FontSize(13).Bold().FontColor(Purple);
                            c.Spacing(4);
                            foreach (var point in topic.KeyPoints)
                                c.Item().Text($"•  {point}");
                        });

                        col.Item().PaddingTop(6).Text("Dica: leia o material com calma, refaça os exemplos de código e, quando se sentir seguro(a), faça a prova. Boa sorte!")
                            .Italic().FontColor(Colors.Grey.Darken1);
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Grey.Darken1));
                        t.Span("7Academy - Instituto de Educação Superior  •  Página ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}
