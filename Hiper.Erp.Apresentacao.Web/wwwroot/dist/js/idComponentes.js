

function Submeter(_type, _url, _data, _contentType) {

    return new Promise((resolve, reject) => {

        try {

            var msgAguarde = MessageBoxAguarde("Aguarde...").fire();

            $.ajax({
                type: _type,
                url: _url,
                data: _data,
                contentType: _contentType,
                processData: false,
                async: true,
                success: function (res) {
                    msgAguarde.close();
                    resolve({
                        sucesso: true,
                        mensagem: 'sucesso',
                        status: 200,
                        html: res
                    });
                },
                error: function (err) {

                    msgAguarde.close();

                    resolve({
                        sucesso: false,
                        mensagem: err.responseText,
                        status: err.status,
                        html: {
                            retorno: {
                                sucesso: false,
                                mensagem: err.responseText
                            }
                        }
                    });
                }
            });

        } catch (ex) {
            msgAguarde.close();

            resolve({
                sucesso: false,
                mensagem: ex.toString(),
                status: 400,
                html: {
                    retorno: {
                        sucesso: false,
                        mensagem: ex.toString()
                    }
                }
            });
        }

    });
}


function RemoverModalBackdrop() {
    const backdrop = document.querySelector('.modal-backdrop');
    if (backdrop) {
        backdrop.parentNode.removeChild(backdrop);
    }
};



//Componentes para LocalStorage
function LocalStorageObter(nome) {
    var objeto = localStorage.getItem(nome);
    return objeto ? JSON.parse(objeto) : null;
}
function LocalStoreageSalvar(nome, objeto) {
    localStorage.setItem(nome, JSON.stringify(objeto));
}
function LocalStoreageDeletar(nome) {
    localStorage.removeItem(nome);
}



