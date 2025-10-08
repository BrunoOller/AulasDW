<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Exercicio 2</title>
    <link rel="stylesheet" href="../../assets/css/reset.css">
    <link rel="stylesheet" href="../../assets/css/exStyle.css">
</head>

<body>

    <div class="content">
        <?php
            $v1 = $_GET["v1"];
            $v2 = $_GET["v2"];
            $som = $v1 + $v2;
            $sub = $v1 - $v2;
            $mult = $v1 * $v2;
            $div = $v1 / $v2;

            echo "<strong>Resultado dos Cálculos</strong> entre <strong>$v1</strong> e <strong>$v2</strong> <br>";
            echo "$v1 + $v2 = <strong>$som</strong> <br>";
            echo "$v1 - $v2 = <strong>$sub</strong> <br>";
            echo "$v1 * $v2 = <strong>$mult</strong> <br>";
            echo "$v1 / $v2 = <strong>", number_format($div, 2, ",", "."), "</strong>";
            echo "<br> <a href='./'>Voltar</a>";
        ?>
    </div>
</body>

</html>