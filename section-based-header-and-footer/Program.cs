using Syncfusion.Drawing;
using Syncfusion.Licensing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;

// Replace "YOUR_LICENSE_KEY" with the key obtained from Syncfusion
SyncfusionLicenseProvider.RegisterLicense("YOUR_LICENSE_KEY");

// Create a new PDF document and define page settings
using (PdfDocument document = new PdfDocument())
{
    //Create section for cover page
    PdfSection coverPageSection = document.Sections.Add();

    //Add page and insert the cover page image
    PdfPage coverPage = coverPageSection.Pages.Add();

    //Draw the image
    PdfBitmap coverPageImage = new PdfBitmap(new FileStream("../../../../data/cover-page.jpg", FileMode.Open));

    coverPage.Graphics.DrawImage(coverPageImage, new RectangleF(0, 0, coverPage.GetClientSize().Width, coverPage.GetClientSize().Height));

    //Create another section for author page and add header
    PdfSection authorSection = document.Sections.Add();

    //Add author page
    PdfPage authorPage = authorSection.Pages.Add();

    authorSection.Template.Top = CreateHeaderTemplate(new SizeF(authorPage.GetClientSize().Width, 50), true);

    //Add author page content
    authorPage.Graphics.DrawString("PDF Succinctly", new PdfStandardFont(PdfFontFamily.TimesRoman, 30), PdfBrushes.Black, new RectangleF(0, 60, authorPage.GetClientSize().Width, authorPage.GetClientSize().Height), new PdfStringFormat(PdfTextAlignment.Center));

    authorPage.Graphics.DrawLine(PdfPens.Black, new PointF(40, 110), new PointF(435, 110));

    authorPage.Graphics.DrawString("By\r\nRyan Hodson", new PdfStandardFont(PdfFontFamily.TimesRoman, 16, PdfFontStyle.Bold), PdfBrushes.Black, new RectangleF(0, 130, authorPage.GetClientSize().Width, authorPage.GetClientSize().Height), new PdfStringFormat(PdfTextAlignment.Center));

    authorPage.Graphics.DrawString("Foreword by Daniel Jebaraj", new PdfStandardFont(PdfFontFamily.TimesRoman, 20), PdfBrushes.Black, new RectangleF(0, 220, authorPage.GetClientSize().Width, authorPage.GetClientSize().Height), new PdfStringFormat(PdfTextAlignment.Center));


    //Create content section
    PdfSection mainContentSection = document.Sections.Add();

    //Add the main content page
    PdfPage mainContentPage = mainContentSection.Pages.Add();

    //Add header and footer for the main content section
    mainContentSection.Template.Top = CreateHeaderTemplate(new SizeF(mainContentPage.GetClientSize().Width, 50), false);

    mainContentSection.Template.Bottom = CreateFooterTemplate(new SizeF(mainContentPage.GetClientSize().Width, 50));

    //Create font to draw main content
    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);

    //Read the text from the text file
    string text = System.IO.File.ReadAllText("../../../../data/input.txt");

    PdfTextElement element = new PdfTextElement(text, font);

    element.Draw(mainContentPage, new PointF(0, 10));

    //Save the document
    document.Save("SectionBasedHeaderAndFooter.pdf");

    document.Close(true);
}

//Create header template and return
PdfPageTemplateElement CreateHeaderTemplate(SizeF headerSize, bool leftSideHeader)
{
    //Create PdfPageTemplateElement
    PdfPageTemplateElement header = new PdfPageTemplateElement(new RectangleF(0, 0, headerSize.Width, headerSize.Height));
    string headerText = "PDF Succinctly";
    //font
    PdfStandardFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Regular);

    //Measure the text size
    SizeF textSize = font.MeasureString(headerText);

    //Draw text on different location for odd and even pages
    PointF location = leftSideHeader ? new PointF(0, (headerSize.Height - font.Height) / 2) : new PointF(headerSize.Width - textSize.Width, (headerSize.Height - font.Height) / 2);

    //Draw the text with center alignment
    header.Graphics.DrawString(headerText, font, PdfBrushes.Gray, location);

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

    string footerText = "Copyright © 2001 - 2025 Syncfusion Inc. All Rights Reserved.";
   
    //Draw text on different location for odd and even pages
    PointF location = new PointF(0, (footerSize.Height - font.Height) / 2);

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
    PointF pageNumberLocation = new PointF(450, (footerSize.Height - font.Height) / 2);

    //Draw the composite field in footer.
    compositeField.Draw(footer.Graphics, pageNumberLocation);

    //Draw line to separate footer
    footer.Graphics.DrawLine(new PdfPen(PdfBrushes.LightGray, 0.5f), new PointF(0, 0), new PointF(footerSize.Width, 0));
    return footer;
}
