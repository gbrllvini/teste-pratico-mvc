$(document).ready(function () {
    var viewModel = {
        pessoas: ko.observableArray([]),

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
