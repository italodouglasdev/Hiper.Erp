
//Agentes
function AgentesValidacaoInicialCnpjCpf(idForm, controller, action) {

    var form = new FormData($("form[id='" + idForm + "']")[0]);

    let url = "/" + controller + "/" + action;

    Submeter('POST', url, form, false).then((model) => {
        if (model.html.retorno.sucesso == false) {

            if (model.html.retorno.acao == "CnpjCpfExiste") {

                MessageBoxConfirmacao(model.html.retorno.mensagem).then((resposta) => {

                    if (resposta == true) {
                        CarregarModalFormulario(controller, 'Agentes', model.html.codigo);
                    }
                    else {
                        CarregarModalFormulario(controller, 'Agentes', 0);
                    }

                });

            }
            else if (model.html.retorno.acao == "CnpjCpfInvalido") {
                MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
            }


        }
    });

};

function AgentesAtualizarFoto() {
    $('#FotoBase64').click();

    $('#FotoBase64').on('change', function (event) {
        var file = event.target.files[0];
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#usuario-foto').attr('src', e.target.result);
        };

        reader.readAsDataURL(file);
    });
};
function AgentesConsultarEnderecoCEP(idForm) {

    var form = new FormData($("form[id='" + idForm + "']")[0]);

    let url = "/Cadastro/AgentesConsultarEnderecoCEP";

    Submeter('POST', url, form, false).then((model) => {
        if (model.html.retorno.sucesso == true) {

            MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

            $("#" + idForm + " #CEP").val(model.html.cep);
            $("#" + idForm + " #Logradouro").val(model.html.logradouro);
            $("#" + idForm + " #Bairro").val(model.html.bairro);

            SetarItemSelect2(idForm, 'CodigoCidade',
                {
                    id: model.html.cidade.codigo,
                    text: model.html.cidade.ibge + " |" + model.html.cidade.nome + " | " + model.html.cidade.uf
                });
        }
        else {
            MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
        }
    });
};
function AgentesConsultarEnderecoLogradouro(idForm) {

    var form = new FormData($("form[id='" + idForm + "']")[0]);

    let url = "/Cadastro/AgentesConsultarEnderecoLogradouro";

    Submeter('POST', url, form, false).then((retorno) => {
        if (retorno.sucesso == true) {
            $("#div-principal").append(retorno.html);
            $("#modal-AgentesEnderecosBusca").modal('show');
        }
        else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });

};
function AgentesConsultarEnderecoLogradouroSelecionar(cep, logradouro, bairro, cidade, uf) {


    var Mensagem = "Confirmar Endereço?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var idForm = 'modal-form-Agentes';

            $("#" + idForm + " #CEP").val(cep);
            $("#" + idForm + " #Logradouro").val(logradouro);
            $("#" + idForm + " #Cidade").val(cidade);
            $("#" + idForm + " #Bairro").val(bairro);
            $("#" + idForm + " #UF").val(uf);

            $("#modal-AgentesEnderecosBusca").modal('hide');

        }

    });

};
function AgentesCartelasTabVer(idDiv, codigoAgente) {
    let url = "/Cadastro/AgentesCartelasTabVer?CodigoAgente=" + codigoAgente;

    Submeter('GET', url, null, false).then((retorno) => {
        if (retorno.sucesso == true) {
            if ($("#" + idDiv).length) {
                $("#" + idDiv).html(retorno.html);
            }
        } else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function AgentesCartelasTabInserir(idForm) {

    var Mensagem = "Confirmar novo cadastro?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/Cadastro/AgentesCartelasTabInserir";

            Submeter('POST', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {
                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                    AgentesCartelasTabVer('model-tabs-Agentes-cartelas', model.html.codigoCliente);
                    CartelasParcelasTabVer('model-tabs-Cartelas-parcelas', model.html.codigoCartela);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });

};
function AgentesVinculosTabVer(idDiv, codigoRepresentante) {
    let url = "/Cadastro/AgentesVinculosTabVer?CodigoRepresentante=" + codigoRepresentante;

    Submeter('GET', url, null, false).then((retorno) => {
        if (retorno.sucesso == true) {
            if ($("#" + idDiv).length) {
                $("#" + idDiv).html(retorno.html);
            }
        } else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function AgentesVinculosAdicionar(idForm) {

    var Mensagem = "Vincular Vendedor?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/Cadastro/AgentesVinculosAdicionar";

            Submeter('POST', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    AgentesVinculosTabVer('model-tabs-Agentes-vinculos', model.html.codigoRepresentanteVinculado);

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });

};
function AgentesVinculosRemover(codigoRepresentante, codigoVendedor) {

    var Mensagem = "Desvincular Vendedor?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            let url = "/Cadastro/AgentesVinculosRemover?CodigoRepresentante=" + codigoRepresentante + "&CodigoVendedor=" + codigoVendedor;

            Submeter('POST', url, null, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    AgentesVinculosTabVer('model-tabs-Agentes-vinculos', model.html.codigoRepresentanteVinculado);

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });

};
function AgentesAtualizarCidadePorIBGE(controller, action) {

    var Mensagem = "Atualizar a cidade com base no IBGE?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            let url = "/" + controller + "/" + action;

            Submeter('PUT', url, null, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });


};
function AtualizarCadastroCidadesIBGE() {

    let url = "/Cadastro/AtualizarCadastroCidadesIBGE";

    MessageBoxConfirmacao("Atualizar tabela de cidades com base nos dados do IBGE?").then((resposta) => {

        if (resposta == true) {

            Submeter('POST', url, null, false).then((model) => {
                CarregarGrid('tabela-grid-Cidades', 'Cadastro', 'CidadesGrid', 'Cidades');
                MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);
            });
        }
    });
};
function AtualizarCadastroCEPs() {

    let url = "/Cadastro/AtualizarCadastroCEPs";

    MessageBoxConfirmacao("Atualizar tabela de CEPs com base nos dados dos Correios?").then((resposta) => {

        if (resposta == true) {

            Submeter('POST', url, null, false).then((model) => {
                CarregarGrid('tabela-grid-CEPs', 'Cadastro', 'CEPsGrid', 'CEPs');
                MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);
            });
        }
    });
};


function PremiacoesGanhadoresTabVer(idDiv, codigoPremiacao) {

    let url = "/Cadastro/PremiacoesGanhadoresTabVer?CodigoPremiacao=" + codigoPremiacao;

    Submeter('GET', url, null, false).then((retorno) => {
        if (retorno.sucesso == true) {
            $("#" + idDiv).html(retorno.html);
        }
        else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function FormasPagamentosBoletoEspacoTrabalhoIdGerar(idForm) {

    var Mensagem = "Gerar um novo Id?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/Cadastro/FormasPagamentosBoletoEspacoTrabalhoIdGerar";

            Submeter('PUT', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {
                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                    $("#" + idForm + " #BoletoEspacoTrabalhoId").val(model.html.boletoEspacoTrabalhoId);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });

};
function FormaPagamentoObtenhaPorSorteio(idForm, idCampo, codigoSorteio) {

    var form = new FormData($("form[id='" + idForm + "']")[0]);

    let url = "/Cadastro/FormaPagamentoObtenhaPorSorteio?CodigoSorteio=" + codigoSorteio;

    Submeter('POST', url, form, false).then((model) => {
        if (model.sucesso == true) {

            SetarItemSelect2(idForm, idCampo, {
                id: model.html.id,
                text: model.html.text === null ? "" : model.html.text
            });
        }
        else {
            MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
        }
    });
};
function BoletosGerarPorCodigoContaReceber(codigoContaReceber) {

    var Mensagem = "Emitir o Boleto?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            let url = "/Cadastro/BoletosEmitirPorCodigoContaReceber?CodigoContaReceber=" + codigoContaReceber;

            Submeter('POST', url, null, false).then((model) => {
                if (model.html.retorno.sucesso == true) {
                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                    Mensagem = "Abrir o Boleto Emitido?";

                    MessageBoxConfirmacao(Mensagem).then((resposta) => {

                        if (resposta == true) {

                            CarregarModalFormulario('Cadastro', 'Boletos', model.html.codigo);
                        }

                    });

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });

};


//Contas a Pagar e Receber
function ContasPagamento(idForm, controller, action) {

    var Mensagem = "Confirmar Pagamento?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/" + controller + "/" + action;

            Submeter('PUT', url, form, false).then((model) => {

                if (model.html.retorno.sucesso == true) {

                    var tabela = idForm.split('-')[2];

                    if (ElementoExiste('tabela-grid-' + tabela)) {
                        CarregarGrid('tabela-grid-' + tabela, controller, tabela + 'Grid', tabela);
                    }

                    $('#modal-' + tabela).modal('hide');
                    AgentesCartelasTabVer('model-tabs-Agentes-cartelas', model.html.codigoAgente);
                    CartelasParcelasTabVer('model-tabs-Cartelas-parcelas', model.html.codigoCartela);

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }

            });

        }

    });
};
function ContasPagamentoComissao(idForm, controller, action) {

    var Mensagem = "O Pagamento será recebido como Comissão do vendedor. Confirmar Pagamento?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/" + controller + "/" + action;

            Submeter('PUT', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    var tabela = idForm.split('-')[2];
                    CarregarGrid('tabela-grid-' + tabela, controller, tabela + 'Grid', tabela);
                    $('#modal-' + tabela).modal('hide');

                    AgentesCartelasTabVer('model-tabs-Agentes-cartelas', model.html.codigoAgente);
                    CartelasParcelasTabVer('model-tabs-Cartelas-parcelas', model.html.codigoCartela);

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });
};


//Caixas
function CaixasEstornar(idForm, controller, action) {

    var Mensagem = "Confirmar Estorno?";

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


//Cartelas

function CartelasParcelasTabVer(idDiv, codigoCartela) {

    let url = "/Cadastro/CartelasParcelasTabVer?CodigoCartela=" + codigoCartela;

    Submeter('GET', url, null, false).then((retorno) => {
        if (retorno.sucesso == true) {
            if ($("#" + idDiv).length) {
                $("#" + idDiv).html(retorno.html);
            }
        } else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function CartelasResgatesTabVer(idDiv, codigoCartela) {

    let url = "/Cadastro/CartelasResgatesTabVer?CodigoCartela=" + codigoCartela;

    Submeter('GET', url, null, false).then((retorno) => {
        if (retorno.sucesso == true) {
            $("#" + idDiv).html(retorno.html);
        }
        else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function CartelaCancelar(codigoCartela, codigoContaReceber) {

    var Mensagem = "Todas a Contas a Receber e Boletos com status diferente de Pago, serão canceladas. Confirmar Cancelamento do Cartela? ";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            let url = "/Cadastro/CartelasCancelar?CodigoCartela=" + codigoCartela + "&CodigoContaReceber=" + codigoContaReceber;

            Submeter('PUT', url, null, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    var tabela = "Cartelas";

                    CarregarGrid('tabela-grid-' + tabela, 'Cadastro', tabela + 'Grid', tabela);

                    $('#modal-' + tabela).modal('hide');

                    AgentesCartelasTabVer('model-tabs-Agentes-cartelas', model.html.codigoCliente);
                    CartelasParcelasTabVer('model-tabs-Cartelas-parcelas', model.html.codigoCartela);

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });

};
function CartelaReativar(idForm) {

    var Mensagem = "Todas a Contas a Receber e Boletos com status diferente de Pago, serão cancelados. Confirmar Cancelamento do Cartela? ";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/Cadastro/CartelasReativar";

            Submeter('PUT', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    var tabela = idForm.split('-')[2];
                    CarregarGrid('tabela-grid-' + tabela, 'Cadastro', tabela + 'Grid', tabela);
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
function CartelasRenegociar(codigoCartela, codigoContaReceber) {

    var controller = "Cadastro";
    var actionTabela = "CartelasRenegociar";
    var nomeModal = actionTabela.replace('Tabela', '')
    var action = nomeModal + 'Ver';

    if ($("#modal-" + nomeModal).length > 0) {

        $("#modal-" + nomeModal).remove();
    };

    let url = "/" + controller + "/" + action + "?CodigoCartela=" + codigoCartela + "&CodigoContaReceber=" + codigoContaReceber;

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
function CartelasRenegociarAtualizar(idForm) {

    var Mensagem = "Atualizar cadastro?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/Cadastro/CartelasRenegociarAtualizar";

            Submeter('PUT', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    var tabela = 'Cartelas';

                    CarregarGrid('tabela-grid-' + tabela, 'Cadastro', tabela + 'Grid', tabela);

                    $('#modal-' + tabela).modal('hide');

                    $('#modal-CartelasRenegociar').modal('hide');

                    AgentesCartelasTabVer('model-tabs-Agentes-cartelas', model.html.codigoCliente);

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });


};

function CartelaRestaurarNoBanco(codigoCliente) {

    var Mensagem = "Restaurar cartelas com os bancos? ";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            let url = "/Cadastro/InserirPendentesPorCliente?CodigoCliente=" + codigoCliente;

            Submeter('PUT', url, null, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    var tabela = "Cartelas";

                    CarregarGrid('tabela-grid-' + tabela, 'Cadastro', tabela + 'Grid', tabela);

                    $('#modal-' + tabela).modal('hide');

                    AgentesCartelasTabVer('model-tabs-Agentes-cartelas', model.html.codigoCliente);
                    CartelasParcelasTabVer('model-tabs-Cartelas-parcelas', model.html.codigoCartela);

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });

};


//Controle para envio de Fotos da Documentação
function CartelasResgatesDocumentoCameraVer(codigoCartela, tipoDocumento) {

    var nomeModal = 'CartelasResgatesDocumentoCamera';

    let url = "/Cadastro/CartelasResgatesDocumentoCameraVer?CodigoCartela=" + codigoCartela + "&TipoDocumento=" + tipoDocumento;

    Submeter('GET', url, null).then((retorno) => {
        if (retorno.sucesso === true) {
            $("#div-principal").append(retorno.html);
            $("#modal-" + nomeModal).modal('show');

            const video = document.getElementById('modal-' + nomeModal + '-video');

            // Solicita acesso à câmera
            navigator.mediaDevices.getUserMedia({ video: true })
                .then(function (stream) {
                    video.srcObject = stream;
                })
                .catch(function (err) {
                    MessageBoxError(false, err);
                });
        } else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function CartelasResgatesDocumentoCameraAtualizar(idForm) {

    var Mensagem = "Enviar Documento?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        var nomeModal = 'CartelasResgatesDocumentoCamera';

        var form = new FormData($("form[id='" + idForm + "']")[0]);

        if (resposta === true) {
            const video = document.getElementById('modal-' + nomeModal + '-video');
            const canvas = document.createElement('canvas');
            const context = canvas.getContext('2d');

            if (video && video.videoWidth && video.videoHeight) {
                canvas.width = video.videoWidth;
                canvas.height = video.videoHeight;
                context.drawImage(video, 0, 0, canvas.width, canvas.height);
                const dataURL = canvas.toDataURL('image/png');

                form.append('ImgBase64', dataURL);

                let url = "/Cadastro/CartelasResgatesDocumentoCameraAtualizar";

                Submeter('PUT', url, form, false).then((model) => {
                    if (model.html.retorno.sucesso === true) {
                        CartelasResgatesTabVer('model-tabs-Cartelas-resgate', model.html.codigoCartela);
                        MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                    } else {

                        MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                    }
                });

                FecharModal('modal-' + nomeModal);

            } else {

                MessageBoxError(false, "Erro ao capturar a imagem. Verifique se a câmera está funcionando corretamente.");

            }
        }
    });
}
function CartelasResgatesDocumentoArrastarSoltarVer(codigoCartela, tipoDocumento) {

    var nomeModal = 'CartelasResgatesDocumentoArrastarSoltar';

    if ($("#modal-" + nomeModal).length > 0) {
        $("#modal-" + nomeModal).remove();
    }

    let url = "/Cadastro/CartelasResgatesDocumentoArrastarSoltarVer?CodigoCartela=" + codigoCartela + "&TipoDocumento=" + tipoDocumento;

    Submeter('GET', url, null).then((retorno) => {
        if (retorno.sucesso === true) {

            $("#div-principal").append(retorno.html);
            $("#modal-" + nomeModal).modal('show');

            var selectedFile;

            $("#drop-zone").on("dragover", function (e) {
                e.preventDefault();
                e.stopPropagation();
                $(this).addClass("drag-over");
            });

            $("#drop-zone").on("dragleave", function (e) {
                e.preventDefault();
                e.stopPropagation();
                $(this).removeClass("drag-over");
            });

            $("#drop-zone").on("drop", function (e) {
                e.preventDefault();
                e.stopPropagation();
                $(this).removeClass("drag-over");
                selectedFile = e.originalEvent.dataTransfer.files[0];
                showPreview(selectedFile);
                addFileToForm(selectedFile);
            });

            $("#drop-zone").on("click", function () {
                $("#CartelasResgatesDocumentoArrastarSoltar-Input-Imagem").click();
            });

            $("#CartelasResgatesDocumentoArrastarSoltar-Input-Imagem").on("change", function () {
                selectedFile = this.files[0];
                showPreview(selectedFile);
                addFileToForm(selectedFile);
            });

            function showPreview(file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#drop-zone").css("background-image", "url(" + e.target.result + ")");
                    $("#drop-zone").css("background-size", "contain");
                    $("#drop-zone").css("background-position", "center");
                    $("#drop-zone").css("background-repeat", "no-repeat");
                    $("#drop-zone").text('');
                }
                reader.readAsDataURL(file);
            }

            function addFileToForm(file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#ImgBase64").val(e.target.result);
                }
                reader.readAsDataURL(file);
            }


        } else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function CartelasResgatesDocumentoArrastarSoltarAtualizar(idForm) {

    var Mensagem = "Enviar Documento?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        var nomeModal = 'CartelasResgatesDocumentoArrastarSoltar';

        var form = new FormData($("form[id='" + idForm + "']")[0]);

        if (resposta === true) {

            let url = "/Cadastro/CartelasResgatesDocumentoArrastarSoltarAtualizar";

            Submeter('PUT', url, form, false).then((model) => {

                if (model.html.retorno.sucesso === true) {

                    CartelasResgatesTabVer('model-tabs-Cartelas-resgate', model.html.codigoCartela);
                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                } else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }

                FecharModal('modal-' + nomeModal);

            });

        }
    });
}
function CartelasResgatesDocumentoVisualizar(codigoCartela, tipoDocumento) {

    var nomeModal = 'CartelasResgatesDocumentoVisualizar';

    let url = "/Cadastro/CartelasResgatesDocumentoVisualizarVer?CodigoCartela=" + codigoCartela + "&TipoDocumento=" + tipoDocumento;

    Submeter('GET', url, null).then((retorno) => {
        if (retorno.sucesso === true) {
            $("#div-principal").append(retorno.html);
            $("#modal-" + nomeModal).modal('show');
        } else {
            MessageBoxError(retorno.status, retorno.mensagem);
        }
    });
}
function CartelasDeclaracoesResgatesPDF(codigoCartela) {

    var Mensagem = "Gerar Declaração?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var url = "/Relatorio/CartelasDeclaracoesResgatesPDF?CodigoCartela=" + codigoCartela;

            Submeter('POST', url, null, false).then((model) => {

                if (model.html.retorno.sucesso == true) {
                    RelatorioBaixar(model.html.nomeArquivo, model.html.contentType, model.html.base64);
                } else {
                    MessageBoxError(model.html.retorno.sucesso, model.html.retorno.mensagem);
                }

            });

        }

    });
}
function BoletosGerarPDF(codigoBoleto) {

    var Mensagem = "Gerar PDF do Boleto?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var url = "/Relatorio/BoletosGerarPDF?CodigoBoleto=" + codigoBoleto;

            Submeter('POST', url, null, false).then((model) => {

                if (model.html.retorno.sucesso == true) {
                    RelatorioBaixar(model.html.nomeArquivo, model.html.contentType, model.html.base64);
                } else {
                    MessageBoxError(model.html.retorno.sucesso, model.html.retorno.mensagem);
                }

            });

        }

    });
}
function BoletosGerarCarnePDF(codigoCartela) {

    var Mensagem = "Gerar Carnê da Cartela?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var url = "/Relatorio/BoletosGerarCarnePDF?CodigoCartela=" + codigoCartela;

            Submeter('POST', url, null, false).then((model) => {

                if (model.html.retorno.sucesso == true) {
                    RelatorioBaixar(model.html.nomeArquivo, model.html.contentType, model.html.base64);
                } else {
                    MessageBoxError(model.html.retorno.sucesso, model.html.retorno.mensagem);
                }

            });

        }

    });
}


