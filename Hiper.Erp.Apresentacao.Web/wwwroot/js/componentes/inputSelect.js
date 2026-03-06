window.initSelect2 = (id, placeholder, multiple) => {

    const $select = $('#' + id);

    if ($select.hasClass("select2-hidden-accessible")) {
        $select.select2('destroy');
    }

    $select.select2({
        theme: 'bootstrap4',
        language: 'pt-BR',
        placeholder: placeholder,
        multiple: multiple,
        width: '100%'
    });
};

window.setSelect2Values = (id, values) => {
    const $select = $('#' + id);
    $select.val(values).trigger('change');
};

window.getSelect2Values = (id) => {
    const values = [];
    $('#' + id + ' option:selected').each(function () {
        values.push($(this).val());
    });
    return values;
};

window.bindSelect2Change = (id, dotnetRef) => {
    $('#' + id).on('change', function () {
        const values = $(this).val();
        dotnetRef.invokeMethodAsync('OnJsChange', values);
    });
};
