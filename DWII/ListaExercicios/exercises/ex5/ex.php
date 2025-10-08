<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Exercicio 5</title>
    <link rel="stylesheet" href="../../assets/css/reset.css">
    <link rel="stylesheet" href="../../assets/css/exStyle.css">
</head>

<body>
    <div class="content">
        <?php
            $num = $_GET["num"];
            $soma = 0;

            for ($i = 1; $i <= $num; $i++) {
                $soma += $i;
            }

            echo "A <strong>soma</strong> de <strong>1</strong> até <strong>$num</strong> é: <strong>$soma</strong>";
            echo "<br> <a href='./'>Voltar</a>";
        ?>
    </div>
</body>

</html>