var dataTable;

$(document).ready(() => loadDataTable());

function loadDataTable() {
    dataTable = $("#CompanyTable").DataTable({
        "ajax": { url: "/api/admin/company/getall" },
        "columns": [
            { data: "name" },
            { data: "cnpj" },
            { data: "address.streetAddress" },
            { data: "address.city" },
            { data: "address.state" },
            { data: "address.postalCode" },
            {
                data: "id",
                render: (data) => {
                    return `
                         <div class="w-75 btn-group" role="group">
                            <a class="btn btn-outline-warning mx-1"
                                href="/admin/company/upsert/?id=${data}"
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a class="btn btn-outline-danger mx-1"
                                onClick=DeleteConfirmation("${data}")
                               <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>
                    `
                }
            }
        ],
        "responsive": true
    })
}

function DeleteConfirmation(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: 'rgb(252, 57, 57)',
        cancelButtonColor: 'rgb(0, 156, 220)',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/api/admin/company/delete?id=${id}`,
                type: "DELETE",
                success: (data) => {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}