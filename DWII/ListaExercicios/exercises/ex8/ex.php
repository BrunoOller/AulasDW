<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Exercicio 8</title>
    <link rel="stylesheet" href="../../assets/css/reset.css">
    <link rel="stylesheet" href="../../assets/css/exStyle.css">
</head>
<body>
    <div class="content">
        <?php
            if (isset($_GET['num'])) {
                $num = $_GET['num'];
                $fatorial = 1;
                for ($i = $num; $i >= 1; $i--) {
                    $fatorial *= $i;
                }
                echo "O <strong>fatorial</strong> de <strong>$num</strong> é: <strong>", number_format($fatorial, 2, ",", "."),"</strong>";
            } else {
                echo "<strong>Nenhum número foi enviado.</strong>";
            }
            echo "<br> <a href='./'>Voltar</a>";
        ?>
    </div>
</body>
</html>