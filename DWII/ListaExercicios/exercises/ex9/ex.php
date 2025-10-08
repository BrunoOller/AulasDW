<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Exercicio 9</title>
    <link rel="stylesheet" href="../../assets/css/reset.css">
    <link rel="stylesheet" href="../../assets/css/exStyle.css">
</head>
<body>
    <div class="content">
        <?php
            if (isset($_GET['num'])) {
                $num = $_GET['num'];
                echo "<strong>Arredondado para cima:</strong> " . ceil($num) . "<br>";
                echo "<strong>Arredondado para baixo:</strong> " . floor($num) . "<br>";
            } else {
                echo "<strong>Nenhum número foi enviado.</strong>";
            }
            echo "<br> <a href='./'>Voltar</a>";
        ?>
    </div>
</body>
</html>