using DBL.Models;
using Firebase.Storage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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
        public bool IsSignatureAvailable => !string.IsNullOrEmpty(TenantAgreementDetailData?.TenantSignatureimageurl);
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

            await Task.Delay(500);
            if (TenantAgreementDetailData == null)
            {
                IsProcessing = false;
                return;
            }
            TenantAgreementDetailData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            TenantAgreementDetailData.Signatureimageurl = imageUrl;
            TenantAgreementDetailData.Ownerortenant = "Tenant";
            TenantAgreementDetailData.Agreementname = TenantAgreementDetailData.Tenantfullname + " Room " + TenantAgreementDetailData.Propertyhousename + " " + TenantAgreementDetailData.Systempropertyhousesizename + " " + TenantAgreementDetailData.Systempropertyhousesizename + " Tenant Agreement";
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
                        Application.Current.MainPage.Navigation.PopModalAsync();
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
                using (var document = new Document(PageSize.A4))
                {
                    PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 14);

                    // Title
                    var titleParagraph = new Paragraph("TENANT RENTAL AGREEMENT", boldFont);
                    titleParagraph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    document.Add(titleParagraph);
                    document.Add(new Paragraph(" ")); // Add spacing

                    // Agreement Details
                    document.Add(new Paragraph($"This Rental Agreement (Agreement) is made and entered into on this {TenantAgreementDetailData.TenantDatecreated:dd} day of {TenantAgreementDetailData.TenantDatecreated:MMM}, 20{TenantAgreementDetailData.TenantDatecreated:yy}, by and between:"));

                    // Landlord Details
                    document.Add(new Paragraph($"Landlord: {TenantAgreementDetailData.Ownerfullname}"));
                    document.Add(new Paragraph($"Property Name: {TenantAgreementDetailData.Propertyhousename}"));
                    document.Add(new Paragraph($"Address: {TenantAgreementDetailData.Countyname}-{TenantAgreementDetailData.Subcountyname}-{TenantAgreementDetailData.Subcountywardname}"));
                    document.Add(new Paragraph($"Phone Number: {TenantAgreementDetailData.Ownerphonenumber}"));
                    document.Add(new Paragraph($"Email Address: {TenantAgreementDetailData.Owneremailaddress}"));

                    // Tenant Details
                    document.Add(new Paragraph("AND"));
                    document.Add(new Paragraph($"Tenant: {TenantAgreementDetailData.Tenantfullname}"));
                    document.Add(new Paragraph($"ID/Passport Number: {TenantAgreementDetailData.Tenantidnumber}"));
                    document.Add(new Paragraph($"Phone Number: {TenantAgreementDetailData.Tenantphonenumber}"));
                    document.Add(new Paragraph($"Email Address: {TenantAgreementDetailData.Tenantemailaddress}"));

                    // Agreement Sections
                    document.Add(new Paragraph($"1. PREMISES: The Landlord hereby agrees to rent to the Tenant, and the Tenant hereby agrees to rent from the Landlord {TenantAgreementDetailData.Propertyhousename}, the residential premises located at: {TenantAgreementDetailData.Countyname}-{TenantAgreementDetailData.Subcountyname}-{TenantAgreementDetailData.Subcountywardname}"));
                    document.Add(new Paragraph($"Land Mark: {TenantAgreementDetailData.Streetorlandmark}"));

                    // Term
                    var startDate = TenantAgreementDetailData.TenantDatecreated.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var endDate = TenantAgreementDetailData.TenantDatecreated.AddMonths(12).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); // Example of fixed-term lease
                    document.Add(new Paragraph($"2. TERM: The term of this rental agreement shall commence on {startDate} (Start Date) and shall continue as follows:"));
                    document.Add(new Paragraph(TenantAgreementDetailData.Monthlyrentterms
                        ? $"Month-to-month tenancy beginning on {startDate}."
                        : $"Fixed-term lease ending on {endDate}."));

                    // Rent
                    document.Add(new Paragraph($"3. RENT: The Tenant agrees to pay the Landlord a monthly rent of Ksh. {TenantAgreementDetailData.Systempropertyhousesizerent:#,##0.00}, payable in advance on or before the {TenantAgreementDetailData.Rentdueday} day of each month. The first rent payment is due on {TenantAgreementDetailData.Nextrentduedate:yyyy-MM-dd}."));

                    // Security Deposit
                    document.Add(new Paragraph($"4. SECURITY DEPOSIT: The Tenant agrees to pay a security deposit of Ksh. {TenantAgreementDetailData.Systempropertyhousesizerentdeposit:#,##0.00}, equivalent to {TenantAgreementDetailData.Rentdepositmonth} month's rent, to be held by the Landlord as security for the performance of the Tenant's obligations under this Agreement. The security deposit will be refunded to the Tenant within {TenantAgreementDetailData.Rentdepositrefunddays} days after vacating the premises, less any deductions for damages beyond normal wear and tear."));

                    // Utilities
                    document.Add(new Paragraph("5. UTILITIES:"));
                    document.Add(new Paragraph(TenantAgreementDetailData.Rentutilityinclusive
                        ? "All utilities (e.g., electricity, water) are included in the rent."
                        : $"The Tenant shall be responsible for the payment of the following utilities: {TenantAgreementDetailData.Propertyhouseutility}"));

                    // Additional Sections
                    document.Add(new Paragraph("6. MAINTENANCE AND REPAIRS: The Tenant agrees to keep the premises in a clean and habitable condition and to promptly notify the Landlord of any necessary repairs. The Tenant shall not make any alterations to the premises without the prior written consent of the Landlord."));
                    document.Add(new Paragraph($"7. OCCUPANTS: The premises shall be occupied by the Tenant and the following individuals:\n{TenantAgreementDetailData.Tenantsintheroom}"));
                    document.Add(new Paragraph("8. PETS:"));
                    document.Add(new Paragraph(TenantAgreementDetailData.Allowpets
                        ? "Pets are allowed, subject to the following conditions: [Specify pet conditions here]"
                        : "No pets are allowed on the premises"));
                    document.Add(new Paragraph("9. PAYMENT DETAILS: The Tenant shall make all payments to the following bank account details:"));
                    document.Add(new Paragraph($"Banking Details: {TenantAgreementDetailData.Systempropertybankname}"));
                    document.Add(new Paragraph("10. INSURANCE: The Tenant is encouraged to obtain renter's insurance to cover personal property against loss or damage. The Landlord shall not be responsible for any loss or damage to the Tenant's personal property."));
                    document.Add(new Paragraph("11. DISPUTE RESOLUTION: Any disputes arising out of or relating to this Agreement shall be resolved through mediation or arbitration, as per the laws of Kenya. The parties agree to seek resolution through these methods before pursuing any legal action."));
                    document.Add(new Paragraph("12. GOVERNING LAW: This Agreement shall be governed by and construed in accordance with the laws of Kenya."));
                    document.Add(new Paragraph("13. ENTIRE AGREEMENT: This Agreement constitutes the entire agreement between the parties and supersedes all prior agreements or understandings, whether written or oral, relating to the subject matter hereof."));

                    // Additional Clauses
                    document.Add(new Paragraph("14. WEAR AND TEAR: Normal wear and tear refers to the deterioration of the premises and its fixtures caused by ordinary use over time, without negligence or abuse. Examples include minor scuffs on walls, worn carpet, or faded paint. Damage beyond normal wear and tear includes, but is not limited to, large holes in walls, broken windows, and significant damage to appliances or fixtures. The Tenant is responsible for repairs or costs associated with damage beyond normal wear and tear. The Landlord shall provide a written estimate for such repairs, and the Tenant shall have the right to contest the charges if they disagree."));

                    document.Add(new Paragraph("15. DATA PROTECTION: The Landlord and Tenant agree to comply with all applicable data protection laws and regulations. The Tenant's personal data collected under this Agreement will be used solely for the purpose of managing the rental relationship and will not be shared with third parties without the Tenant's consent, except as required by law. The Tenant has the right to access, correct, or delete their personal data upon request."));
                    // Signatures
                    document.Add(new Paragraph("AGREED AND ACCEPTED", boldFont));
                    document.Add(new Paragraph(" ")); // Add spacing

                    // Signatures
                    var signatureOwnerImage = iTextSharp.text.Image.GetInstance(TenantAgreementDetailData.OwnerSignatureimageurl);
                    signatureOwnerImage.ScaleToFit(200, 50);
                    document.Add(signatureOwnerImage);
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("Property Owner", regularFont));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph($"Date: {TenantAgreementDetailData.TenantDatecreated:yyyy-MM-dd}", regularFont));
                    document.Add(new Paragraph(" "));



                    // Add the signature image
                    var signatureTenantImage = iTextSharp.text.Image.GetInstance(TenantAgreementDetailData.TenantSignatureimageurl);
                    signatureTenantImage.ScaleToFit(200, 50);
                    document.Add(signatureTenantImage);
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("Tenant:", regularFont));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph($"Date: {TenantAgreementDetailData.TenantDatecreated:yyyy-MM-dd}", regularFont));
                    document.Add(new Paragraph(" "));


                    // Close the document
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
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
