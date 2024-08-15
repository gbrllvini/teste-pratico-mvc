$(document).ready(function () {
    var viewModel = {
        pessoas: ko.observableArray([]),
        id: ko.observable(""),
        nome: ko.observable(""),
        sobrenome: ko.observable(""),
        dataNascimento: ko.observable(""),
        estadoCivil: ko.observable(""),
        cpf: ko.observable(""),
        rg: ko.observable(""),


        // rotas
        detalhes: function (id) {
            sessionStorage.setItem("Id", id);
            window.location.href = '/pessoas/detalhes/' + id;
        },
        editar: function (id) {
            sessionStorage.setItem("Id", id);
            window.location.href = '/pessoas/editar/' + id;
        },
        excluir: function (id) {
            sessionStorage.setItem("Id", id);
            window.location.href = '/pessoas/excluir/' + id;
        },

        // get
        getPessoas: function () {
            $.ajax({
                url: '/pessoas/get',
                type: 'GET',
                dataType: 'json',
                success: function (pessoasList) {
                   pessoasList.forEach(function (pessoa) {
                       pessoa.DataNascimento = viewModel.formataDataList(pessoa.DataNascimento);
                    });
                    viewModel.pessoas(pessoasList);                 
                },
                error: function (xhr, status, error) {
                    alert("Erro ao exibir lista de pessoas: " + error);
                }
            });
        },

         // getById
        loadPessoaForm: function (id) {
            $.ajax({
                url: '/pessoas/pessoa/' + id,
                type: 'GET',
                dataType: 'json',
                success: function (pessoa) {
                    viewModel.id(pessoa.Id);
                    viewModel.nome(pessoa.Nome);
                    viewModel.sobrenome(pessoa.Sobrenome);
                    viewModel.dataNascimento(
                        viewModel.formataDataForm(pessoa.DataNascimento)
                    );
                    viewModel.estadoCivil(pessoa.EstadoCivil);
                    viewModel.cpf(pessoa.Cpf);
                    viewModel.rg(pessoa.Rg);
                },
                error: function (xhr, status, error) {
                    alert("Erro ao carregar pessoa: " + error);
                }
            });
        },

        loadPessoaList: function (id) {
            $.ajax({
                url: '/pessoas/pessoa/' + id,
                type: 'GET',
                dataType: 'json',
                success: function (pessoa) {
                    viewModel.id(pessoa.Id);
                    viewModel.nome(pessoa.Nome);
                    viewModel.sobrenome(pessoa.Sobrenome);
                    viewModel.dataNascimento(
                        viewModel.formataDataList(pessoa.DataNascimento)
                    );
                    viewModel.estadoCivil(pessoa.EstadoCivil);
                    viewModel.cpf(pessoa.Cpf);
                    viewModel.rg(pessoa.Rg);
                },
                error: function (xhr, status, error) {
                    alert("Erro ao carregar pessoa: " + error);
                }
            });
        },

        // create
        createPessoa: function () {
            var novaPessoa = {
                Nome: viewModel.nome(),
                Sobrenome: viewModel.sobrenome(),
                DataNascimento: viewModel.dataNascimento(),
                EstadoCivil: viewModel.estadoCivil(),
                Cpf: viewModel.removeMask(viewModel.cpf()),
                Rg: viewModel.removeMask(viewModel.rg())
            };
            $.ajax({
                url: '/pessoas/nova',
                type: 'POST',
                dataType: 'json',
                data: novaPessoa,
               
                success: function (resposta) {
                    if (resposta.Sucesso) {
                        alert("Pessoa cadastrada com sucesso!");
                        window.location.href = '/pessoas/index/'
                    } else {
                        alert("Erro ao cadastrar pessoa: " + resposta.Mensagem);
                    }
                },
                error: function () {
                    alert("Erro ao cadastrar pessoa, verifique os dados e tente novamente." );
                }

            });
            
        },

        // edit
        editPessoa: function(){
            var pessoaEditada = {
                Id: viewModel.id(),
                Nome: viewModel.nome(),
                Sobrenome: viewModel.sobrenome(),
                DataNascimento: viewModel.dataNascimento(),
                EstadoCivil: viewModel.estadoCivil(),
                Cpf: viewModel.removeMask(viewModel.cpf()),
                Rg: viewModel.removeMask(viewModel.rg())
            };
            $.ajax({
                url: '/pessoas/editar/' + viewModel.id(),
                type: 'POST',
                dataType: 'json',
                data: pessoaEditada,
               
                success: function (resposta) {
                    if (resposta.Sucesso) {
                        alert("Pessoa editada com sucesso!");
                        window.location.href = '/pessoas/index/'
                    } else {
                        alert("Erro ao editar pessoa: " + resposta.Mensagem);
                    }
                },
                error: function () {
                    alert("Erro ao editar pessoa, verifique os dados e tente novamente." );
                }
            });
        },

        removeMask: function(value) {
           return value.replace(/\D/g, '');
        },

        formataDataList: function (dateString) {
            var timestamp = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'), 10);
            var date = new Date(timestamp);
            return date.toLocaleDateString();
        },

        formataDataForm: function (dateString) {
            // checa se esta no formato dd/mm/yyyy
            var regex = /^(\d{2})\/(\d{2})\/(\d{4})$/;
            var match = dateString.match(regex);
            if (match) {
                var dia = match[1];
                var mes = match[2];
                var ano = match[3];
                return ano + '-' + mes + '-' + dia;
            } else {
                var timestamp = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'), 10);
                var date = new Date(timestamp);
                var ano = date.getFullYear();
                var mes = ("0" + (date.getMonth() + 1)).slice(-2);
                var dia = ("0" + date.getDate()).slice(-2);
                return ano + '-' + mes + '-' + dia;
            }
        },
    };

    // aplica bindings
    ko.applyBindings(viewModel);

     // carrega pessoa no formulario de edicao
    if (window.location.pathname.startsWith('/pessoas/editar/')) {
        var pessoaId = sessionStorage.getItem("Id");
        if (pessoaId) {
            viewModel.loadPessoaForm(pessoaId);
        } else {
            alert("Id da pessoa não encontrado.");
        }
    }

    // carrega pessoa na tela de detalhes
    if (window.location.pathname.startsWith('/pessoas/detalhes/')) {
        var pessoaId = sessionStorage.getItem("Id");
        if (pessoaId) {
            viewModel.loadPessoaList(pessoaId);
        } else {
            alert("Id da pessoa não encontrado.");
        }
    }

    // carrega pessoa na tela de delete
    if (window.location.pathname.startsWith('/pessoas/excluir/')) {
        var pessoaId = sessionStorage.getItem("Id");
        if (pessoaId) {
            viewModel.loadPessoaList(pessoaId);
        } else {
            alert("Id da pessoa não encontrado.");
        }
    }

    $('#cpf').mask('000.000.000-00', { reverse: true });
    $('#rg').mask('00.000.000-0', { reverse: true });

    viewModel.getPessoas();
  
});
