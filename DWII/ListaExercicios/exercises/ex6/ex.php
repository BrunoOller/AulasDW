<!DOCTYPE html>
<html lang="pt-br">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Exercicio 6</title>
    <link rel="stylesheet" href="../../assets/css/reset.css">
    <link rel="stylesheet" href="../../assets/css/exStyle.css">
</head>

<body>

    <div class="content">
        <?php
            if (isset($_GET['num'])) {
                $nums = $_GET['num'];
                $media = array_sum($nums) / count($nums);
                echo "A <strong>média</strong> dos números é: <strong>", number_format($media, 2), "</strong>";
            } else {
                echo "<strong>Nenhum número foi enviado.</strong>";
            }

            echo "<br> <a href='./'>Voltar</a>";
        ?>
    </div>
</body>

</html>