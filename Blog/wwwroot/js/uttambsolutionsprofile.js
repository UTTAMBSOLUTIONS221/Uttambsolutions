function savesystemsocialmediadata() {
    document.getElementById("savesystemsocialmediaid").disabled = true;
    document.getElementById("processingSpinner").style.display = "inline-block";
    document.getElementById("buttonText").innerText = "Processing...";
    if ($('#Socialpagenameid').val() === '') {
        Swal.fire("Social Page Not Created", 'Page Name is Required', "warning");
        document.getElementById("savesystemsocialmediaid").disabled = false;
        document.getElementById("processingSpinner").style.display = "none";
        document.getElementById("buttonText").innerText = "SAVE";
        return;
    }
    if ($('#UserAccessTokenid').val() === '') {
        Swal.fire("Social Page Not Created", 'Access Token is Required', "warning");
        document.getElementById("savesystemsocialmediaid").disabled = false;
        document.getElementById("processingSpinner").style.display = "none";
        document.getElementById("buttonText").innerText = "SAVE";
        return;
    }
    if ($('#Appid').val() === '') {
        Swal.fire("Social Page Not Created", 'App Id is Required', "warning");
        document.getElementById("savesystemsocialmediaid").disabled = false;
        document.getElementById("processingSpinner").style.display = "none";
        document.getElementById("buttonText").innerText = "SAVE";
        return;
    }
    if ($('#Appsecretid').val() === '') {
        Swal.fire("Social Page Not Created", 'App Secret is Required', "warning");
        document.getElementById("savesystemsocialmediaid").disabled = false;
        document.getElementById("processingSpinner").style.display = "none";
        document.getElementById("buttonText").innerText = "SAVE";
        return;
    }

    if ($('#Pagetypeid').val() === '') {
        Swal.fire("Social Page Not Created", 'Page Type is Required', "warning");
        document.getElementById("savesystemsocialmediaid").disabled = false;
        document.getElementById("processingSpinner").style.display = "none";
        document.getElementById("buttonText").innerText = "SAVE";
        return;
    }

    var uil1 = {
        SocialSettingId: $('#SocialSettingId').val(), Socialpagename: $('#Socialpagenameid').val(), UserAccessToken: $('#UserAccessTokenid').val(), Appid: $('#Appid').val(), Appsecret: $('#Appsecretid').val(), PageType: $('#Pagetypeid').val(), Socialowner: $('#systemLoggedinedUserid').val(), Createdby: $('#systemLoggedinedUserid').val(), Modifiedby: $('#systemLoggedinedUserid').val(), Datecreated: new Date(new Date().toString().split('GMT')[0] + ' UTC').toISOString().split('.')[0].replace('T', ' '), Datemodified: new Date(new Date().toString().split('GMT')[0] + ' UTC').toISOString().split('.')[0].replace('T', ' ')
    };
    $.post("/Socialmedia/Addsocialmediapagedata", uil1, function (response) {
        if (response.RespStatus == 0) {
            Swal.fire('Saved!', response.RespMessage, 'success')
            $('#Uttambsolutionsmodallarge').hide();
            setTimeout(function () { location.reload(); }, 1000);
        } else if (response.RespStatus == 1) {
            Swal.fire("Module details not saved", response.RespMessage, "warning");
        }
        else {
            Swal.fire("Oops! Something Went Wrong", "Database Error has occured. Kindly Contact our support team.", "error");
        }
        document.getElementById("savesystemsocialmediaid").disabled = false;
        document.getElementById("processingSpinner").style.display = "none";
        document.getElementById("buttonText").innerText = "SAVE";
    });
}