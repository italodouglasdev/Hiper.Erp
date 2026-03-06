function MessageBoxAguarde(Titulo, Tempo) {

    var Alerta = Swal.mixin({});

    if (Tempo == null) {
        Alerta = Swal.mixin({
            title: 'Por favor, aguarde...',
            allowOutsideClick: false,
            showConfirmButton: false
        });
    }
    else {
        Alerta = Swal.mixin({
            title: 'Por favor, aguarde...',
            allowOutsideClick: false,
            showConfirmButton: false,
            timer: Tempo,
            timerProgressBar: true
        });
    }

    return Alerta;
};

function MessageBoxRedirecionar(Titulo, Texto, Url, Tempo) {

    var Alerta = Swal.mixin({});

    Alerta = Swal.mixin({
        title: Titulo,
        text: Texto,
        allowOutsideClick: false,
        showConfirmButton: false,
        timer: Tempo,
        timerProgressBar: true,
        willClose: () => {
            window.location.replace(Url);
        },
    });

    return Alerta;
};

function MessageBoxSuccess(Mensagem) {

    Alerta = Swal.mixin({
        icon: 'success',
        title: Mensagem,
        allowOutsideClick: false,
        showConfirmButton: true,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-default m-1"
        },
        confirmButtonText: '<i class="fas fa-long-arrow-alt-left"></i> Voltar'
    });

    return Alerta;
};

function MessageBoxError(Mensagem) {

    Alerta = Swal.mixin({
        icon: 'error',
        title: Mensagem,
        allowOutsideClick: false,
        showConfirmButton: true,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-default m-1"
        },
        confirmButtonText: '<i class="fas fa-long-arrow-alt-left"></i> Voltar'
    });

    return Alerta;
};

function MessageBoxSuccessToast(Titulo) {

    Alerta = Swal.mixin({
        icon: 'success',
        title: Titulo,
        toast: true,
        position: 'top',
        showConfirmButton: false,
        timer: 2000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    return Alerta;
};

function MessageBoxErrorToast(Titulo) {

    Alerta = Swal.mixin({
        icon: 'error',
        title: Titulo,
        toast: true,
        position: 'top',
        showConfirmButton: false,
        timer: 5000,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
        }
    });

    return Alerta;
};

function MessageBoxResultado(Sucesso, Mensagem) {

    if (Sucesso == "True" || Sucesso == "true" || Sucesso == true) {
        MessageBoxSuccess(Mensagem).fire();
    } else {
        MessageBoxError(Mensagem).fire();
    }
};

function MessageBoxToastResultado(Sucesso, Mensagem) {

    if (Sucesso == "True" || Sucesso == "true" || Sucesso == true) {
        MessageBoxSuccessToast(Mensagem).fire();
    } else {
        MessageBoxErrorToast(Mensagem).fire();
    }
};

function MessageBoxConfirmacao(Mensagem) {

    return Swal.fire({
        title: Mensagem,
        icon: "question",
        showCancelButton: true,
        confirmButtonText: '<i class="fas fa-check"></i> Sim',
        cancelButtonText: '<i class="fas fa-times"></i> Cancelar',
        reverseButtons: true,
        buttonsStyling: false,
        customClass: {
            confirmButton: "btn btn-default m-1",
            cancelButton: "btn btn-default m-1"
        }
    }).then((result) => {
        if (result.isConfirmed) {
            RemoverModalBackdrop();
            return true;
        } else if (result.isDenied) {
            RemoverModalBackdrop();
            return false;
        }
    });

}