// Componentes para Tabelas
function CarregarPartialView(idDiv, controller, action, codigo) {

    let url = "/" + controller + "/" + action + "?Codigo=" + codigo;

    Submeter('GET', url, null).then((retorno) => {
        if (retorno.sucesso == true) {
            $("#" + idDiv).html(retorno.html);
        }
        else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function CarregarTabela(idDiv, controller, action) {

    let url = "/" + controller + "/" + action;

    Submeter('GET', url, null, false).then((retorno) => {
        if (retorno.sucesso == true) {
            $("#" + idDiv).html(retorno.html);
        }
        else {
            MessageBoxToastSalvar(retorno.status, retorno.mensagem);
        }
    });
}
function CarregarTabelaNovaAba(idDiv, controller, action) {

    var url = "/Home/VerTabelaNovaAba?IdDivTabela=" + idDiv + "&ControllerTabela=" + controller + "&ActionTabela=" + action;

    Submeter('GET', url, null).then((retorno) => {
        if (retorno.sucesso == true) {
            var novaAba = window.open('', '_blank');
            novaAba.document.write(retorno.html);
            novaAba.document.close();
        } else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function CarregarGrid(idDiv, controller, action, nomeTabela) {

    let url = "/" + controller + "/" + action;

    var tabela = LocalStorageObter(nomeTabela);

    Submeter('POST', url, JSON.stringify(tabela), 'application/json').then((retorno) => {
        if (retorno.sucesso == true) {
            $("#" + idDiv).html(retorno.html);
        }
        else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function AtualizarPaginacao(nomeTabela, numeroPagina) {

    var tabela = LocalStorageObter(nomeTabela);

    tabela.paginaAtual = numeroPagina;

    LocalStoreageSalvar(nomeTabela, tabela);

    CarregarGrid(tabela.grid.idDiv, tabela.grid.controller, tabela.grid.action, nomeTabela);

}
function AtualizarOrdenacao(nomeTabela, selectComponente) {

    var tabela = LocalStorageObter(nomeTabela);

    tabela.campoOrdenacao = tabela.campos.find(x => x.descricao == selectComponente.value);

    LocalStoreageSalvar(nomeTabela, tabela);

}
function AtualizarOrganizacao(nomeTabela, selectComponente) {

    var tabela = LocalStorageObter(nomeTabela);
    tabela.tiposOrganizacao.forEach(function (item) {

        if (item.nome == selectComponente.value) {
            item.selecionado = true;
        }
        else {
            item.selecionado = false;
        }
    });

    LocalStoreageSalvar(nomeTabela, tabela);

}
function AtualizarItensPorPagina(nomeTabela, selectComponente) {

    var tabela = LocalStorageObter(nomeTabela);
    tabela.itensPorPagina.forEach(function (item) {

        if (item.quantidade == selectComponente.value) {
            item.selecionado = true;
        }
        else {
            item.selecionado = false;
        }
    });

    LocalStoreageSalvar(nomeTabela, tabela);

}
function AtualizarCampoExibir(nomeTabela, inputComponente) {

    var tabela = LocalStorageObter(nomeTabela);
    tabela.camposExibir.forEach(function (item) {

        if (item.nome == inputComponente.value.replace('customCheckbox-', '')) {
            item.selecionado = inputComponente.checked;
            return;
        }

    });
    LocalStoreageSalvar(nomeTabela, tabela);
}
function AtualizarFiltroCampo(nomeTabela, selectComponente) {

    var tabela = LocalStorageObter(nomeTabela);

    var codigoFiltro = $(selectComponente).attr("id").split('-')[2];

    var campo = tabela.campos.find(x => x.descricao == selectComponente.value);

    tabela.filtros.forEach(function (item) {

        if (item.codigo == codigoFiltro) {

            ResetarFiltroValor('filtros-' + nomeTabela + '-' + codigoFiltro + '-valor-input', campo.tipo);
            //Lembrar de limpar os controle de valor e desabilitar os controls de Tipo

            item.campo = campo;
            return;
        }


    });

    LocalStoreageSalvar(nomeTabela, tabela);

}
function AtualizarFiltroTipo(nomeTabela, selectComponente) {

    var tabela = LocalStorageObter(nomeTabela);

    var codigoFiltro = $(selectComponente).attr("id").split('-')[2];

    var tipo = tabela.tiposFiltro.find(x => x.nome == selectComponente.value);

    tabela.filtros.forEach(function (item) {

        if (item.codigo == codigoFiltro) {

            //Lembrar de limpar os controle de valor

            item.tipo = tipo;
            return;
        }

    });

    LocalStoreageSalvar(nomeTabela, tabela);
}
function AtualizarFiltroValor(nomeTabela, inputComponente) {

    var tabela = LocalStorageObter(nomeTabela);

    var codigoFiltro = $(inputComponente).attr("id").split('-')[2];

    tabela.filtros.forEach(function (item) {

        if (item.codigo == codigoFiltro) {

            item.valor = inputComponente.value;
            return;
        }

    });

    LocalStoreageSalvar(nomeTabela, tabela);
}
function ResetarFiltroValor(campoId, tipo) {

    var campo = $('#' + campoId);

    if (campo.length > 0) {
        campo.val('');
    }

    switch (tipo) {
        case 'string':
            campo.attr('type', 'text');
            break;
        case 'int':
            campo.attr('type', 'number');
            campo.attr('min', '0');
            campo.attr('step', 'any');
            break;
        case 'datetime':
            campo.attr('type', 'date');
            var dataAtual = new Date().toISOString().split('T')[0];
            campo.val(dataAtual);
            campo.attr('max', '9999-12-31');
            campo.attr('min', '0001-01-01');
            break;
        case 'bool':
            campo.attr('type', 'checkbox');
            break;
        default:
            campo.attr('type', 'text');
            break;
    }

}
function AtualizarFiltroCampoBuscar(nomeTabela, inputComponente) {

    var tabela = LocalStorageObter(nomeTabela);

    tabela.campoBuscar = inputComponente.value;

    LocalStoreageSalvar(nomeTabela, tabela);

}
function CriarControleOrdenacao(idDiv) {

    var nomeTabela = idDiv.split('-')[1];

    var tabela = LocalStorageObter(nomeTabela);

    var divFormGroup = $('<div>').addClass('form-group');
    var label = $('<label>').text('Ordenação');
    var select = $('<select>')
        .attr('id', idDiv + '-select')
        .addClass('form-control select2')
        .css('width', '100%')
        .on('change', function () {
            AtualizarOrdenacao(nomeTabela, this);
        });

    tabela.campos.forEach(function (item) {
        var option = $('<option>').text(item.descricao);

        if (item.selecionado == true) {
            option.attr('selected', 'selected');
        }

        select.append(option);
    });

    divFormGroup.append(label).append(select);

    $('#' + idDiv).append(divFormGroup);

    $('#' + idDiv + '-select').select2({ theme: 'bootstrap4' });
}
function CriarControleOrganizacao(idDiv) {

    var nomeTabela = idDiv.split('-')[1];

    var tabela = LocalStorageObter(nomeTabela);

    var divFormGroup = $('<div>').addClass('form-group');
    var label = $('<label>').text('Organização');
    var select = $('<select>')
        .attr('id', idDiv + '-select')
        .addClass('form-control select2')
        .css('width', '100%')
        .on('change', function () {
            AtualizarOrganizacao(nomeTabela, this);
        });

    tabela.tiposOrganizacao.forEach(function (item) {
        var option = $('<option>').text(item.nome);

        if (item.selecionado == true) {
            option.attr('selected', 'selected');
        }

        select.append(option);
    });

    divFormGroup.append(label).append(select);

    $('#' + idDiv).append(divFormGroup);

    $('#' + idDiv + '-select').select2({ theme: 'bootstrap4' });
}
function CriarControleItensPorPagina(idDiv) {

    var nomeTabela = idDiv.split('-')[1];

    var tabela = LocalStorageObter(nomeTabela);

    var divFormGroup = $('<div>').addClass('form-group');
    var label = $('<label>').text('Itens por Página');
    var select = $('<select>')
        .attr('id', idDiv + '-select')
        .addClass('form-control select2')
        .css('width', '100%')
        .on('change', function () {
            AtualizarItensPorPagina(nomeTabela, this);
        });

    tabela.itensPorPagina.forEach(function (item) {

        var option = $('<option>').text(item.quantidade);

        if (item.selecionado == true) {
            option.attr('selected', 'selected');
        }

        select.append(option);

    });

    divFormGroup.append(label).append(select);

    $('#' + idDiv).append(divFormGroup);

    $('#' + idDiv + '-select').select2({ theme: 'bootstrap4' });
}
function CriarControleCamposExibir(idDiv) {

    var nomeTabela = idDiv.split('-')[1];

    var tabela = LocalStorageObter(nomeTabela);

    var divFormGroup = $('<div>').addClass('form-group clearfix');
    tabela.camposExibir.forEach(function (campo) {
        var divCheckbox = $('<div>').addClass('custom-control custom-checkbox m-1');

        var inputCheckbox = $('<input>')
            .addClass('custom-control-input')
            .attr('type', 'checkbox')
            .attr('id', 'customCheckbox-' + campo.nome)
            .attr('value', 'customCheckbox-' + campo.nome)
            .prop('checked', campo.selecionado) // Marca o checkbox se campo.selecionado for true
            .on('click', function () {
                AtualizarCampoExibir(nomeTabela, this);
            });

        var labelCheckbox = $('<label>')
            .addClass('custom-control-label')
            .attr('for', 'customCheckbox-' + campo.nome)
            .text(campo.descricao);

        divCheckbox.append(inputCheckbox).append(labelCheckbox);
        divFormGroup.append(divCheckbox);
    });

    $('#' + idDiv).append(divFormGroup);
}
function CriarControleFiltroCampo(idDiv) {
    var nomeTabela = idDiv.split('-')[1];
    var tabela = LocalStorageObter(nomeTabela);

    // Primeiro col-4: Campo
    var divColCampo = $('<div>')
        .addClass('col-12 col-md-4 d-flex align-items-center')
        .attr('id', idDiv + '-campo');

    var divFormGroupCampo = $('<div>').addClass('form-group flex-grow-1');
    var label = $('<label>').text('Campo');
    var select = $('<select>')
        .attr('id', idDiv + '-campo-select')
        .addClass('form-control select2')
        .css('width', '100%')
        .on('change', function () {
            AtualizarFiltroCampo(nomeTabela, this);
        });

    tabela.campos.forEach(function (campo) {
        var option = $('<option>').text(campo.descricao);
        select.append(option);
    });

    divFormGroupCampo.append(label).append(select);

    // Criar o botão com o ícone
    var button = $('<button>')
        .addClass('btn btn-default mt-3 mr-2') // Adiciona margem à esquerda
        .html('<i class="fas fa-times"></i>') // Adiciona o ícone
        .on('click', function () {
            RemoverControleFiltro(idDiv);
        });

    // Criar uma div para o botão e select
    var divButtonSelect = $('<div>').addClass('d-flex align-items-center w-100');
    divButtonSelect.append(button).append(divFormGroupCampo);

    divColCampo.append(divButtonSelect);

    // Adiciona a div row à div principal
    $('#' + idDiv).append(divColCampo);

    $('#' + idDiv + '-campo-select').select2({ theme: 'bootstrap4' });
}
function RemoverControleFiltro(idDivCampo) {

    var nomeTabela = idDivCampo.split('-')[1];
    var codigoFiltro = idDivCampo.split('-')[2];

    var tabela = LocalStorageObter(nomeTabela);


    var indiceRemover = -1;
    tabela.filtros.forEach(function (item, index) {
        if (item.codigo == codigoFiltro) {
            indiceRemover = index;
        }
    });

    if (indiceRemover !== -1) {
        tabela.filtros.splice(indiceRemover, 1);
    }

    LocalStoreageSalvar(nomeTabela, tabela);

    $('#' + idDivCampo).remove();
}
function CriarControleFiltroTipo(idDiv) {

    var nomeTabela = idDiv.split('-')[1];

    var tabela = LocalStorageObter(nomeTabela);

    // Primeiro col-4: Campo
    var divColTipo = $('<div>')
        .addClass('col-12 col-md-4')
        .attr('id', idDiv + '-tipo')

    var divFormGroupCampo = $('<div>').addClass('form-group');
    var label = $('<label>').text('Tipo');
    var select = $('<select>')
        .attr('id', idDiv + '-tipo-select')
        .addClass('form-control select2')
        .css('width', '100%').css('width', '100%').on('change', function () {
            AtualizarFiltroTipo(nomeTabela, this);
        });

    tabela.tiposFiltro.forEach(function (item) {
        var option = $('<option>').text(item.nome);

        if (item.selecionado == true) {
            option.attr('selected', 'selected');
        }

        select.append(option);
    });

    divFormGroupCampo.append(label).append(select);
    divColTipo.append(divFormGroupCampo);


    // Adiciona a div row à div principal
    $('#' + idDiv).append(divColTipo);

    $('#' + idDiv + '-tipo-select').select2({ theme: 'bootstrap4' });
}
function CriarControleFiltroValor(idDiv) {

    var nomeTabela = idDiv.split('-')[1];

    var tabela = LocalStorageObter(nomeTabela);

    // Terceiro col-4: Valor
    var divColValor = $('<div>')
        .addClass('col-12 col-md-4')
        .attr('id', idDiv + '-valor');

    var divFormGroupValor = $('<div>').addClass('form-group');
    var labelValor = $('<label>').attr('for', 'filtro-valor').text('Valor');
    var inputValor = $('<input>')
        .attr('type', 'text')
        .addClass('form-control')
        .attr('id', idDiv + '-valor-input')
        .on('blur', function () {
            AtualizarFiltroValor(nomeTabela, this);
        });



    divFormGroupValor.append(labelValor).append(inputValor);
    divColValor.append(divFormGroupValor);

    $('#' + idDiv).append(divColValor);

    ResetarFiltroValor(idDiv + '-valor-input', 'int');

}
function CriarControleFiltro(idDiv) {

    var nomeTabela = idDiv.split('-')[1];

    var codigoFiltro = ObtenhaCodigoProximoFiltro(idDiv);

    var filtro = CriarObjetoFiltroCampo(nomeTabela, codigoFiltro);

    var divRow = $('<div>')
        .addClass('row')
        .attr('id', idDiv + '-' + filtro.codigo);
    $('#' + idDiv).append(divRow);

    CriarControleFiltroCampo(divRow.attr("id"));
    CriarControleFiltroTipo(divRow.attr("id"));
    CriarControleFiltroValor(divRow.attr("id"));

}
function ObtenhaCodigoProximoFiltro(idDiv) {

    var nomeTabela = idDiv.split('-')[1];

    var tabela = LocalStorageObter(nomeTabela);

    var ultimoCodigo = 0;

    if (tabela && tabela.filtros && tabela.filtros.length > 0) {
        maxIdade = Math.max.apply(Math, $.map(tabela.filtros, function (filtro) {
            ultimoCodigo = filtro.codigo;
        }));
    }

    return ultimoCodigo + 1;
}
function CriarObjetoFiltroCampo(nomeTabela, codigoFiltro) {

    var tabela = LocalStorageObter(nomeTabela);

    var primeiroCampo = tabela.campos.length > 0 ? tabela.campos[0] : null;
    var primeiroTipo = tabela.tiposFiltro.length > 0 ? tabela.tiposFiltro[0] : null;

    var filtro = {
        "codigo": codigoFiltro,
        "campo": primeiroCampo,
        "tipo": primeiroTipo,
        "valor": ''
    };

    tabela.filtros.push(filtro);

    LocalStoreageSalvar(nomeTabela, tabela);

    return filtro;
}



//Componentes para Formulário
function CarregarModalFormulario(controller, actionTabela, codigo) {

    var nomeModal = actionTabela.replace('Tabela', '')
    var action = nomeModal + 'Ver';

    if ($("#modal-" + nomeModal).length > 0) {

        $("#modal-" + nomeModal).remove();
    };

    let url = "/" + controller + "/" + action + "?Codigo=" + codigo;

    Submeter('GET', url, null).then((retorno) => {
        if (retorno.sucesso == true) {
            $("#div-principal").append(retorno.html);
            $("#modal-" + nomeModal).modal('show');
        }
        else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
};


function CarregarModalFuncoes(controller, actionTabela, codigo) {

    var nomeModal = actionTabela.replace('Tabela', '')
    var action = nomeModal + 'Funcoes';

    if ($("#modal-" + nomeModal).length > 0) {

        $("#modal-" + nomeModal).remove();
    };

    let url = "/" + controller + "/" + action + "?Codigo=" + codigo;

    Submeter('GET', url, null).then((retorno) => {
        if (retorno.sucesso == true) {
            $("#div-principal").append(retorno.html);
            $("#modal-" + nomeModal).modal('show');
        }
        else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
};
function InserirFormulario(idForm, controller, action) {

    var Mensagem = "Confirmar novo cadastro?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/" + controller + "/" + action;

            Submeter('POST', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    var tabela = idForm.split('-')[2];
                    CarregarGrid('tabela-grid-' + tabela, controller, tabela + 'Grid', tabela);
                    $('#modal-' + tabela).modal('hide');

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });

};
function AtualizarFormulario(idForm, controller, action) {

    var Mensagem = "Atualizar cadastro?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/" + controller + "/" + action;

            Submeter('PUT', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    var tabela = idForm.split('-')[2];
                    CarregarGrid('tabela-grid-' + tabela, controller, tabela + 'Grid', tabela);
                    $('#modal-' + tabela).modal('hide');

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });


};
function ExcluirFormulario(idForm, controller, action) {

    var Mensagem = "Excluir cadastro?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/" + controller + "/" + action;

            Submeter('DELETE', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    var tabela = idForm.split('-')[2];
                    CarregarGrid('tabela-grid-' + tabela, controller, tabela + 'Grid', tabela);
                    $('#modal-' + tabela).modal('hide');

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });
};
function CancelarFormulario(idModal) {
    var Mensagem = "Cancelar operação?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {
        if (resposta === true) {

            var $modal = $('#' + idModal);

            $modal.modal('hide');

            $modal.on('hidden.bs.modal', function () {
                $modal.remove();
            });

        }
    });
};




//Relatórios
function EmitirRelatorio(idForm, nomeModal) {

    var Mensagem = "Emitir Relatório?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        var action = nomeModal + 'Emitir';

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/Relatorio/" + action;

            Submeter('POST', url, form, false).then((model) => {

                if (model.sucesso == true) {
                    RelatorioBaixar(model.html.nomeArquivo, model.html.contentType, model.html.base64);
                } else {
                    MessageBoxError(model.status, model.mensagem);
                }

            });

        }

    });


};
function EmitirRelatorioGenerico(tipo, action, nomeTabela) {

    var Mensagem = "Emitir Relatório?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var url = "/Relatorio/" + action + "Generico?Tipo=" + tipo;

            var tabela = LocalStorageObter(nomeTabela);

            Submeter('POST', url, JSON.stringify(tabela), 'application/json').then((model) => {
                if (model.sucesso == true) {

                    RelatorioBaixar(model.html.nomeArquivo, model.html.contentType, model.html.base64);

                } else {
                    MessageBoxError(model.status, model.mensagem);
                }
            });

        }

    });
}
function RelatorioBaixar(nome, contentType, base64) {
    const link = document.createElement('a');
    link.href = 'data:' + contentType + ';base64,' + base64;
    link.download = nome;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}


//Componentes Select2
function AplicarSelect2Simples(idForm, idSelect) {
    var multiple = $('#' + idSelect).prop('multiple');

    $('#' + idForm + ' #' + idSelect).select2({
        theme: 'bootstrap4',
        multiple: multiple,
        language: "pt-BR",
        placeholder: 'Digite para buscar'
    });
};

function AplicarSelect2(idForm, idSelect, controller, action) {

    var multiple = $('#' + idSelect).prop('multiple');

    $('#' + idForm + ' #' + idSelect).select2({
        theme: 'bootstrap4',
        minimumInputLength: 3,
        language: "pt-BR",
        multiple: multiple,
        ajax: {
            url: '/' + controller + '/' + action,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    q: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data.items
                };
            },
            cache: true
        },
        placeholder: 'Digite para buscar',
        allowClear: true
    });
};