//Boletos

function CarregarModalBoletosAtualizacao(controller, actionTabela, codigoSorteio, codigoAgente, numeroCartela) {

    var nomeModal = actionTabela.replace('Tabela', '')
    var action = nomeModal;

    if ($("#modal-" + nomeModal).length > 0) {
        $("#modal-" + nomeModal).remove();
    };

    let url = "/" + controller + "/" + action + "?CodigoSorteio=" + codigoSorteio + "&CodigoAgente=" + codigoAgente + "&NumeroCartela=" + numeroCartela;

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

function CarregarModalBoletosAtualizacaoAtualizar(idForm, controller, action) {

    var Mensagem = "Verificar status do(s) boleto(s) junto ao Santander?";

    MessageBoxConfirmacao(Mensagem).then((resposta) => {

        if (resposta == true) {

            var form = new FormData($("form[id='" + idForm + "']")[0]);

            let url = "/" + controller + "/" + action;

            Submeter('PUT', url, form, false).then((model) => {
                if (model.html.retorno.sucesso == true) {

                    $('#modal-' + action).modal('hide');

                    if (VerificarSeElementoExiste('tabela-grid-Boletos')) {
                        CarregarGrid('tabela-grid-Boletos', controller, 'BoletosGrid', "Boletos");
                    } else {
                        AgentesCartelasTabVer('model-tabs-Agentes-cartelas', model.html.codigoAgente);
                    }

                    MessageBoxToastSalvar(model.html.retorno.sucesso, model.html.retorno.mensagem);

                }
                else {
                    MessageBoxToastSalvar(model.html.retorno.status, model.html.retorno.mensagem);
                }
            });

        }

    });


};

function ElementoExiste(id) {
    return $("#" + id).length > 0;
}

function FecharModal(idModal) {
    var $modal = $('#' + idModal);
    $modal.modal('hide');
    $modal.on('hidden.bs.modal', function () {
        $modal.remove();
    });
}
