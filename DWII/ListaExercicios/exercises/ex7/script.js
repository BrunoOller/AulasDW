function calcularMedias() {
    let form = document.getElementById('formNotas');
    let notas = [
        [parseFloat(form['aluno1[]'][0].value), parseFloat(form['aluno1[]'][1].value)],
        [parseFloat(form['aluno2[]'][0].value), parseFloat(form['aluno2[]'][1].value)],
        [parseFloat(form['aluno3[]'][0].value), parseFloat(form['aluno3[]'][1].value)]
    ];

    let resultado = '';
    for (let i = 0; i < notas.length; i++) {
        let media = (notas[i][0] + notas[i][1]) / 2;
        resultado += `Média do aluno ${i + 1}: ${media.toFixed(2)}<br>`;
    }

    document.getElementById('resultado').innerHTML = resultado;
}