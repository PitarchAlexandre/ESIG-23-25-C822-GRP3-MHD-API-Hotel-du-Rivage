<?php


function connectDb()
{


    $user = 'ih1o2_pi02db3';
    $pass = 'P_XUhBsKxM0';
    $base = "ih1o2_pi02db3";

    $dsn = 'mysql:host=ih1o2.myd.infomaniak.com;dbname=' . $base . ';charset=UTF8';
    try {

        $dbh = new PDO($dsn, $user, $pass);
        /*** les erreurs sont gérées par des exceptions ***/
        $dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    } catch (PDOException $e) {
        print "erreur ! :" . $e->getMessage() . "<br/>";
        die();
    }

    return $dbh;
}

var_dump($_POST);
if(isset($_POST["submit"])) {
    $ch1 = filter_input(INPUT_POST, "nom", FILTER_SANITIZE_SPECIAL_CHARS);
    $ch2 = filter_input(INPUT_POST, "prenom", FILTER_SANITIZE_SPECIAL_CHARS);
    $ch3 = filter_input(INPUT_POST, "rue", FILTER_SANITIZE_SPECIAL_CHARS);
    $ch4= filter_input(INPUT_POST, "telephone", FILTER_SANITIZE_SPECIAL_CHARS);
    $ch5 = filter_input(INPUT_POST, "email", FILTER_SANITIZE_SPECIAL_CHARS);
    $ch6 = filter_input(INPUT_POST, "dateNaissance", FILTER_SANITIZE_SPECIAL_CHARS);
    $ch7 = filter_input(INPUT_POST, "localite", FILTER_SANITIZE_SPECIAL_CHARS);





    //rédaction de la requête

    $sql ="INSERT INTO tb_clients 
        ( 
            nom_cli,
            pre_cli,
            rue_cli,
            tel_cli,
            mail_cli,
            datNais_cli,
            fk_cli_loc
        )
        VALUES 
        ( :ch1, :ch2, :ch3, :ch4, :ch5, :ch6, :ch7)";
    var_dump($sql);

    //ouverture de la connexion
    $dbh = connectDb();


    // la compilation de la requête sur le serveur retour un objet PDOStatment qui représente la requête sur le serveur
    $stmt = $dbh->prepare($sql);

    //association du marqueur à une variable (E/S)
    //lier les données
    $stmt->bindParam(':ch1',$ch1,PDO::PARAM_STR);
    $stmt->bindParam(':ch2',$ch2,PDO::PARAM_STR);
    $stmt->bindParam(':ch3',$ch3,PDO::PARAM_STR);
    $stmt->bindParam(':ch4',$ch4,PDO::PARAM_STR);
    $stmt->bindParam(':ch5',$ch5,PDO::PARAM_STR);
    $stmt->bindParam(':ch6',$ch6,PDO::PARAM_STR);
    $stmt->bindParam(':ch7',$ch7,PDO::PARAM_STR);


    //exécution de la requête
    $stmt->execute();
    echo $stmt->rowCount();


    //redirection
    header("location:../index.html");






}