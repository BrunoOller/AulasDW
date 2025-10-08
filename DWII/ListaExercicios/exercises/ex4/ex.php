<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Exercicio 4</title>
    <link rel="stylesheet" href="../../assets/css/reset.css">
    <link rel="stylesheet" href="../../assets/css/exStyle.css">
</head>

<body>
    <div class="content">
        <?php
            $nota = $_GET["nota"];
            $por = $_GET["por"];

            if ($nota >= 6) {
                echo "<strong>APROVADO!</strong>";
            } elseif ($nota >= 4 && $nota < 6) {
                echo "<strong>SEGUNDA ÉPOCA!</strong>";
            } else {
                echo "<strong>REPROVADO!</strong>";
            }

            echo "<br> <a href='./'>Voltar</a>";
            
            // COMPLETO, ADICIONANDO A PORCENTAGEM DE PRESENÇA
            // PARA DEFINIR SE O ALUNO FOI APROVADO OU REPROVADO
            /* if ($nota >= 6 || $por >= 80) {
                echo "APROVADO!";
            } elseif ($nota >= 4 && $nota < 6) {
                echo "SEGUNDA ÉPOCA!";
            } else {
                echo "REPROVADO!";
            } */
        ?>
    </div>
</body>

</html>