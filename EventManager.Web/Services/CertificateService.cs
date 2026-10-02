using EventManager.Web.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;

namespace EventManager.Web.Services
{
    public class CertificateService
    {
        public byte[] GenerateCertificate(EventRegistration registration)
        {
            var studentName = string.IsNullOrWhiteSpace(registration.Participant?.FullName) ? (registration.Participant?.Email ?? "Aluno") : registration.Participant.FullName;
            var eventTitle = registration.Event?.Title ?? "Evento";
            var workload = registration.Event?.WorkloadHours ?? 0;
            var date = registration.Event?.DateTime.ToString("dd 'de' MMMM 'de' yyyy") ?? DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
            var grade = registration.Rating ?? 10;
            
            var percentage = grade * 10;
            var title = grade == 10 ? "CERTIFICADO PURPLE MASTER" : "CERTIFICADO OFICIAL";
            var statusLevel = grade == 10 ? "NÍVEL MÁXIMO - PESQUISADORA CERTIFICADA" : "PESQUISADORA CERTIFICADA";

            var sealSvg = @"<svg width=""100"" height=""100"" viewBox=""0 0 120 120"" xmlns=""http://www.w3.org/2000/svg"">
                <circle cx=""60"" cy=""60"" r=""55"" fill=""none"" stroke=""#D4AF37"" stroke-width=""3"" stroke-dasharray=""4 4""/>
                <circle cx=""60"" cy=""60"" r=""48"" fill=""none"" stroke=""#D4AF37"" stroke-width=""1""/>
                <circle cx=""60"" cy=""60"" r=""44"" fill=""#D4AF37"" opacity=""0.1""/>
                <path d=""M 25 60 A 35 35 0 0 1 95 60 A 35 35 0 0 1 25 60"" fill=""none"" stroke=""#D4AF37"" stroke-width=""1""/>
                <text x=""60"" y=""40"" font-family=""Arial"" font-size=""10"" fill=""#D4AF37"" text-anchor=""middle"" letter-spacing=""2"">ACADEMIA</text>
                <text x=""60"" y=""70"" font-family=""Arial"" font-size=""32"" font-weight=""bold"" fill=""#D4AF37"" text-anchor=""middle"">7A</text>
                <text x=""60"" y=""95"" font-family=""Arial"" font-size=""8"" fill=""#D4AF37"" text-anchor=""middle"" letter-spacing=""4"">EXCELÊNCIA</text>
                <polygon points=""60,18 63,25 70,25 65,30 67,37 60,33 53,37 55,30 50,25 57,25"" fill=""#D4AF37""/>
            </svg>";

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(20);
                    page.PageColor("#240B36"); // Dark purple background
                    page.DefaultTextStyle(x => x.FontFamily("Lato").FontColor("#D4AF37"));

                    // ScaleToFit guarantees it stays on 1 page!
                    page.Content().Border(3).BorderColor("#D4AF37").Padding(20).ScaleToFit().Column(column =>
                    {
                        column.Spacing(5);

                        // Header
                        column.Item().AlignCenter().Text("7ACADEMY - INSTITUTO DE EDUCAÇÃO SUPERIOR").FontFamily("Montserrat").FontSize(18).Bold().LetterSpacing(0.05f);

                        column.Item().PaddingTop(10).PaddingBottom(10).AlignCenter()
                            .Text(title).FontFamily("Montserrat").FontSize(28).Bold();

                        // Body
                        column.Item().AlignCenter().Text("O Conselho Acadêmico da 7Academy certifica que")
                            .FontSize(14).FontColor(Colors.White);

                        // Student Name
                        column.Item().AlignCenter().Text(studentName)
                            .FontFamily("Dancing Script").FontSize(54).FontColor(Colors.White);

                        // Event Info
                        column.Item().AlignCenter().Text("concluiu com aproveitamento o evento:")
                            .FontSize(14).FontColor(Colors.White);
                            
                        column.Item().AlignCenter().Text(eventTitle)
                            .FontFamily("Montserrat").FontSize(24).Bold().FontColor("#D4AF37");
                            
                        column.Item().AlignCenter().Text($"com carga horária total de {workload} horas.")
                            .FontSize(14).FontColor(Colors.White);

                        // Grade & Status Row
                        column.Item().PaddingTop(15).AlignCenter().Column(c =>
                        {
                            c.Item().AlignCenter().Text($"{grade} / 10  -  {percentage}%").FontFamily("Montserrat").FontSize(20).Bold().FontColor("#D4AF37");
                            c.Item().AlignCenter().Text(statusLevel).FontSize(12).LetterSpacing(0.05f).FontColor(Colors.White);
                        });

                        // Footer / Signature (Aligned)
                        column.Item().PaddingTop(25).AlignBottom().Row(row =>
                        {
                            row.RelativeItem().AlignCenter().AlignBottom().Column(c =>
                            {
                                c.Item().AlignCenter().Text("Ana Júlia Leite").FontFamily("Dancing Script").FontSize(44).FontColor(Colors.White);
                                c.Item().LineHorizontal(1).LineColor("#D4AF37");
                                c.Item().AlignCenter().Text("Diretora Acadêmica - 7Academy").FontSize(11).FontColor(Colors.White);
                            });

                            row.RelativeItem().AlignCenter().AlignBottom().Svg(sealSvg);

                            row.RelativeItem().AlignCenter().AlignBottom().Column(c =>
                            {
                                c.Item().AlignCenter().PaddingBottom(5).Text(date).FontSize(14).FontColor(Colors.White);
                                c.Item().LineHorizontal(1).LineColor("#D4AF37");
                                c.Item().AlignCenter().Text("Data de Emissão").FontSize(11).FontColor(Colors.White);
                            });
                        });
                    });
                });
            });

            return document.GeneratePdf();
        }
    }
}