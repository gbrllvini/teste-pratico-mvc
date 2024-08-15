$(document).ready(function () {
    var viewModel = {
        pessoas: ko.observableArray([]),
        nome: ko.observable(""),
        sobrenome: ko.observable(""),
        dataNascimento: ko.observable(""),
        estadoCivil: ko.observable(""),
        cpf: ko.observable(""),
        rg: ko.observable(""),


        // rotas
        detalhes: function (id) {
            window.location.href = '/pessoas/detalhes/' + id;
        },
        editar: function (id) {
            window.location.href = '/pessoas/editar/' + id;
        },
        excluir: function (id) {
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
                        pessoa.DataNascimento = viewModel.formataData(pessoa.DataNascimento);
                    });
                    viewModel.pessoas(pessoasList); 
                },
                error: function (xhr, status, error) {
                    alert("Erro ao exibir lista de pessoas: " + error);
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
                Cpf: viewModel.cpf(),
                Rg: viewModel.rg()
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
                        viewModel.getPessoas();
                    } else {
                        alert("Erro ao cadastrar pessoa: " + resposta.Mensagem);
                    }
                },
                error: function (xhr, status, error) {
                    alert("Erro ao cadastrar pessoa." , error);
                }

            });
            
        },

        formataData: function (dateString) {
            var timestamp = parseInt(dateString.replace(/\/Date\((\d+)\)\//, '$1'), 10);
            var date = new Date(timestamp);
            return date.toLocaleDateString();
        },
    };

    // aplica bindings
    ko.applyBindings(viewModel);

    viewModel.getPessoas();
});
