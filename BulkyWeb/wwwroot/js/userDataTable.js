var dataTable;

$(document).ready(() => LoadDataTable());

function LoadDataTable() {
    dataTable = $("#UsersTable").DataTable({
        "ajax": { url: "/API/Admin/User/GetAll" },
        "columns": [
            { "data": "name" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "role" },
            { "data": "company.name" },
            {
                data: { id: "id", lockoutEnd: "lockoutEnd" },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {
                        return `
                            <div class="text-center">
                               <a onclick=LockUnlockAccount("${data.id}")  
                                    class="btn btn-success rounded role="button" style="width: 130px">
                                    <i class="bi bi-unlock-fill"></i> Unlock
                               </a>
                               <a href="/Admin/User/RoleManager/?userId=${data.id}"
                                    class="btn btn-warning rounded" role="button" style="width: 130px">
                                    <i class="bi bi-pencil-square"></i> Permissions
                                </a>
                            </div> 
                        `
                    } else {
                        return `
                            <div class="text-center">
                                  <a onclick=LockUnlockAccount("${data.id}")  
                                    class="btn btn-dark rounded role="button" style="width: 130px">
                                    <i class="bi bi-lock-fill"></i> Lock
                                </a>
                                <a href="/Admin/User/RoleManager/?userId=${data.id}"
                                    class="btn btn-warning rounded" role="button" style="width: 130px">
                                    <i class="bi bi-pencil-square"></i> Permissions
                                </a>
                            </div> 
                        `
                    }
                },
            }
        ],
        "responsive": true
    });
}

function LockUnlockAccount(id) {
    $.ajax({
        type: "POST",
        url: "/API/Admin/User/LockUnlockAccount",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            } else {
                toastr.error(data.message);
            }
        }
    })
}