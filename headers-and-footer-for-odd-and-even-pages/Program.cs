using Syncfusion.Drawing;
using Syncfusion.Licensing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

// Replace "YOUR_LICENSE_KEY" with the key obtained from Syncfusion
SyncfusionLicenseProvider.RegisterLicense("YOUR_LICENSE_KEY");

// Create a new PDF document and define page settings
using (PdfDocument document = new PdfDocument())
{
    //Set the page margins
    document.PageSettings.Margins.Top = 0;
    document.PageSettings.Margins.Bottom = 0;

    //Add page to the document
    PdfPage page = document.Pages.Add();

    //Get the page size
    SizeF pageSize = page.GetClientSize();

    //Create the header template and add to the document
    document.Template.OddTop = CreateHeaderTemplate(new SizeF(pageSize.Width, 50), true);

    //Create the header template for even pages and add to the document
    document.Template.EvenTop = CreateHeaderTemplate(new SizeF(pageSize.Width, 50), false);

    //Create the footer template and add to the document
    document.Template.OddBottom = CreateFooterTemplate(new SizeF(pageSize.Width, 50), true);

    //Create the footer template for even pages and add to the document
    document.Template.EvenBottom = CreateFooterTemplate(new SizeF(pageSize.Width, 50), false);

    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

    //Read the text from the text file
    string text = System.IO.File.ReadAllText("../../../../data/input.txt");

    PdfTextElement element = new PdfTextElement(text, font);

    element.Draw(page, new PointF(0, 0));

    //Save the document
    document.Save("HeaderFooterForOddAndEvenPages.pdf");

    document.Close(true);
}

//Create header template and return
PdfPageTemplateElement CreateHeaderTemplate(SizeF headerSize, bool isOddHeader)
{
    //Create PdfPageTemplateElement
    PdfPageTemplateElement header = new PdfPageTemplateElement(new RectangleF(0, 0, headerSize.Width, headerSize.Height));
    string headerText = "PDF Succinctly";
    //font
    PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Regular);

    //Measure the text size
    SizeF textSize = font.MeasureString(headerText);

    //Draw text on different location for odd and even pages
    PointF location = isOddHeader ? new PointF(0, (headerSize.Height - font.Height) / 2) : new PointF(headerSize.Width - textSize.Width, (headerSize.Height - font.Height) / 2);

    //Draw the text with center alignment
    header.Graphics.DrawString("PDF Succinctly", font, PdfBrushes.Gray, location);

    //Draw line to separate header 
    header.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(0, headerSize.Height - 1), new PointF(headerSize.Width, headerSize.Height - 1));

    return header;
}

//Create footer template and return
PdfPageTemplateElement CreateFooterTemplate(SizeF footerSize, bool isOddFooter)
{
    //Create the footer template
    PdfPageTemplateElement footer = new PdfPageTemplateElement(new RectangleF(0, 0, footerSize.Width, footerSize.Height));
    //Create font to draw the text
    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

    string footerText = "Copyright © 2001 - 2025 Syncfusion Inc. All Rights Reserved."; 

    //Measure the text size
    SizeF textSize = font.MeasureString(footerText);

    //Draw text on different location for odd and even pages
    PointF location = isOddFooter ?  new PointF(footerSize.Width - textSize.Width, (footerSize.Height - font.Height) / 2) : new PointF(0, (footerSize.Height - font.Height) / 2);

    //Draw the text
    footer.Graphics.DrawString(footerText, font, PdfBrushes.Gray, location);

    // Create the page number field
    PdfPageNumberField pageNumber = new PdfPageNumberField(font, PdfBrushes.Black);

    // Create the page count field
    PdfPageCountField count = new PdfPageCountField(font, PdfBrushes.Black);

    // Create a composite field to combine page number and count
    PdfCompositeField compositeField = new PdfCompositeField(font, PdfBrushes.Black, "Page {0} of {1}", pageNumber, count);

    compositeField.Bounds = footer.Bounds;

    //Draw the page number based on odd or even pages
   PointF pageNumberLocation = isOddFooter ? new PointF(0, (footerSize.Height - font.Height) / 2) : new PointF(450, (footerSize.Height - font.Height) / 2);

    //Draw the composite field in footer.
    compositeField.Draw(footer.Graphics, pageNumberLocation);

    //Draw line to separate footer
    footer.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(0, 0), new PointF(footerSize.Width, 0));
    return footer;
}
