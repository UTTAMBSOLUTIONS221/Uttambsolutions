const firebaseConfig = {
    apiKey: "AIzaSyCmRT7WLWSDUAqUmUCNRVlTcZ1ZOPsmgwc",
    authDomain: "uttambsolutions-4ec2a.firebaseapp.com",
    projectId: "uttambsolutions-4ec2a",
    storageBucket: "uttambsolutions-4ec2a.appspot.com",
    messagingSenderId: "336739823411",
    appId: "1:336739823411:web:f2ad2d4dba0f7881f5d846",
    measurementId: "G-Z24LLL0113"
};
firebase.initializeApp(firebaseConfig);
var storage = firebase.storage();

function churcheventimageFileChanged(event) {
    var files = event.target.files;
    uploadCompanyLogoImage(files);
    console.log(document.getElementById('UttambsolutionsimagesurlId').files);
}

async function uploadCompanyLogoImage(files) {
    // Array to store the download URLs
    var downloadURLs = [];

    // Function to generate a unique file name
    function generateFileName(file) {
        // Generate a unique identifier
        var uniqueId = Date.now();
        // Get the file extension
        var fileExtension = file.name.split('.').pop();
        // Construct the new file name
        var newFileName = 'Church/ChurchEvent/' + uniqueId + '.' + fileExtension;
        return newFileName;
    }

    // Function to upload a single file
    async function uploadFile(file) {
        return new Promise((resolve, reject) => {
            var newFileName = generateFileName(file); // Generate a new file name

            var storageRef = storage.ref().child(newFileName);
            var uploadTask = storageRef.put(file);

            uploadTask.on('state_changed',
                function (snapshot) {
                    var progress = (snapshot.bytesTransferred / snapshot.totalBytes) * 100;
                    console.log('Upload is ' + progress + '% done');
                },
                function (error) {
                    reject('Error uploading file: ' + error);
                },
                function () {
                    // Upload complete
                    uploadTask.snapshot.ref.getDownloadURL().then(function (downloadURL) {
                        console.log('File available at', downloadURL);
                        downloadURLs.push(downloadURL);
                        resolve(downloadURL);
                    });
                }
            );
        });
    }

    // Function to display image preview
    function displayImagePreview(file) {
        return new Promise((resolve, reject) => {
            var reader = new FileReader();
            reader.onload = function (e) {
                // Create a new image element
                var img = document.createElement('img');
                img.src = e.target.result;
                img.width = 140;
                img.height = 140;

                // Append the image to a container element
                var previewContainer = document.getElementById('Uttambsolutionsimagespreviewcontainer');
                previewContainer.appendChild(img);

                resolve();
            };
            reader.readAsDataURL(file);
        });
    }

    // Iterate over each file
    for (var i = 0; i < files.length; i++) {
        var file = files[i];

        try {
            // Upload the file and get the download URL
            var downloadURL = await uploadFile(file);

            // Display image preview
            await displayImagePreview(file);
        } catch (error) {
            console.error(error);
        }
    }

    // Set the input field value with JSON stringified URLs
    var inputField = document.getElementById('UttambsolutionsimagesurlId');
    inputField.value = JSON.stringify(downloadURLs);
}


