// Criando constantes pegando os elementos do HTML
const display = document.getElementById('display');
const buttons = document.querySelectorAll('.num');

// buttons.forEach vai pegar todos os elementos buttons, que no caso no HTML são os botões com classes num.
// Isso facilita, pois meio que aplica a todos esses elementos de uma vez, sem ter que criar varias consts
// E varios addEventListener pra cada.
buttons.forEach(button => {

    // Aqui eu tô adicionando um evento que quando eu clicar no botão, ele vai verificar isso e vai realizar o que tiver dentro
    button.addEventListener('click', (event) => {

        // O event.preventDefault retira a possibilidade de eu clicar em algum botão e ele atualizar a página derrepente
        // Pois ai reseta tudo.
        event.preventDefault();
        const value = button.getAttribute('data-value');

        // Estrutura condicional, estou verificando se o valor será o "C", se sim ->
        if (value === 'C') {
            display.value = ''; // Vai limpar o display
        } else if (value === '=') { // Aqui se o valor for "=" ->
            calculate(); // Vai realizar o cálculo
        } else { // Se não for nenhum dos valores acima ->
            display.value += value; // Vai permitir que adicione mais valores
        }
    });
});

// Função que vai realizar o cálculo
function calculate() {
    // Aqui cria a função de cálculo e se não funcionar, ou der algum erro, vai pro catch e ele adiciona uma mensagem de erro
    try {
        display.value = new Function("return " + display.value)();
    } catch {
        display.value = 'Error';
    }
};