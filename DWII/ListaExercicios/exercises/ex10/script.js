let numeros = [];
let pares = 0;

function adicionarNumero() {
    let n = parseInt(document.getElementById("num").value);
    document.getElementById("num").value = "";

    if (n === 0) {
        document.getElementById("resultado").innerHTML = `
          <strong>Você digitou ${pares} número(s) par(es).</strong>
        `;
    } else {
        numeros.push(n);
        if (n % 2 === 0) {
            pares++;
        }
    }
}