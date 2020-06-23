Dropzone.autoDiscover = false;
$(function () {
    $(".ddzone").on({
        dragover: function (e) {
            var $this = $(this);
            $this.addClass('in');
        },
        dragleave: function (e) {
            var $this = $(this);
            $this.removeClass('in');
        }
    });

    var myDropzone;
    $(".ddzone").each(function () {
        var $this = $(this);
        var $this_fileinput = $this.find("div.fileinput").get(0);

        myDropzone = new Dropzone($this.get(0),
            {
                //$this.Dropzone({
                paramName: "files", // The name that will be used to transfer the file
                maxFilesize: 2, // MB
                
                url: url_action,
                params: {
                    domain: $this.data("domain"),
                    id: $this.data("id")
                },
                acceptedFiles: '.pdf,.doc,.docx,.txt,.xls,.xlsx,.csv,.html,.htm,.cshtm,.js,.zip,.rar',
                //addRemoveLinks: true,
                clickable: $this_fileinput, //".ddzone" $this, // ".fileinput-button" Define the element that should be used as click trigger to select files

                success: function (file, response) {
                    console.log("Successfully uploaded " + file);
                    file.previewElement.innerHTML = "";

                    
                    sessionStorage.setItem("msg_cls", "alert-success");
                    sessionStorage.setItem("msg_alert", 'Uppladdning av filen:' + file.name +' klar.');
                    window.location.reload();
                },
                error: function (file, response) {
                    console.log("something goes wrong: " + response);
                    file.previewElement.innerHTML = "";
                    //var errorDisplay = document.querySelectorAll('[data-dz-errormessage]');
                    //errorDisplay[errorDisplay.length - 1].innerHTML = 'Error';  //'@TempData["FailText"]

                    sessionStorage.setItem("msg_cls", "alert-danger");
                    sessionStorage.setItem("msg_alert", 'Uppladdning av filen:' + file.name + ' mislyckades. Fel:' + response);
                    msgAlert();
                }
            });
    });

});