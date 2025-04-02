using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLMProject.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PLMProject.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMaterialService _materialService;

        private readonly HttpClient _httpClient;

        public ReportsController(IProductService service, IMaterialService materialService, HttpClient httpClient)
        {
            _productService = service;
            _materialService = materialService;
            _httpClient = httpClient;
        }

        [HttpGet("generate-product-report")]
        public async Task<IActionResult> GenerateProductStatisticsPdfAsync()
        {
            // Ensure productStatistics is an IEnumerable or List of strongly typed objects
            List<ProjectStatistics> productStatistics = await _productService.GetDetailedProductStatisticsAsync();

            // Define the path to save the PDF
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ProductLifecycleReport.pdf");

            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                    // Title of the report
                    document.Add(new Paragraph("Product Lifecycle Report")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(18)
                        .SetBold());

                    // Add some space
                    document.Add(new Paragraph("\n"));

                    // Ensure productStatistics is properly typed
                    foreach (var product in productStatistics)
                    {
                        // Ensure product is correctly typed, e.g., ProductStatistics
                        document.Add(new Paragraph($"Product: {product.ProductName}")
                            .SetFontSize(14)
                            .SetBold());

                        // Add other information as needed
                        document.Add(new Paragraph($"Description: {product.ProductDescription}"));
                        document.Add(new Paragraph($"Estimated Height: {product.EstimatedHeight}"));
                        document.Add(new Paragraph($"Estimated Weight: {product.EstimatedWeight}"));
                        document.Add(new Paragraph($"Estimated Width: {product.EstimatedWidth}"));

                        // BOM details
                        document.Add(new Paragraph($"BOM: {product.BOMName}"));
                        document.Add(new Paragraph("Materials in BOM:"));

                        // Create a table for BOM materials with specified column widths
                        float[] bomColumnWidths = { 1f, 3f, 1f, 2f }; // Adjust the ratios as needed
                        var bomTable = new Table(bomColumnWidths)
                            .AddHeaderCell("Material Number")
                            .AddHeaderCell("Material Description")
                            .AddHeaderCell("Quantity")
                            .AddHeaderCell("Unit Measure Code");

                        foreach (var material in product.BOMMaterials)
                        {
                            bomTable.AddCell(material.MaterialNumber).SetTextAlignment(TextAlignment.CENTER)
                                     .AddCell(material.MaterialDescription).SetTextAlignment(TextAlignment.CENTER)
                                     .AddCell(material.Qty.ToString()).SetTextAlignment(TextAlignment.CENTER)
                                     .AddCell(material.UnitMeasureCode).SetTextAlignment(TextAlignment.CENTER);
                        }

                        document.Add(bomTable);

                        // Stage history
                        document.Add(new Paragraph("Stage History:"));
                        float[] stageColumnWidths = { 2f, 1f, 1f, 1f }; // Adjust the ratios as needed
                        var stageTable = new Table(stageColumnWidths)
                            .AddHeaderCell("Stage Name")
                            .AddHeaderCell("Start Date")
                            .AddHeaderCell("User Name")
                            .AddHeaderCell("Duration (Days)");

                        foreach (var stage in product.StageHistory)
                        {
                            stageTable.AddCell(stage.StageName).SetTextAlignment(TextAlignment.CENTER)
                                      .AddCell(stage.StartOfStage.ToShortDateString()).SetTextAlignment(TextAlignment.CENTER)
                                      .AddCell(stage.UserName).SetTextAlignment(TextAlignment.CENTER)
                                      .AddCell(stage.Duration.ToString()).SetTextAlignment(TextAlignment.CENTER);
                        }

                        document.Add(stageTable);

                        // Current stage info
                        document.Add(new Paragraph($"Current Stage: {product.CurrentStage}"));
                        document.Add(new Paragraph($"Total Material Quantity: {product.TotalMaterialQuantity}"));

                        // Add space between products
                        document.Add(new Paragraph("\n"));
                        document.Add(new LineSeparator(new SolidLine()));
                        document.Add(new Paragraph("\n"));
                    }

                    // Close the document
                    document.Close();
                }
            }

            // Return the file as a downloadable response
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", "ProductLifecycleReport.pdf");
        }

        [HttpGet("generate-product-report/{projectId}")]
        public async Task<IActionResult> GenerateProjectReportPdfAsync(int projectId)
        {
            // Fetch the project data using the projectId
            var productStatistics = await _productService.GetDetailedProductStatisticsByProjectIdAsync(projectId);

            if (productStatistics == null)
            {
                return NotFound("Project not found.");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "ProductLifecycleReport.pdf");

            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                    // Title of the report
                    document.Add(new Paragraph("Product Lifecycle Report")
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(18)
                        .SetBold());

                    // Add some space
                    document.Add(new Paragraph("\n"));

                    // Product information
                    document.Add(new Paragraph($"Product: {productStatistics.ProductName}")
                        .SetFontSize(14)
                        .SetBold());
                    document.Add(new Paragraph($"Description: {productStatistics.ProductDescription}"));
                    document.Add(new Paragraph($"Estimated Height: {productStatistics.EstimatedHeight}"));
                    document.Add(new Paragraph($"Estimated Weight: {productStatistics.EstimatedWeight}"));
                    document.Add(new Paragraph($"Estimated Width: {productStatistics.EstimatedWidth}"));

                    // BOM details
                    document.Add(new Paragraph($"BOM: {productStatistics.BOMName}"));
                    document.Add(new Paragraph("Materials in BOM:"));

                    // Create a table for BOM materials with specified column widths
                    float[] bomColumnWidths = { 1f, 3f, 1f, 2f }; // Adjust the ratios as needed
                    var bomTable = new Table(bomColumnWidths)
                        .AddHeaderCell("Material Number")
                        .AddHeaderCell("Material Description")
                        .AddHeaderCell("Quantity")
                        .AddHeaderCell("Unit Measure Code");

                    foreach (var material in productStatistics.BOMMaterials)
                    {
                        // Center the cell content
                        bomTable.AddCell(new Cell().Add(new Paragraph(material.MaterialNumber)).SetTextAlignment(TextAlignment.CENTER))
                                 .AddCell(new Cell().Add(new Paragraph(material.MaterialDescription)).SetTextAlignment(TextAlignment.CENTER))
                                 .AddCell(new Cell().Add(new Paragraph(material.Qty.ToString())).SetTextAlignment(TextAlignment.CENTER))
                                 .AddCell(new Cell().Add(new Paragraph(material.UnitMeasureCode)).SetTextAlignment(TextAlignment.CENTER));
                    }

                    document.Add(bomTable);

                    // Stage history
                    document.Add(new Paragraph("Stage History:"));
                    float[] stageColumnWidths = { 2f, 1f, 1f, 1f }; // Adjust the ratios as needed
                    var stageTable = new Table(stageColumnWidths)
                        .AddHeaderCell("Stage Name")
                        .AddHeaderCell("Start Date")
                        .AddHeaderCell("User Name")
                        .AddHeaderCell("Duration (Days)");

                    foreach (var stage in productStatistics.StageHistory)
                    {
                        // Center the stage history cells
                        stageTable.AddCell(new Cell().Add(new Paragraph(stage.StageName)).SetTextAlignment(TextAlignment.CENTER))
                                  .AddCell(new Cell().Add(new Paragraph(stage.StartOfStage.ToShortDateString())).SetTextAlignment(TextAlignment.CENTER))
                                  .AddCell(new Cell().Add(new Paragraph(stage.UserName)).SetTextAlignment(TextAlignment.CENTER))
                                  .AddCell(new Cell().Add(new Paragraph(stage.Duration.ToString())).SetTextAlignment(TextAlignment.CENTER));
                    }

                    document.Add(stageTable);

                    // Current stage info
                    document.Add(new Paragraph($"Current Stage: {productStatistics.CurrentStage}"));
                    document.Add(new Paragraph($"Total Material Quantity: {productStatistics.TotalMaterialQuantity}"));

                    // Add space between products
                    document.Add(new Paragraph("\n"));
                    document.Add(new LineSeparator(new SolidLine()));
                    document.Add(new Paragraph("\n"));
                }
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf", "ProductLifecycleReport.pdf");
        }





    }
}
