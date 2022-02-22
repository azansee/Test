﻿var editor = null;
(function ($) {

    var $container = $("#qa-new-post-container");
    var $editorContainer = $container.find(".new-post-editor");
    var $submitButton = $container.find("button[type=submit]");
    var editorDataKey = "tuiEditor";

    var uploadImage = function (file, callbackFn) {
        var formData = new FormData();
        formData.append('file', file);

        $.ajax({ 
            type: "POST",
            url: "",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                callbackFn(response.fileUrl);
            }
        });
    };

    var newPostEditor = $editorContainer.tuiEditor({
        usageStatistics: false,
        initialEditType: 'markdown',
        previewStyle: 'tab',
        height: "auto",
        hooks: {
            addImageBlobHook: function (blob, callback, source) {
                var imageAltText = blob.name;

                uploadImage(blob,
                    function (fileUrl) {
                        callback(fileUrl, imageAltText);
                    });
            }
        },
        events: {
            load: function () {
                $editorContainer.find(".loading-cover").remove();
                $submitButton.prop("disabled", false);
            }
        }
    }).data(editorDataKey);



    $container.find("form#new-post-form").submit(function (e) {
        var $form = $container.find("form#new-post-form");
        var $postTextInput = $form.find("input[name='Post.Content']");

        var postText = newPostEditor.getMarkdown();
        $postTextInput.val(postText);
        console.log(postText);

        $submitButton.buttonBusy();
        $(this).off('submit').submit();
    });

    $('#Post_Title').on("change paste keyup", function() {
        var title = $('#Post_Title').val();

        if (title.length > 64) {
            title = title.substring(0, 64);
        }

        title = title.replace(' ','-');

        $('#Post_Url').val(title);
    });

})(jQuery);