function SetarItemSelect2(idForm, idSelect, item) {
    var $select = $('#' + idForm + ' #' + idSelect);

    // Limpa todos os itens
    $select.empty();

    // Cria a nova opção
    var option = new Option(item.text, item.id, true, true);

    // Adiciona ao select
    $select.append(option).trigger('change');
}

//Componentes Gerais
function CopiarValorDoCampo(formId, inputId) {
    // Selecionar o input dentro do formulário
    var $input = $('#' + formId).find('#' + inputId);

    // Selecionar o texto do input
    $input.select();

    // Copiar o texto para a área de transferência
    try {
        var sucesso = document.execCommand('copy');
        if (sucesso) {
            MessageBoxToastSalvar(true, 'Texto copiado com sucesso!');
        } else {
            MessageBoxToastSalvar(false, 'Falha ao copiar o texto!');
        }
    } catch (err) {
        MessageBoxToastSalvar(false, 'Falha ao copiar o texto! Detalhes: ' + err);
    }
    // Desselecionar o texto
    $input.blur();
}
function ExibirOuOcultarValorCampo(idInput) {
    var input = $('#' + idInput);

    if (input.attr('type') === 'password') {
        input.attr('type', 'text');
    } else {
        input.attr('type', 'password');
    }
};
function VerificarSeElementoExiste(idElemento) {
    return $('#' + idElemento).length > 0;
}

//Componentes Auth
function FazerLogin(idForm, controller, action) {

    var form = new FormData($("form[id='" + idForm + "']")[0]);

    let url = "/" + controller + "/" + action;

    Submeter('POST', url, form, false).then((model) => {
        if (model.html.retorno.sucesso == true) {
            MessageBoxRedirecionar('Redirecionando...', model.html.retorno.mensagem, model.html.retorno.url, 3000).fire();
        }
        else {
            MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
        }
    });

};
function FazerLogout(controller, action) {

    var Mensagem = "Sair?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            let url = "/" + controller + "/" + action;

            Submeter('GET', url, null, false).then((model) => {
                if (model.html.retorno.sucesso == true) {
                    MessageBoxRedirecionar('Redirecionando...', model.html.retorno.mensagem, model.html.retorno.url, 2000).fire();
                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });
        }
    });
};



function AplicarMascaraDecimal(id, casasDecimais) {

    if (casasDecimais == null)
        casasDecimais = 2;

    const zeros = '0'.repeat(casasDecimais);
    const mascara = '#.##0,' + zeros;

    $('#' + id).mask(mascara, { reverse: true });
}
