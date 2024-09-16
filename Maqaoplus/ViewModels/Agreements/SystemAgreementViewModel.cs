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

namespace Maqaoplus.ViewModels.Agreements
{
    public class SystemAgreementViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        private OwnerTenantAgreementDetailData _ownerTenantAgreementDetailData;
        private TenantAgreementDetailData _tenantAgreementDetailData;
        public ICommand ViewPropertyAgentAgreementCommand { get; }
        public ICommand ViewPropertyOwnerAgreementCommand { get; }
        public ICommand ViewPropertyTenantAgreementCommand { get; }


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



        public OwnerTenantAgreementDetailData OwnerTenantAgreementDetailData
        {
            get => _ownerTenantAgreementDetailData;
            set
            {
                _ownerTenantAgreementDetailData = value;
                OnPropertyChanged(nameof(OwnerTenantAgreementDetailData));
                OnPropertyChanged(nameof(IsSignatureDrawingVisible));
                OnPropertyChanged(nameof(IsSignatureImageVisible));
                OnPropertyChanged(nameof(IsSignatureAvailable));
            }
        }

        public bool IsSignatureDrawingVisible => string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);
        public bool IsSignatureImageVisible => !string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);
        public bool IsSignatureAvailable => !string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);

        public TenantAgreementDetailData TenantAgreementDetailData
        {
            get => _tenantAgreementDetailData;
            set
            {
                _tenantAgreementDetailData = value;
                OnPropertyChanged(nameof(TenantAgreementDetailData));
                OnPropertyChanged(nameof(IsTenantSignatureDrawingVisible));
                OnPropertyChanged(nameof(IsTenantSignatureImageVisible));
                OnPropertyChanged(nameof(IsTenantSignatureAvailable));
            }
        }

        public bool IsTenantSignatureDrawingVisible => string.IsNullOrEmpty(TenantAgreementDetailData?.TenantSignatureimageurl);
        public bool IsTenantSignatureImageVisible => !string.IsNullOrEmpty(TenantAgreementDetailData?.TenantSignatureimageurl);
        public bool IsTenantSignatureAvailable
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
        public SystemAgreementViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            OwnerTenantAgreementDetailData = new OwnerTenantAgreementDetailData();
            TenantAgreementDetailData = new TenantAgreementDetailData();
            ViewPropertyAgentAgreementCommand = new Command(async () => await ViewPropertyAgentAgreementDetails());
            ViewPropertyOwnerAgreementCommand = new Command(async () => await ViewPropertyOwnerAgreementDetails());
            ViewPropertyTenantAgreementCommand = new Command(async () => await ViewPropertyTenantAgreementDetails());
        }
        #region Property Agent
        private async Task ViewPropertyAgentAgreementDetails()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseagreementdetaildatabyagentid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    OwnerTenantAgreementDetailData = JsonConvert.DeserializeObject<OwnerTenantAgreementDetailData>(response.Data.ToString());
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

        public async Task AgreeToPropertyAgentAgreementasync(string imageUrl)
        {
            IsProcessing = true;


            if (OwnerTenantAgreementDetailData == null)
            {
                IsProcessing = false;
                return;
            }
            OwnerTenantAgreementDetailData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            OwnerTenantAgreementDetailData.Signatureimageurl = imageUrl;
            OwnerTenantAgreementDetailData.Ownerortenant = "Agent";
            OwnerTenantAgreementDetailData.Agreementname = OwnerTenantAgreementDetailData.Fullname + " Agent Agreement";
            OwnerTenantAgreementDetailData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", OwnerTenantAgreementDetailData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    OwnerTenantAgreementDetailData.OwnerSignatureimageurl = response.Data2;
                    OwnerTenantAgreementDetailData.Agreementdetailpdfurl = await GenerateAndUploadAgentAgreementPdfAsync();
                    OwnerTenantAgreementDetailData.Agreementid = Convert.ToInt64(response.Data1);
                    var responseAfter = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", OwnerTenantAgreementDetailData);
                    if (responseAfter.RespStatus == 200 || responseAfter.RespStatus == 0)
                    {
                        (Shell.Current.CurrentPage.BindingContext as SystemAgreementViewModel)?.ViewPropertyAgentAgreementCommand.Execute(null);
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

        public async Task<string> GenerateAndUploadAgentAgreementPdfAsync()
        {
            if (OwnerTenantAgreementDetailData == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                using (var document = new Document(PageSize.A4, 50, 50, 50, 50))
                {
                    PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                    var sectionHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                    // Title
                    var titleParagraph = new Paragraph("RENTAL MANAGEMENT SYSTEM AGREEMENT", titleFont);
                    titleParagraph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    document.Add(titleParagraph);
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Add a line separator
                    var lineSeparator = new LineSeparator(1, 100, BaseColor.Black, iTextSharp.text.Element.ALIGN_CENTER, -2);
                    document.Add(lineSeparator);
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Date
                    document.Add(new Paragraph($"Date: {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd}", smallFont));
                    document.Add(new Paragraph(" ", smallFont)); // Add spacing

                    // Property Owner Details
                    document.Add(new Paragraph("Property Agent Details", sectionHeaderFont));
                    document.Add(new Paragraph($"Name: {OwnerTenantAgreementDetailData.Fullname}", regularFont));
                    document.Add(new Paragraph($"Phone: {OwnerTenantAgreementDetailData.Phonenumber}", regularFont));
                    document.Add(new Paragraph($"Email: {OwnerTenantAgreementDetailData.Emailaddress}", regularFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Rental Management System Provider
                    document.Add(new Paragraph("Rental Management System Provider", sectionHeaderFont));
                    document.Add(new Paragraph("Name: UTTAMB SOLUTIONS LIMITED", regularFont));
                    document.Add(new Paragraph("Address: Nairobi, Kenya", regularFont));
                    document.Add(new Paragraph("Phone: 0717850720", regularFont));
                    document.Add(new Paragraph("Email: support@uttambsolutions.com", regularFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Agreement Sections
                    AddAgentAgreementSection(document, "1. PURPOSE OF THE AGREEMENT", sectionHeaderFont, regularFont, $"The purpose of this Agreement is to outline the terms and conditions under which UTTAMB SOLUTIONS LIMITED (hereinafter referred to as the Management System Provider) will provide rental management services to {OwnerTenantAgreementDetailData.Fullname} (hereinafter referred to as the Property Agent)");
                    AddAgentAgreementSection(document, "2. SERVICES PROVIDED", sectionHeaderFont, regularFont, "- Advertising and Marketing: Listing the Property on various platforms to attract potential tenants.\n- Tenant Screening: Conducting background checks and verifying tenant credentials.\n- Rent Collection: Facilitating the collection of rent payments from tenants.\n- Property Maintenance: Coordinating with contractors for repairs and regular maintenance of the Property.\n- Reporting: Providing regular reports on the status of the Property, rent collection, and any issues that arise.");
                    AddAgentAgreementSection(document, "3. FEES AND PAYMENTS", sectionHeaderFont, regularFont, $"- Service Fee: The Property Agent agrees to pay the Management System Provider a service fee of 1% of the monthly rent collected.\n- Subscription Payment: The Property Agent agrees to pay a subscription fee for the services rendered by the Management System Provider. The subscription fee shall be paid monthly to the following bank account:\n\n  Bank Name: FAMILY BANK\n  Pay Bill: 222111\n  Account Number: 2340982\n\n- Payment Terms: The subscription fee is due on the 10th day of each month.\n- Additional Costs: Any costs related to property maintenance, legal fees, or other services not covered under this Agreement will be billed separately with the Property Agent's prior approval.");
                    AddAgentAgreementSection(document, "4. PROPERTY AGENT RESPONSIBILITIES", sectionHeaderFont, regularFont, "- Property Upkeep: The Property Agent agrees to maintain the Property in a condition suitable for rental.\n- Insurance: The Property Agent is responsible for obtaining and maintaining appropriate insurance coverage for the Property.\n- Legal Compliance: The Property Agent agrees to comply with all local, county, and national laws relating to the rental and maintenance of the Property.");
                    AddAgentAgreementSection(document, "5. DATA PROTECTION AND PRIVACY", sectionHeaderFont, regularFont, "- Compliance with Data Protection Act, 2019: The Management System Provider shall ensure that all personal data collected, processed, and stored as part of the rental management services is handled in accordance with the Data Protection Act, 2019 of Kenya.\n- Data Security: Both parties agree to implement appropriate technical and organizational measures to protect personal data against unauthorized or unlawful processing, accidental loss, destruction, or damage.");
                    AddAgentAgreementSection(document, "6. TERM AND TERMINATION", sectionHeaderFont, regularFont, $"- Term: This Agreement will begin on {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd} and will continue until terminated by either party.\n- Termination: Either party may terminate this Agreement with 14 days' written notice. Upon termination, the Property Agent is responsible for any outstanding fees and obligations under this Agreement.");
                    AddAgentAgreementSection(document, "7. INDEMNIFICATION", sectionHeaderFont, regularFont, "The Property Agent agrees to indemnify and hold harmless the Management System Provider from any claims, liabilities, or damages arising out of the management of the Property, except in cases of gross negligence or willful misconduct by the Management System Provider.");
                    AddAgentAgreementSection(document, "8. GOVERNING LAW", sectionHeaderFont, regularFont, "This Agreement shall be governed by and construed in accordance with the laws of Kenya.");
                    AddAgentAgreementSection(document, "9. ENTIRE AGREEMENT", sectionHeaderFont, regularFont, "This Agreement constitutes the entire agreement between the parties with respect to its subject matter and supersedes all prior agreements and understandings, whether written or oral.");

                    // Signatures
                    document.Add(new Paragraph("AGREED AND ACCEPTED", sectionHeaderFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Create a table with 2 columns
                    var table = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };

                    // Set column widths (adjust as necessary)
                    table.SetWidths(new float[] { 1f, 1f });
                    // Add Management System Provider signature
                    AddAgentSignatureToTable(table, "Management System Provider", "Francis Kingori-Director \n Uttamb Solutions Limited", "https://firebasestorage.googleapis.com/v0/b/uttambsolutions-4ec2a.appspot.com/o/UttambSolutionsPrivate%2Fmysignature.jpg?alt=media&token=d970f2d8-f4bd-4a30-b47e-12d9f8d1edc9", OwnerTenantAgreementDetailData.OwnerDatecreated);
                    // Add Property Owner signature
                    AddAgentSignatureToTable(table, "Property Agent", OwnerTenantAgreementDetailData.Fullname, OwnerTenantAgreementDetailData.OwnerSignatureimageurl, OwnerTenantAgreementDetailData.OwnerDatecreated);


                    // Add the table to the document
                    document.Add(table);

                    document.Add(new Paragraph(" ", regularFont)); // Add spacing
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing
                    document.Add(new Paragraph("This Agreement constitutes the entire understanding between the parties and supersedes all prior agreements, whether written or oral, relating to the subject matter herein.", regularFont));

                    // Close the document
                    document.Close();
                }

                // Convert memory stream to byte array
                var pdfBytes = memoryStream.ToArray();

                // Upload to Firebase Storage
                var storage = new FirebaseStorage("uttambsolutions-4ec2a.appspot.com");
                var stream = new MemoryStream(pdfBytes);

                // Sanitize the file name to avoid issues with special characters
                string sanitizedFullName = OwnerTenantAgreementDetailData.Fullname.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                string sanitizedPropertyName = OwnerTenantAgreementDetailData.Propertyhousename.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                var fileName = $"{sanitizedFullName}_{sanitizedPropertyName}_Owner_Agreement.pdf";
                var uploadTask = storage.Child("maqaoplus").Child("agreements").Child(fileName).PutAsync(stream);
                var downloadUrl = await uploadTask;
                return downloadUrl;
            }
        }

        private void AddAgentAgreementSection(Document document, string title, Font titleFont, Font contentFont, string content)
        {
            document.Add(new Paragraph(title, titleFont));
            document.Add(new Paragraph(content, contentFont));
            document.Add(new Paragraph(" ", contentFont)); // Add spacing
        }

        private void AddAgentSignatureToTable(PdfPTable table, string role, string name, string signatureImageUrl, DateTime date)
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

        #endregion


        #region Property Owners
        private async Task ViewPropertyOwnerAgreementDetails()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseagreementdetaildatabyownerid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    OwnerTenantAgreementDetailData = JsonConvert.DeserializeObject<OwnerTenantAgreementDetailData>(response.Data.ToString());
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

        public async Task AgreeToPropertyOwnerAgreementasync(string imageUrl)
        {
            IsProcessing = true;


            if (OwnerTenantAgreementDetailData == null)
            {
                IsProcessing = false;
                return;
            }
            OwnerTenantAgreementDetailData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            OwnerTenantAgreementDetailData.Signatureimageurl = imageUrl;
            OwnerTenantAgreementDetailData.Ownerortenant = "Owner";
            OwnerTenantAgreementDetailData.Agreementname = OwnerTenantAgreementDetailData.Fullname + " Property " + OwnerTenantAgreementDetailData.Propertyhousename + " Owner Agreement";
            OwnerTenantAgreementDetailData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", OwnerTenantAgreementDetailData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    OwnerTenantAgreementDetailData.OwnerSignatureimageurl = response.Data2;
                    OwnerTenantAgreementDetailData.Agreementdetailpdfurl = await GenerateAndUploadOwnerAgreementPdfAsync();
                    OwnerTenantAgreementDetailData.Agreementid = Convert.ToInt64(response.Data1);
                    var responseAfter = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", OwnerTenantAgreementDetailData);
                    if (responseAfter.RespStatus == 200 || responseAfter.RespStatus == 0)
                    {
                        (Shell.Current.CurrentPage.BindingContext as SystemAgreementViewModel)?.ViewPropertyOwnerAgreementCommand.Execute(null);
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

        public async Task<string> GenerateAndUploadOwnerAgreementPdfAsync()
        {
            if (OwnerTenantAgreementDetailData == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                using (var document = new Document(PageSize.A4, 50, 50, 50, 50))
                {
                    PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                    var sectionHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                    // Title
                    var titleParagraph = new Paragraph("RENTAL MANAGEMENT SYSTEM AGREEMENT", titleFont);
                    titleParagraph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    document.Add(titleParagraph);
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Add a line separator
                    var lineSeparator = new LineSeparator(1, 100, BaseColor.Black, iTextSharp.text.Element.ALIGN_CENTER, -2);
                    document.Add(lineSeparator);
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Date
                    document.Add(new Paragraph($"Date: {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd}", smallFont));
                    document.Add(new Paragraph(" ", smallFont)); // Add spacing

                    // Property Owner Details
                    document.Add(new Paragraph("Property Owner Details", sectionHeaderFont));
                    document.Add(new Paragraph($"Property: {OwnerTenantAgreementDetailData.Propertyhousename}", regularFont));
                    document.Add(new Paragraph($"Name: {OwnerTenantAgreementDetailData.Fullname}", regularFont));
                    document.Add(new Paragraph($"Address: {OwnerTenantAgreementDetailData.Countyname}, {OwnerTenantAgreementDetailData.Subcountyname}, {OwnerTenantAgreementDetailData.Subcountywardname}", regularFont));
                    document.Add(new Paragraph($"Phone: {OwnerTenantAgreementDetailData.Phonenumber}", regularFont));
                    document.Add(new Paragraph($"Email: {OwnerTenantAgreementDetailData.Emailaddress}", regularFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Rental Management System Provider
                    document.Add(new Paragraph("Rental Management System Provider", sectionHeaderFont));
                    document.Add(new Paragraph("Name: UTTAMB SOLUTIONS LIMITED", regularFont));
                    document.Add(new Paragraph("Address: Nairobi, Kenya", regularFont));
                    document.Add(new Paragraph("Phone: 0717850720", regularFont));
                    document.Add(new Paragraph("Email: support@uttambsolutions.com", regularFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Agreement Sections
                    AddOwnerAgreementSection(document, "1. PURPOSE OF THE AGREEMENT", sectionHeaderFont, regularFont, $"The purpose of this Agreement is to outline the terms and conditions under which UTTAMB SOLUTIONS LIMITED (hereinafter referred to as the Management System Provider) will provide rental management services to {OwnerTenantAgreementDetailData.Fullname} (hereinafter referred to as the Property Owner) for the property located at {OwnerTenantAgreementDetailData.Countyname}, {OwnerTenantAgreementDetailData.Subcountyname}, {OwnerTenantAgreementDetailData.Subcountywardname} (hereinafter referred to as the Property).");
                    AddOwnerAgreementSection(document, "2. SERVICES PROVIDED", sectionHeaderFont, regularFont, "- Advertising and Marketing: Listing the Property on various platforms to attract potential tenants.\n- Tenant Screening: Conducting background checks and verifying tenant credentials.\n- Rent Collection: Facilitating the collection of rent payments from tenants.\n- Property Maintenance: Coordinating with contractors for repairs and regular maintenance of the Property.\n- Reporting: Providing regular reports on the status of the Property, rent collection, and any issues that arise.");
                    AddOwnerAgreementSection(document, "3. FEES AND PAYMENTS", sectionHeaderFont, regularFont, $"- Service Fee: The Property Owner agrees to pay the Management System Provider a service fee of 1% of the monthly rent collected for Collections More than Kes. 100,000 and 2% for collections less than Kes. 100,000.\n- Subscription Payment: The Property Owner agrees to pay a subscription fee for the services rendered by the Management System Provider. The subscription fee shall be paid monthly to the following bank account:\n\n  Bank Name: FAMILY BANK\n  Pay Bill: 222111\n  Account Number: 2340982\n\n- Payment Terms: The subscription fee is due on the 10th day of each month.\n- Additional Costs: Any costs related to property maintenance, legal fees, or other services not covered under this Agreement will be billed separately with the Property Owner's prior approval.");
                    AddOwnerAgreementSection(document, "4. PROPERTY OWNER RESPONSIBILITIES", sectionHeaderFont, regularFont, "- Property Upkeep: The Property Owner agrees to maintain the Property in a condition suitable for rental.\n- Insurance: The Property Owner is responsible for obtaining and maintaining appropriate insurance coverage for the Property.\n- Legal Compliance: The Property Owner agrees to comply with all local, county, and national laws relating to the rental and maintenance of the Property.");
                    AddOwnerAgreementSection(document, "5. DATA PROTECTION AND PRIVACY", sectionHeaderFont, regularFont, "- Compliance with Data Protection Act, 2019: The Management System Provider shall ensure that all personal data collected, processed, and stored as part of the rental management services is handled in accordance with the Data Protection Act, 2019 of Kenya.\n- Data Security: Both parties agree to implement appropriate technical and organizational measures to protect personal data against unauthorized or unlawful processing, accidental loss, destruction, or damage.");
                    AddOwnerAgreementSection(document, "6. TERM AND TERMINATION", sectionHeaderFont, regularFont, $"- Term: This Agreement will begin on {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd} and will continue until terminated by either party.\n- Termination: Either party may terminate this Agreement with 14 days' written notice. Upon termination, the Property Owner is responsible for any outstanding fees and obligations under this Agreement.");
                    AddOwnerAgreementSection(document, "7. INDEMNIFICATION", sectionHeaderFont, regularFont, "The Property Owner agrees to indemnify and hold harmless the Management System Provider from any claims, liabilities, or damages arising out of the management of the Property, except in cases of gross negligence or willful misconduct by the Management System Provider.");
                    AddOwnerAgreementSection(document, "8. GOVERNING LAW", sectionHeaderFont, regularFont, "This Agreement shall be governed by and construed in accordance with the laws of Kenya.");
                    AddOwnerAgreementSection(document, "9. ENTIRE AGREEMENT", sectionHeaderFont, regularFont, "This Agreement constitutes the entire agreement between the parties with respect to its subject matter and supersedes all prior agreements and understandings, whether written or oral.");

                    // Signatures
                    document.Add(new Paragraph("AGREED AND ACCEPTED", sectionHeaderFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Create a table with 2 columns
                    var table = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };

                    // Set column widths (adjust as necessary)
                    table.SetWidths(new float[] { 1f, 1f });
                    // Add Management System Provider signature
                    AddOwnerSignatureToTable(table, "Management System Provider", "Francis Kingori-Director \n Uttamb Solutions Limited", "https://firebasestorage.googleapis.com/v0/b/uttambsolutions-4ec2a.appspot.com/o/UttambSolutionsPrivate%2Fmysignature.jpg?alt=media&token=d970f2d8-f4bd-4a30-b47e-12d9f8d1edc9", OwnerTenantAgreementDetailData.OwnerDatecreated);
                    // Add Property Owner signature
                    AddOwnerSignatureToTable(table, "Property Owner", OwnerTenantAgreementDetailData.Fullname, OwnerTenantAgreementDetailData.OwnerSignatureimageurl, OwnerTenantAgreementDetailData.OwnerDatecreated);


                    // Add the table to the document
                    document.Add(table);

                    document.Add(new Paragraph(" ", regularFont)); // Add spacing
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing
                    document.Add(new Paragraph("This Agreement constitutes the entire understanding between the parties and supersedes all prior agreements, whether written or oral, relating to the subject matter herein.", regularFont));

                    // Close the document
                    document.Close();
                }

                // Convert memory stream to byte array
                var pdfBytes = memoryStream.ToArray();

                // Upload to Firebase Storage
                var storage = new FirebaseStorage("uttambsolutions-4ec2a.appspot.com");
                var stream = new MemoryStream(pdfBytes);

                // Sanitize the file name to avoid issues with special characters
                string sanitizedFullName = OwnerTenantAgreementDetailData.Fullname.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                string sanitizedPropertyName = OwnerTenantAgreementDetailData.Propertyhousename.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                var fileName = $"{sanitizedFullName}_{sanitizedPropertyName}_Owner_Agreement.pdf";
                var uploadTask = storage.Child("maqaoplus").Child("agreements").Child(fileName).PutAsync(stream);
                var downloadUrl = await uploadTask;
                return downloadUrl;
            }
        }

        private void AddOwnerAgreementSection(Document document, string title, Font titleFont, Font contentFont, string content)
        {
            document.Add(new Paragraph(title, titleFont));
            document.Add(new Paragraph(content, contentFont));
            document.Add(new Paragraph(" ", contentFont)); // Add spacing
        }

        private void AddOwnerSignatureToTable(PdfPTable table, string role, string name, string signatureImageUrl, DateTime date)
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


        #endregion

        #region Property Tenant
        private async Task ViewPropertyTenantAgreementDetails()
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

        public async Task AgreeToPropertyTenantAgreementasync(string imageUrl)
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
                    TenantAgreementDetailData.Agreementdetailpdfurl = await GenerateAndUploadTenantAgreementPdfAsync();
                    TenantAgreementDetailData.Agreementid = Convert.ToInt64(response.Data1);
                    var responseAfter = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", TenantAgreementDetailData);
                    if (responseAfter.RespStatus == 200 || responseAfter.RespStatus == 0)
                    {
                        (Shell.Current.CurrentPage.BindingContext as SystemAgreementViewModel)?.ViewPropertyTenantAgreementCommand.Execute(null);
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
        public async Task<string> GenerateAndUploadTenantAgreementPdfAsync()
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

        #endregion
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
