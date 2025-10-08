<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Exercicio 3</title>
    <link rel="stylesheet" href="../../assets/css/reset.css">
    <link rel="stylesheet" href="../../assets/css/exStyle.css">
</head>

<body>

    <div class="content">
        <?php
            $b = $_GET["base"];
            $h = $_GET["altura"];
            $area = $b * $h;
            $per = 2 * ($b + $h);

            echo "<strong>Área:</strong> $area <br>";
            echo "<strong>Perimetro:</strong> $per";
            echo "<br> <a href='./'>Voltar</a>";
        ?>
    </div>
</body>

</html>