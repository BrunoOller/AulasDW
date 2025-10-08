<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Exercicio 1</title>
    <link rel="stylesheet" href="../../assets/css/reset.css">
    <link rel="stylesheet" href="../../assets/css/exStyle.css">
</head>

<body>

    <div class="content">
        <?php
            $name = $_GET["name"];
            $age = $_GET["age"];
            $email = $_GET["email"];
            $tel = $_GET["tel"];
            echo "<strong>$name</strong> tem <strong>$age</strong> anos. Seu email: <strong>$email</strong> e telefone: <strong>$tel</strong>";
            echo "<br> <a href='./'>Voltar</a>";
        ?>
    </div>
</body>

</html>