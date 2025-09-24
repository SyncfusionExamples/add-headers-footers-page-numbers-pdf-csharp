using Syncfusion.Drawing;
using Syncfusion.Licensing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Parsing;

// Replace "YOUR_LICENSE_KEY" with the key obtained from Syncfusion
SyncfusionLicenseProvider.RegisterLicense("YOUR_LICENSE_KEY");

using (PdfLoadedDocument loadedDocument = new PdfLoadedDocument("../../../../data/input.pdf"))
{
    //Create a new PDF document to add header and footer.
    PdfDocument document = new PdfDocument();

    //Create header and footer template and assign to the new document.
    document.Template.Top = CreateHeaderTemplate(new SizeF(loadedDocument.Pages[0].Size.Width, 50));
    document.Template.Bottom = CreateFooterTemplate(new SizeF(loadedDocument.Pages[0].Size.Width, 50));

    //Iterate the pages of the loaded document
    foreach (PdfPageBase page in loadedDocument.Pages)
    {
        //Add new section in the new document
        PdfSection section = document.Sections.Add();
        section.PageSettings.Margins.All = 0;
        //Set page size '100 is reserved for header and footer'
        section.PageSettings.Size = new SizeF(page.Size.Width, page.Size.Height + 100);

        //Add a page in the section
        PdfPage pdfPage = section.Pages.Add();
        //Draw the loaded page to the new page.
        pdfPage.Graphics.DrawPdfTemplate(page.CreateTemplate(), new PointF(0, 0));
    }
    //Save the document
    document.Save("HeaderAndFooterInExistingPDF.pdf");
    //Close the document
    document.Close(true);
    loadedDocument.Close(true);
}

PdfPageTemplateElement CreateHeaderTemplate(SizeF headerSize)
{
    //Create PdfPageTemplateElement
    PdfPageTemplateElement header = new PdfPageTemplateElement(new RectangleF(0, 0, headerSize.Width, headerSize.Height));
    string headerText = "Lorem Ipsum";
    //font
    PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Regular);

    //Measure the text size
    SizeF textSize = font.MeasureString(headerText);

    //Draw text on different location for odd and even pages
    PointF location = new PointF((headerSize.Width - textSize.Width)/2, (headerSize.Height - font.Height) / 2);

    //Draw the text with center alignment
    header.Graphics.DrawString(headerText, font, PdfBrushes.Black, location);

    //Draw line to separate header 
    header.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(0, headerSize.Height - 1), new PointF(headerSize.Width, headerSize.Height - 1));

    return header;
}

//Create footer template and return
PdfPageTemplateElement CreateFooterTemplate(SizeF footerSize)
{
    //Create the footer template
    PdfPageTemplateElement footer = new PdfPageTemplateElement(new RectangleF(0, 0, footerSize.Width, footerSize.Height));
    //Create font to draw the text
    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

    // Create the page number field
    PdfPageNumberField pageNumber = new PdfPageNumberField(font, PdfBrushes.Black);

    // Create the page count field
    PdfPageCountField count = new PdfPageCountField(font, PdfBrushes.Black);

    // Create a composite field to combine page number and count
    PdfCompositeField compositeField = new PdfCompositeField(font, PdfBrushes.Black, "Page {0} of {1}", pageNumber, count);

    compositeField.Bounds = footer.Bounds;

    //Draw the page number based on odd or even pages
    PointF pageNumberLocation = new PointF(450, (footerSize.Height - font.Height) / 2);

    //Draw the composite field in footer.
    compositeField.Draw(footer.Graphics, pageNumberLocation);

    //Draw line to separate footer
    footer.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(0, 0), new PointF(footerSize.Width, 0));
    return footer;
}