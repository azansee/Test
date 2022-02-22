﻿$(function () {
    var _identityUserAppService = volo.abp.identity.identityUser;
    var _localize = abp.localization.getResource('AbpIdentityWeb');

    var dataTable = $('#IdentityUsersTable').DataTable({
        order: [[1, "asc"]],
        ajax: function (requestData, callback, settings) {
            var inputFilter = {};

            //set paging filters
            if (settings.oInit.paging) {
                inputFilter = $.extend(inputFilter, {
                    maxResultCount: requestData.length,
                    skipCount: requestData.start
                });
            }

            //set sorting filter
            if (requestData.order && requestData.order.length > 0) {
                var orderingField = requestData.order[0];
                if (requestData.columns[orderingField.column].data) {
                    inputFilter.sorting = requestData.columns[orderingField.column].data + " " + orderingField.dir;
                }
            }

            //set searching filter
            if (requestData.search && requestData.search.value !== "") {
                inputFilter.filter = requestData.search.value;
            }

            if (callback) {
                _identityUserAppService.getList(inputFilter).done(function (result) {
                    callback({
                        recordsTotal: result.totalCount,
                        recordsFiltered: result.totalCount,
                        data: result.items
                    });
                });
            }
        },
        //TODO: localize strings after imlementation of js localization
        columnDefs: [
            {
                targets: 0,
                data: null,
                orderable: false,
                autoWidth: false,
                defaultContent: '',
                render: function (list, type, record, meta) {
                    return '<div class="dropdown">' +
                            '<button class="btn btn-primary btn-sm dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">' +
                                'Actions' +
                            '</button>' +
                            '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                                '<a class="dropdown-item update-user" href="#" data-id="' + record.id + '">Edit</a>' +
                                '<a class="dropdown-item delete-user" href="#" data-id="' + record.id + '">Delete</a>' +
                            '</div>' +
                        '</div>';
                }
            },
            {
                targets: 1,
                data: "userName"
            },
            {
                targets: 2,
                data: "email"
            },
            {
                targets: 3,
                data: "phoneNumber"
            }
        ]
    });

    $('#IdentityUsersTable').on('click', '.update-user', function () {
        var id = $(this).data('id');

        $('#createUpdateUserModal').modal('show')
            .find('.modal-content')
            .load(abp.appPath + 'Identity/Users/Update', { id: id });
    });

    $('#IdentityUsersTable').on('click', '.delete-user', function () {
        var id = $(this).data('id');
        var userName = $(this).data('userName');

        if (confirm(_localize('UserDeletionConfirmationMessage', userName))) {
            _identityUserAppService.delete(id).done(function () {
                dataTable.ajax.reload();
            });
        }
    });

    $('.create-user').click(function () {
        $('#createUpdateUserModal').modal('show')
            .find('.modal-content')
            .load(abp.appPath + 'Identity/Users/Create');
    });

    $('#createUpdateUserModal').on('click', '#btnCreateUserSave', function () {
        var $createUserForm = $('#createUserForm');
        var user = $createUserForm.serializeFormToObject();
        user.RoleNames = findAssignedRoleNames();

        _identityUserAppService.create(user).done(function () {
            $('#createUpdateUserModal').modal('hide');
            dataTable.ajax.reload();
        });
    });

    $('#createUpdateUserModal').on('click', '#btnUpdateUserSave', function () {
        var $updateUserForm = $('#updateUserForm');
        var user = $updateUserForm.serializeFormToObject();
        user.RoleNames = findAssignedRoleNames();

        _identityUserAppService.update(user.Id, user).done(function () {
            $('#createUpdateUserModal').modal('hide');
            dataTable.ajax.reload();
        });
    });
});


function findAssignedRoleNames() {
    var assignedRoleNames = [];

    $(document).find('.user-role-checkbox-list input[type=checkbox]')
        .each(function () {
            if ($(this).is(':checked')) {
                assignedRoleNames.push($(this).attr('name'));
            }
        });

    return assignedRoleNames;
}

//TODO: move to common script file
$.fn.serializeFormToObject = function () {
    //serialize to array
    var data = $(this).serializeArray();

    //add also disabled items
    $(':disabled[name]', this)
        .each(function () {
            data.push({ name: this.name, value: $(this).val() });
        });

    //map to object
    var obj = {};
    data.map(function (x) { obj[x.name] = x.value; });

    return obj;
};

//TODO: move to common script file and also abp.localization is undefined
/************************************************************************
* Overrides default settings for datatables                             *
*************************************************************************/
(function ($) {
    if (!$.fn.dataTable) {
        return;
    }

    var currentLanguage = 'English'; //TODO: Get from current culture!

    //TODO: This does not work!
    //$.extend(true, $.fn.dataTable.defaults, {
    //    language: {
    //        url: '/modules/identity/libs/datatables/localizations/' + currentLanguage + '.json'
    //    },
    //    lengthMenu: [5, 10, 25, 50, 100, 250, 500],
    //    pageLength: 10,
    //    paging: true,
    //    serverSide: true,
    //    processing: true,
    //    responsive: true,
    //    pagingType: "bootstrap_full_number",
    //    dom: 'rt<"bottom"ilp><"clear">',
    //    order: []
    //});

})(jQuery);