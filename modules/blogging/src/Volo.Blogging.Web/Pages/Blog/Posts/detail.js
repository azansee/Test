﻿(function ($) {

    $('div .replyForm').hide();
    $('div .editForm').hide();

    $('form[class="editFormClass"]').submit(function (event) {
        event.preventDefault();
        var form = $(this).serializeFormToObject();

        $.ajax({
            type: "POST",
            url: "/Blog/Comments/Update",
            data: {
                id: form.commentId,
                commentDto: {
                    text: form.text
                }
            },
            success: function (response) {
                $('div .editForm').hide();
                $('#' + form.commentId).text(form.text);
            }
        });
    });

    $('.replyLink').click(function (event) {
        event.preventDefault();
        var linkElement = $(this);
        var replyCommentId = linkElement.attr('data-relpyid');

        if (replyCommentId != '' && replyCommentId !== undefined) {
            var div = linkElement.parent().next();

            if (div.is(":hidden")) {
                $('div .replyForm').hide();
                div.show();
            } else {
                div.hide();
            }
            return;
        }
    });

    $('.deleteLink').click(function(event) {
        event.preventDefault();
        var linkElement = $(this);
        var deleteCommentId = linkElement.attr('data-deleteid');

        if (deleteCommentId != '' && deleteCommentId !== undefined) {
            abp.message.confirm(
                'Comment will be deleted.', // TODO: localize
                'Are you sure?',
                function(isConfirmed) {
                    if (isConfirmed) {
                        $.ajax({
                            type: "POST",
                            url: "/Blog/Comments/Delete",
                            data: { id: deleteCommentId },
                            success: function (response) {
                                linkElement.parent().parent().parent().remove();
                            }
                        });
                    }
                }
            );
        }
    });

    $('.updateLink').click(function (event) {
        event.preventDefault();
        var linkElement = $(this);
        var updateCommentId = $(this).attr('data-updateid');

        if (updateCommentId != '' && updateCommentId !== undefined) {

            var div = linkElement.parent().next().next();

            if (div.is(":hidden")) {
                $('div .editForm').hide();
                div.show();
            } else {
                div.hide();
            }
            return;
        }
    });

})(jQuery);
