using DBL.Models;
using Firebase.Storage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Font = iTextSharp.text.Font;

namespace Maqaoplus.ViewModels.PropertyHouseTenantAgreement
{
    public class PropertyHouseTenantAgreementViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        private TenantAgreementDetailData _tenantAgreementDetailData;
        public ICommand ViewPropertyRoomAgreementCommand { get; }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
            }
        }

        private bool _isDataLoaded;
        public bool IsDataLoaded
        {
            get => _isDataLoaded;
            set
            {
                _isDataLoaded = value;
                OnPropertyChanged();
            }
        }


        public TenantAgreementDetailData TenantAgreementDetailData
        {
            get => _tenantAgreementDetailData;
            set
            {
                _tenantAgreementDetailData = value;
                OnPropertyChanged(nameof(TenantAgreementDetailData));
                OnPropertyChanged(nameof(IsSignatureDrawingVisible));
                OnPropertyChanged(nameof(IsSignatureImageVisible));
                OnPropertyChanged(nameof(IsSignatureAvailable));
            }
        }

        public bool IsSignatureDrawingVisible => string.IsNullOrEmpty(TenantAgreementDetailData?.TenantSignatureimageurl);
        public bool IsSignatureImageVisible => !string.IsNullOrEmpty(TenantAgreementDetailData?.TenantSignatureimageurl);
        public bool IsSignatureAvailable
        {
            get
            {
                // Check if TenantAgreementDetailData is not null
                if (TenantAgreementDetailData == null)
                {
                    return false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(TenantAgreementDetailData.TenantSignatureimageurl))
                    {
                        return !string.IsNullOrEmpty(TenantAgreementDetailData.TenantSignatureimageurl);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        // Parameterless constructor for XAML support
        public PropertyHouseTenantAgreementViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ViewPropertyRoomAgreementCommand = new Command(async () => await ViewPropertyRoomAgreementDetails());
        }

        private async Task ViewPropertyRoomAgreementDetails()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomagreementdetaildatabytenantid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    TenantAgreementDetailData = JsonConvert.DeserializeObject<TenantAgreementDetailData>(response.Data.ToString());
                }
                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public async Task AgreeToPropertyHouseRoomAgreementasync(string imageUrl)
        {
            IsProcessing = true;

           
            if (TenantAgreementDetailData == null)
            {
                IsProcessing = false;
                return;
            }
            TenantAgreementDetailData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            TenantAgreementDetailData.Signatureimageurl = imageUrl;
            TenantAgreementDetailData.Ownerortenant = "Tenant";
            TenantAgreementDetailData.Agreementname = TenantAgreementDetailData.Tenantfullname + " " + TenantAgreementDetailData.Propertyhousename + " Room Tenant Agreement";
            TenantAgreementDetailData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", TenantAgreementDetailData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    TenantAgreementDetailData.TenantSignatureimageurl = response.Data2;
                    TenantAgreementDetailData.Agreementdetailpdfurl = await GenerateAndUploadAgreementPdfAsync();
                    TenantAgreementDetailData.Agreementid = Convert.ToInt64(response.Data1);
                    var responseAfter = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", TenantAgreementDetailData);
                    if (responseAfter.RespStatus == 200 || responseAfter.RespStatus == 0)
                    {
                        (Shell.Current.CurrentPage.BindingContext as PropertyHouseTenantAgreementViewModel)?.ViewPropertyRoomAgreementCommand.Execute(null);
                    }
                    else if (responseAfter.RespStatus == 1)
                    {
                        await Shell.Current.DisplayAlert("Warning", responseAfter.RespMessage, "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");

                    }
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }

        public async Task<string> GenerateAndUploadAgreementPdfAsync()
        {
            if (TenantAgreementDetailData == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                // Initialize PDF writer and document
                using (var document = new Document(PageSize.A4, 50, 50, 50, 50))
                {
                    PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20, BaseColor.Black);
                    var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.Black);
                    var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.Black);
                    var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.Black);

                    // Add a title with styling
                    var titleParagraph = new Paragraph("TENANT RENTAL AGREEMENT", titleFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER,
                        SpacingAfter = 20
                    };
                    document.Add(titleParagraph);

                    // Add a separator line
                    var separator = new LineSeparator(1f, 100f, BaseColor.Black, iTextSharp.text.Element.ALIGN_CENTER, -2);
                    document.Add(new Chunk(separator));

                    // Add Agreement Date
                    var dateParagraph = new Paragraph($"Date: {TenantAgreementDetailData.TenantDatecreated:dd MMMM yyyy}", smallFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_RIGHT,
                        SpacingAfter = 10
                    };
                    document.Add(dateParagraph);

                    // Agreement Intro
                    var introParagraph = new Paragraph(
                        $"This Rental Agreement (\"Agreement\") is made and entered into on this {TenantAgreementDetailData.TenantDatecreated:dd} day of {TenantAgreementDetailData.TenantDatecreated:MMMM}, {TenantAgreementDetailData.TenantDatecreated:yyyy}, by and between:",
                        bodyFont)
                    {
                        SpacingAfter = 20
                    };
                    document.Add(introParagraph);

                    // Landlord and Tenant Details
                    AddHeader(document, "Landlord Details", headerFont);
                    AddDetail(document, $"Name: {TenantAgreementDetailData.Ownerfullname}", bodyFont);
                    AddDetail(document, $"Property Name: {TenantAgreementDetailData.Propertyhousename}", bodyFont);
                    AddDetail(document, $"Address: {TenantAgreementDetailData.Countyname}-{TenantAgreementDetailData.Subcountyname}-{TenantAgreementDetailData.Subcountywardname}", bodyFont);
                    AddDetail(document, $"Phone Number: {TenantAgreementDetailData.Ownerphonenumber}", bodyFont);
                    AddDetail(document, $"Email Address: {TenantAgreementDetailData.Owneremailaddress}", bodyFont);

                    AddHeader(document, "Tenant Details", headerFont);
                    AddDetail(document, $"Name: {TenantAgreementDetailData.Tenantfullname}", bodyFont);
                    AddDetail(document, $"ID/Passport Number: {TenantAgreementDetailData.Tenantidnumber}", bodyFont);
                    AddDetail(document, $"Phone Number: {TenantAgreementDetailData.Tenantphonenumber}", bodyFont);
                    AddDetail(document, $"Email Address: {TenantAgreementDetailData.Tenantemailaddress}", bodyFont);

                    // Agreement Sections
                    AddHeader(document, "1. PREMISES", headerFont);
                    AddDetail(document, $"The Landlord hereby agrees to rent to the Tenant, and the Tenant hereby agrees to rent from the Landlord {TenantAgreementDetailData.Propertyhousename}, the residential premises located at: {TenantAgreementDetailData.Countyname}-{TenantAgreementDetailData.Subcountyname}-{TenantAgreementDetailData.Subcountywardname}.", bodyFont);
                    AddDetail(document, $"Land Mark: {TenantAgreementDetailData.Streetorlandmark}", bodyFont);

                    AddHeader(document, "2. TERM", headerFont);
                    AddDetail(document, $"The term of this rental agreement shall commence on {TenantAgreementDetailData.TenantDatecreated:yyyy-MM-dd} (Start Date) and shall continue as follows:", bodyFont);
                    AddDetail(document, TenantAgreementDetailData.Monthlyrentterms ? "Month-to-month tenancy." : "Fixed-term lease.", bodyFont);

                    AddHeader(document, "3. RENT", headerFont);
                    AddDetail(document, $"The Tenant agrees to pay the Landlord a monthly rent of Ksh. {TenantAgreementDetailData.Systempropertyhousesizerent:#,##0.00}, payable in advance on or before the {TenantAgreementDetailData.Rentdueday} day of each month. The first rent payment is due on {TenantAgreementDetailData.Nextrentduedate:yyyy-MM-dd}.", bodyFont);

                    AddHeader(document, "4. SECURITY DEPOSIT", headerFont);
                    AddDetail(document, $"The Tenant agrees to pay a security deposit of Ksh. {TenantAgreementDetailData.Systempropertyhousesizerentdeposit:#,##0.00}, equivalent to {TenantAgreementDetailData.Rentdepositmonth} month's rent, to be held by the Landlord as security for the performance of the Tenant's obligations under this Agreement.", bodyFont);
                    AddDetail(document, $"The security deposit will be refunded to the Tenant within {TenantAgreementDetailData.Rentdepositrefunddays} days after vacating the premises, less any deductions for damages beyond normal wear and tear.", bodyFont);

                    AddHeader(document, "5. UTILITIES", headerFont);
                    AddDetail(document, TenantAgreementDetailData.Rentutilityinclusive ? "All utilities (e.g., electricity, water) are included in the rent." : $"The Tenant shall be responsible for the payment of the following utilities: {TenantAgreementDetailData.Propertyhouseutility}.", bodyFont);

                    AddHeader(document, "6. MAINTENANCE AND REPAIRS", headerFont);
                    AddDetail(document, "The Tenant agrees to keep the premises in a clean and habitable condition and to promptly notify the Landlord of any necessary repairs. The Tenant shall not make any alterations to the premises without the prior written consent of the Landlord.", bodyFont);

                    AddHeader(document, "7. OCCUPANTS", headerFont);
                    AddDetail(document, $"The premises shall be occupied by the Tenant and the following individuals:\n{TenantAgreementDetailData.Tenantsintheroom}", bodyFont);

                    AddHeader(document, "8. PETS", headerFont);
                    AddDetail(document, TenantAgreementDetailData.Allowpets ? "Pets are allowed, subject to the following conditions: [Specify pet conditions here]." : "No pets are allowed on the premises.", bodyFont);

                    AddHeader(document, "9. PAYMENT DETAILS", headerFont);
                    AddDetail(document, $"The Tenant shall make all payments to the following bank account details:\n{TenantAgreementDetailData.Systempropertybankname}", bodyFont);

                    AddHeader(document, "10. INSURANCE", headerFont);
                    AddDetail(document, "The Tenant is encouraged to obtain renter's insurance to cover personal property against loss or damage. The Landlord shall not be responsible for any loss or damage to the Tenant's personal property.", bodyFont);

                    AddHeader(document, "11. DISPUTE RESOLUTION", headerFont);
                    AddDetail(document, "Any disputes arising out of or relating to this Agreement shall be resolved through mediation or arbitration, as per the laws of Kenya.", bodyFont);
                    AddDetail(document, "The parties agree to seek resolution through these methods before pursuing any legal action.", bodyFont);

                    AddHeader(document, "12. GOVERNING LAW", headerFont);
                    AddDetail(document, "This Agreement shall be governed by and construed in accordance with the laws of Kenya.", bodyFont);

                    AddHeader(document, "13. ENTIRE AGREEMENT", headerFont);
                    AddDetail(document, "This Agreement constitutes the entire agreement between the parties and supersedes all prior agreements or understandings, whether written or oral, relating to the subject matter hereof.", bodyFont);

                    AddHeader(document, "14. WEAR AND TEAR", headerFont);
                    AddDetail(document, "Normal wear and tear refers to the deterioration of the premises and its fixtures caused by ordinary use over time, without negligence or abuse.", bodyFont);
                    AddDetail(document, "The Tenant is responsible for repairs or costs associated with damage beyond normal wear and tear.", bodyFont);

                    AddHeader(document, "15. DATA PROTECTION", headerFont);
                    AddDetail(document, "The Landlord and Tenant agree to comply with all applicable data protection laws and regulations.", bodyFont);

                    // Add Signatures
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("AGREED AND ACCEPTED", headerFont)
                    {
                        Alignment = iTextSharp.text.Element.ALIGN_CENTER,
                        SpacingBefore = 20,
                        SpacingAfter = 20
                    });

                    // Create a table with 2 columns
                    var table = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };

                    // Set column widths (adjust as necessary)
                    table.SetWidths(new float[] { 1f, 1f });
                    // Add Property Owner signature
                    AddSignatureToTable(table, "Property Owner", TenantAgreementDetailData.Ownerfullname, TenantAgreementDetailData.OwnerSignatureimageurl, TenantAgreementDetailData.TenantDatecreated);

                    // Add Management System Provider signature
                    AddSignatureToTable(table, "Property Tenant", TenantAgreementDetailData.Tenantfullname, TenantAgreementDetailData.TenantSignatureimageurl, TenantAgreementDetailData.TenantDatecreated);
                    // Add the table to the document
                    document.Add(table);

                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));
                    AddDetail(document, "This Agreement constitutes the entire understanding between the parties and supersedes all prior agreements, whether written or oral, relating to the subject matter herein.", bodyFont);


                    document.Close();
                }
                // Convert memory stream to byte array
                var pdfBytes = memoryStream.ToArray();

                // Upload to Firebase Storage
                var storage = new FirebaseStorage("uttambsolutions-4ec2a.appspot.com");
                var stream = new MemoryStream(pdfBytes);

                // Sanitize the file name to avoid issues with special characters
                string sanitizedFullName = TenantAgreementDetailData.Tenantfullname.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                string sanitizedPropertyName = TenantAgreementDetailData.Propertyhousename.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                var fileName = $"{sanitizedFullName}_{sanitizedPropertyName}_Tenant_Agreement.pdf";
                var uploadTask = storage.Child("maqaoplus").Child("agreements").Child(fileName).PutAsync(stream);
                var downloadUrl = await uploadTask;
                return downloadUrl;
            }
        }

        private void AddHeader(Document document, string text, Font font)
        {
            var paragraph = new Paragraph(text, font)
            {
                SpacingBefore = 10,
                SpacingAfter = 10
            };
            document.Add(paragraph);
        }

        private void AddDetail(Document document, string text, Font font)
        {
            var paragraph = new Paragraph(text, font)
            {
                FirstLineIndent = 20,
                SpacingAfter = 5
            };
            document.Add(paragraph);
        }

        private void AddSignatureToTable(PdfPTable table, string role, string name, string signatureImageUrl, DateTime date)
        {
            // Create a cell to hold the image and text
            var cell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 5,
                VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP // Aligns the content to the top
            };

            // Add the image
            if (!string.IsNullOrEmpty(signatureImageUrl))
            {
                try
                {
                    var signatureImage = iTextSharp.text.Image.GetInstance(signatureImageUrl);
                    signatureImage.ScaleToFit(150, 75); // Adjust size as needed

                    // Create a paragraph for the image and text
                    var imageParagraph = new Paragraph
                {
                    new Chunk(signatureImage, 0, 0),
                    new Chunk($"\n{name}", FontFactory.GetFont(FontFactory.HELVETICA, 12)),
                    new Chunk($"\n{role}", FontFactory.GetFont(FontFactory.HELVETICA, 12)),
                    new Chunk($"\nDate: {date:yyyy-MM-dd}", FontFactory.GetFont(FontFactory.HELVETICA, 12))
                };

                    cell.AddElement(imageParagraph);
                }
                catch (Exception ex)
                {
                    // Handle image loading exceptions
                    cell.AddElement(new Paragraph($"Error loading image: {ex.Message}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                }
            }
            else
            {
                cell.AddElement(new Paragraph($"No signature available for {role}.", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            }

            // Add cell to table
            table.AddCell(cell);
        }



        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
