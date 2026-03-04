document.addEventListener('DOMContentLoaded', function () {
    const body = document.getElementById('servicesBody');
    const addBtn = document.getElementById('addRowBtn');
    const grandTotalEl = document.getElementById('grandTotal');

    if (!body || !addBtn) return;

    function buildRow(index) {
        const tr = document.createElement('tr');
        tr.className = 'service-row';
        tr.innerHTML = `
            <td>
                <input type="hidden" name="Input.Services[${index}].Id" value="" />
                <input type="text" name="Input.Services[${index}].ServiceName"
                       class="form-control service-name" required />
            </td>
            <td><input type="number" name="Input.Services[${index}].Quantity"
                       class="form-control service-qty" min="1" value="1" required /></td>
            <td><input type="number" name="Input.Services[${index}].UnitPrice"
                       class="form-control service-unit-price" min="0" step="0.01" value="0" required /></td>
            <td><input type="text" class="form-control service-row-total" readonly value="0.00" /></td>
            <td><button type="button" class="btn btn-sm btn-danger remove-row">✕</button></td>
        `;
        return tr;
    }

    function reindex() {
        const rows = body.querySelectorAll('.service-row');
        rows.forEach(function (row, i) {
            row.querySelector('input[name*=".Id"]').name = `Input.Services[${i}].Id`;
            row.querySelector('.service-name').name = `Input.Services[${i}].ServiceName`;
            row.querySelector('.service-qty').name = `Input.Services[${i}].Quantity`;
            row.querySelector('.service-unit-price').name = `Input.Services[${i}].UnitPrice`;
        });
    }

    function updateRowTotal(row) {
        const qty = parseFloat(row.querySelector('.service-qty').value) || 0;
        const price = parseFloat(row.querySelector('.service-unit-price').value) || 0;
        const total = qty * price;
        row.querySelector('.service-row-total').value = total.toFixed(2);
    }

    function updateGrandTotal() {
        let sum = 0;
        body.querySelectorAll('.service-row-total').forEach(function (el) {
            sum += parseFloat(el.value) || 0;
        });
        grandTotalEl.textContent = sum.toFixed(2);
    }

    // Initialize totals for existing rows
    body.querySelectorAll('.service-row').forEach(function (row) {
        updateRowTotal(row);
    });
    updateGrandTotal();

    // Add row
    addBtn.addEventListener('click', function () {
        const index = body.querySelectorAll('.service-row').length;
        const row = buildRow(index);
        body.appendChild(row);
        updateGrandTotal();
    });

    // Remove row + re-index
    body.addEventListener('click', function (e) {
        if (e.target.classList.contains('remove-row')) {
            const row = e.target.closest('.service-row');
            row.remove();
            reindex();
            updateGrandTotal();
        }
    });

    // Live calculation
    body.addEventListener('input', function (e) {
        if (e.target.classList.contains('service-qty') ||
            e.target.classList.contains('service-unit-price')) {
            const row = e.target.closest('.service-row');
            updateRowTotal(row);
            updateGrandTotal();
        }
    });
});
