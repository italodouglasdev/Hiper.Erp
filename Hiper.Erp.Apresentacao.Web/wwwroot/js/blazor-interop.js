window.blazorInterop = {
    inicializarAdminLTE: function () {
        // Força a reinicialização dos componentes do AdminLTE que dependem do DOM
        if (typeof $.fn.ControlSidebar !== 'undefined') {
            $('[data-widget="treeview"]').each(function() {
                $(this).Treeview('init');
            });
            $('[data-widget="pushmenu"]').PushMenu();
        }
        
        // Remove o preloader se ele ainda estiver visível
        setTimeout(function () {
            $('.preloader').fadeOut();
        }, 500);
    },
    
    executarFuncaoGlobal: function (nomeFuncao, ...args) {
        if (typeof window[nomeFuncao] === 'function') {
            return window[nomeFuncao](...args);
        } else {
            console.error("Função não encontrada: " + nomeFuncao);
        }
    }
};
